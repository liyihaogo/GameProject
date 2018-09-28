using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

using System.IO;

using PublicLib;
using SyncData;
using ProtoBuf;

/***
 * author:lichunlei
 */
namespace Server
{
	/// <summary>
	/// 服务器接受的TcpClient
	/// </summary>
	public class ServerClient : IDisposable
	{
		private IPEndPoint m_ipEndPoint;
		private TcpNetProxy m_netProxy;

		public string MIpEndPointString { get { return m_ipEndPoint.ToString(); } }
		public DateTime MLastBeatingTime { get; set; }
		public ServerClient(TcpClient tclient)
		{
			m_ipEndPoint = tclient.Client.RemoteEndPoint as IPEndPoint;

			m_netProxy = new TcpNetProxy(tclient);
			m_netProxy.onDisconnect = OnDisconnect;
			Console.WriteLine("new Client");
		}

		public static ServerClient CreateClient(TcpClient tclient)
		{
			ServerClient client = new ServerClient(tclient);
			return client;
		}

		private void OnDisconnect()
		{
			ServerNet.MInstance.RemoveSClient(m_ipEndPoint.ToString());
			Dispose();
		}

		/// <summary>
		/// 开始读取数据
		/// </summary>
		public void StartRead()
		{
			MLastBeatingTime = DateTime.Now;
			if(m_netProxy != null && m_netProxy.IsConnected)
				m_netProxy.StartAsyncRead();
		}

		public void SendMsg(ProtocolEnum protoId, object obj)
		{
			if(m_netProxy != null && m_netProxy.IsConnected)
				m_netProxy.SendMsg(protoId, obj);
		}

		public void Dispose()
		{
			m_ipEndPoint = null;
			m_netProxy = null;
		}
	}
}
