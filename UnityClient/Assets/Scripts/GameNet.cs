using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;

using PublicLib;
using SyncData;
using ProtoBuf;
using UnityEngine;
using Game.Module;

/***
 * author:lichunlei
 */
namespace Game.Net
{
	/// <summary>
	/// 网络连接
	/// </summary>
	public class GameNet : Singleton<GameNet>
	{
		public delegate void OnConnected(bool ok);
		public OnConnected onConnected;

		private Thread m_connectThread = null;
		private Thread m_heartBeatThread = null;

		private int m_connectTimes = 1;         //尝试连接服务器次数
		private int m_port = 12000;
		private string m_ip = "127.0.0.1";

		private TcpNetProxy m_netProxy = null;
		private TcpClient m_tcpClient = null;
		private int m_connectTimeOut = 1000;
		private bool m_hasConnectServer = false;
		private bool m_gameOver = false;

		private HeartBeatModule m_heartBeatModule;
		public int MConnectTimeOut { get { return m_connectTimeOut; } }
		public void SetIpPort(string ip, int port)
		{
			m_ip = ip;
			m_port = port;
		}

		public GameNet()
		{
			m_heartBeatModule = GameModuleMgr.MInstance.FindModule(HeartBeatModule.ModuleName) as HeartBeatModule;
		}

		/// <summary>
		/// 开始连接服务器
		/// </summary>
		public void StartConnect()
		{
			AbortHeartBeatThread();
			AbortConnectThread();

			m_connectTimes = 1;
			m_hasConnectServer = false;
			m_connectThread = new Thread(TryConnectServer);
			m_connectThread.IsBackground = true;
			m_connectThread.Start();
		}

		/// <summary>
		/// 向服务器发送协议
		/// </summary>
		/// <param name="pId"></param>
		/// <param name="pBody"></param>
		public void SendMsg(ProtocolEnum pId, object pBody)
		{
			if (m_netProxy != null)
			{
				if (m_netProxy.IsConnected)
					m_netProxy.SendMsg(pId, pBody);
				else
					Debug.Log("Client Has Closed");
			}
		}

		public override void Clear()
		{
			base.Clear();
			AbortHeartBeatThread();
			AbortConnectThread();
			CloseTcp();
			m_gameOver = true;
		}

		/// <summary>
		/// 开始心跳
		/// </summary>
		private void StartHeartBeat()
		{
			AbortHeartBeatThread();
			m_heartBeatThread = new Thread(TryHeartBeat);
			m_heartBeatThread.IsBackground = true;
			m_heartBeatThread.Start();
		}

		private void TryConnectServer()
		{
			while (true)
			{
				if (m_gameOver)
					break;
				if (m_hasConnectServer)
				{
					AbortConnectThread();
					break;
				}
			
				CloseTcp();

				GameLog.Log("Connect Server Times:" + m_connectTimes);
				m_tcpClient = new TcpClient();
				m_tcpClient.NoDelay = true;
				try
				{
					m_tcpClient.BeginConnect(IPAddress.Parse(m_ip), m_port, new AsyncCallback(OnAsyncConnected), m_tcpClient);
				}
				catch (Exception exp)
				{
					GameLog.Log(string.Format("Fail Connect Server:{0}, ReConnect", exp.Message));
					StartConnect();
					break;
				}
				m_connectTimes++;
				Thread.Sleep(m_connectTimeOut);
			}
		}

		private void TryHeartBeat()
		{
			while (true)
			{
				if (m_gameOver)
					break;
				if (!m_netProxy.IsConnected)
				{
					AbortHeartBeatThread();
					StartConnect();
					GameLog.Log("Begin ReConnect");
					break;
				}
				
				m_heartBeatModule.RequestHeartBeating();
				Thread.Sleep(m_heartBeatModule.MHeartBeatingSpace);
			}
		}

		/// <summary>
		/// 取消心跳
		/// </summary>
		private void AbortHeartBeatThread()
		{
			if (m_heartBeatThread != null)
				m_heartBeatThread.Abort();
			m_heartBeatThread = null;
		}

		/// <summary>
		/// 取消连接
		/// </summary>
		private void AbortConnectThread()
		{
			if (m_connectThread != null)
				m_connectThread.Abort();
			m_connectThread = null;
		}

		private void OnAsyncConnected(IAsyncResult result)
		{
			TcpClient client = result.AsyncState as TcpClient;
			client.EndConnect(result);
			m_hasConnectServer = true;
			m_connectTimes = 1;

			if (onConnected != null)
				onConnected(true);

			m_netProxy = new TcpNetProxy(client);
			m_netProxy.onDisconnect = OnDisconnectNet;
			m_netProxy.StartAsyncRead();

//			StartHeartBeat();
		}

		/// <summary>
		/// 失去连接，重新连接
		/// </summary>
		private void OnDisconnectNet()
		{
			StartConnect();
		}

		private void CloseTcp()
		{
			if (m_tcpClient != null)
				m_tcpClient.Close();
			m_tcpClient = null;
		}
	}
}
