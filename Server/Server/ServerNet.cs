using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

using PublicLib;
using SyncData;

/***
 * author:lichunlei
 */
namespace Server
{
	/// <summary>
	/// 服务器TcpListener
	/// </summary>
	public class ServerNet : Singleton<ServerNet>
	{
		private TcpListener m_listener;
		private byte[] m_receiveData = new byte[1024];

		private Dictionary<string, ServerClient> m_clientDic = new Dictionary<string, ServerClient>();
		public ServerNet()
		{
			InitServer();
		}

		public void InitServer()
		{
			try
			{
				m_listener = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000));
				m_listener.Server.NoDelay = true;
				m_listener.Start(10);

				ServerLog.Log("Server Start OK!");
				m_listener.BeginAcceptTcpClient(new AsyncCallback(AsyncListen), m_listener);
			}
			catch (Exception exp)
			{
				Console.WriteLine("Wrong When AcceptTcp Client:{0}", exp.Message);
			}
		}

		private void AsyncListen(IAsyncResult result)
		{
			try
			{
				TcpClient client = m_listener.EndAcceptTcpClient(result);
				if (client != null && client.Connected)
				{
					ServerClient sclient = ServerClient.CreateClient(client);
					ServerClient clientValue;
					if (!m_clientDic.TryGetValue(sclient.MIpEndPointString, out clientValue))
					{
						m_clientDic.Add(sclient.MIpEndPointString, sclient);
					}
					Console.WriteLine("{0} Enter Server , {1}Clients Connected", client.Client.RemoteEndPoint.ToString(), m_clientDic.Count);

					sclient.StartRead();
				}
			}
			catch (SocketException exp)
			{
				Console.WriteLine("Wrong When EndAccept:{0}", exp.Message);
			}

			try
			{
				m_listener.BeginAcceptTcpClient(new AsyncCallback(AsyncListen), m_listener);
			}
			catch (SocketException exp)
			{
				Console.WriteLine("Keep Accept Wrong:{0}", exp.Message);
			}
		}

		public void SendMsg(string endPoint, ProtocolEnum protocolId, object data)
		{
			ServerClient client = GetSClient(endPoint);
			if (client != null)
			{
				try
				{
					client.SendMsg(protocolId, data);
				}
				catch (Exception exp)
				{
					ServerLog.Log(string.Format("Send To :{0}, Error:{1}", endPoint, exp.Message));
				}
			}
		}

		/// <summary>
		/// 获取一个连接
		/// </summary>
		/// <param name="endPoint"></param>
		/// <returns></returns>
		public ServerClient GetSClient(string endPoint)
		{
			ServerClient client = null;
			if (!m_clientDic.TryGetValue(endPoint, out client))
			{
				return null;
			}
			return client;
		}

		/// <summary>
		/// 删除一个连接
		/// </summary>
		/// <param name="endPoint"></param>
		public void RemoveSClient(string endPoint)
		{
			if (m_clientDic.ContainsKey(endPoint))
			{
				m_clientDic.Remove(endPoint);
				ServerLog.Log(string.Format("{0}Has Lost Link", endPoint));
			}
		}

		private void CloseServer()
		{
			m_listener.Stop();
		}
	}
}
