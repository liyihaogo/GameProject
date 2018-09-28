using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;

using UnityEngine;

using ProtoBuf;
using SyncData;

/***
 * author:lichunlei
 */
namespace PublicLib
{
	/// <summary>
	/// Tcp代理
	/// </summary>
	public class TcpNetProxy
	{
		public delegate void OnDisconnect();
		public OnDisconnect onDisconnect;

		private TcpClient m_client = null;
		private byte[] m_receiveData = null;

		private int m_maxBuffSize = 1024 * 4;
		private int m_receiveOffSet = 0;	//防止粘包问题
		public byte[] MReceiveData
		{
			get { return m_receiveData; }
			set { m_receiveData = value; }
		}

		public TcpNetProxy(TcpClient client)
		{
			m_client = client;
			m_client.NoDelay = true;
			m_receiveOffSet = 0;
			m_receiveData = new byte[m_maxBuffSize];
		}

		public void StartAsyncRead()
		{
			if (!CheckIsConnect) return;
			KeepRead();
		}

		/// <summary>
		/// 判断连接状态，并处理
		/// </summary>
		private bool CheckIsConnect
		{
			get
			{
				if (IsConnected)
				{
					return true;
				}
				else
				{
					CloseTcp();
					return false;
				}
			}
		}

		/// <summary>
		/// 判断是否连接还在
		/// </summary>
		public bool IsConnected
		{
			get
			{
				try
				{
					//return _tcpClient!= null && _tcpClient.Client!= null && _tcpClient.Client.Connected;
					if (m_client != null && m_client.Client != null && m_client.Client.Connected)
					{
						/* As the documentation:
						 * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
						 * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
						 * -or- true if data is available for reading; 
						 * -or- true if the connection has been closed, reset, or terminated; 
						 * otherwise, returns false
						 */
						//Detect if client disconnected
						if (m_client.Client.Poll(0, SelectMode.SelectRead))
						{
							byte[] buff = new byte[1];
							if (m_client.Client.Receive(buff, SocketFlags.Peek) == 0)
							{
								//Client disconnected
								return false;
							}
							else
							{
								return true;
							}
						}
						return true;
					}
					else
					{
						return false;
					}
				}
				catch
				{
					return false;
				}
			}
		}
		private void OnAsyncRead(IAsyncResult result)
		{
			if (!CheckIsConnect) return;

			try
			{
				NetworkStream stream = result.AsyncState as NetworkStream;
				int byteLen = stream.EndRead(result);
				ServerLog.Log(string.Format("收到：{0}", byteLen));
				GoOnRead(m_receiveData, byteLen);
			}
			catch (SocketException exp)
			{
				AnalyzeException(exp);
			}
		}

		private void GoOnRead(byte[] buffBytes, int remainLen)
		{
			MemoryStream readStream = new MemoryStream();
			readStream.Write(buffBytes, 0, remainLen);
			readStream.Position = 0;

			BinaryReader breader = new BinaryReader(readStream);
			int totalLen = breader.ReadInt32();
			//发生粘包
			if (totalLen <= remainLen)
			{
				//协议id
				int protoId = breader.ReadInt32();
				//协议体
				byte[] protoBody = breader.ReadBytes(totalLen - sizeof(int) * 2);

				ServerLog.Log(string.Format("消息长度：{0}, 消息Id:{1}", totalLen, protoId));
				Type protoType = ProtocolMgr.MInstance.GetTypeByProtoId((ProtocolEnum)protoId);
				if (protoType == null)
				{
					Console.WriteLine("No Such A Type Protocol Id : {0}", protoId);
					return;
				}

				using (MemoryStream pstream = new MemoryStream())
				{
					pstream.Write(protoBody, 0, protoBody.Length);
					pstream.Position = 0;
					object obj = Serializer.NonGeneric.Deserialize(protoType, pstream);

					ProtocolData pData = ProtocolData.MakeProtocol((ProtocolEnum)protoId, obj, m_client.Client.RemoteEndPoint.ToString());
					NetMsgDispatch.MInstance.DispathMsg((ProtocolEnum)protoId, pData);
				}

				//剩下的
				int leftLen = remainLen - totalLen;
				ServerLog.Log(string.Format("还需读取：{0}", leftLen));
				if (leftLen > 0)
				{
					byte[] remainBytes = breader.ReadBytes(leftLen);
					readStream.Close();
					GoOnRead(remainBytes, leftLen);
				}
				else
					KeepRead();
			}
			else
			{
				m_receiveOffSet = remainLen;
				KeepRead();
			}
		}
		private void KeepRead()
		{
			if (!CheckIsConnect) return;
			try
			{
				m_client.GetStream().BeginRead(m_receiveData, m_receiveOffSet, m_receiveData.Length, new AsyncCallback(OnAsyncRead), m_client.GetStream());
			}
			catch (SocketException exp)
			{
				AnalyzeException(exp);
			}
		}

		/// <summary>
		/// 发送协议数据
		/// </summary>
		/// <param name="pid">协议id</param>
		/// <param name="pdata">协议体</param>
		public void SendMsg(ProtocolEnum pid, object pdata)
		{
			if (!CheckIsConnect) return;
			Type ptype = ProtocolMgr.MInstance.GetTypeByProtoId(pid);
			if (ptype == null)
			{
				Console.WriteLine("SendMsg-> No Such One Proto,Id:{0}", pid);
				return;
			}

			//消息构成：消息总长度+消息id+消息体

			//消息体
			MemoryStream pbodyStream = new MemoryStream();
			Serializer.NonGeneric.Serialize(pbodyStream, pdata);
			byte[] bodyBytes = pbodyStream.ToArray();
			//消息头长度
			int pheader = sizeof(int) * 2;
			//消息总长度
			int plength = bodyBytes.Length + pheader;

			using (MemoryStream pstream = new MemoryStream())
			{
				BinaryWriter bwriter = new BinaryWriter(pstream);
				bwriter.Write(plength);
				bwriter.Write((int)pid);
				bwriter.Write(bodyBytes);

				pstream.Position = 0;
				byte[] content = pstream.ToArray();
				m_client.GetStream().Write(content, 0, content.Length);
				m_client.GetStream().Flush();
			}

			pbodyStream.Close();
		}

		public void CloseTcp()
		{
			if (onDisconnect != null)
				onDisconnect();
			if (m_client != null)
			{
				m_client.Close();
				m_client = null;
			}
		}

		private void AnalyzeException(SocketException exp)
		{
			Console.WriteLine("Socket ErrorCode:{0}, The Message Is :{1}", exp.ErrorCode, exp.Message);
			switch (exp.ErrorCode)
			{
				//断开连接
				case 10054:
					Console.WriteLine("Connect Has Disconnect");
					break;
			}
		}
	}
}
