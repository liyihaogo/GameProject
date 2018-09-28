using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyncData;
using PublicLib;

namespace Server.Module 
{
	public class HeartBeatModule : BaseModule
	{
		public static string ModuleName;
		public override string MModuleName
		{
			get
			{
				return base.MModuleName;
			}

			set
			{
				base.MModuleName = value;
			}
		}

		public HeartBeatModule()
		{
			MModuleName = typeof(HeartBeatModule).Name;
			ModuleName = MModuleName;
		}

		public static int m_timeSpace = 10;	//超时秒数设置
		protected override void InitModule()
		{
			RegMsg(CTS_HeartBeating.MProtoId, OnHeartBeating);
		}

		protected override void UnInitModule()
		{
			UnRegMsg(CTS_HeartBeating.MProtoId, OnHeartBeating);
		}

		private void OnHeartBeating(object data)
		{
			ProtocolData pdata = data as ProtocolData;
			CTS_HeartBeating roleInfo = pdata.MData as CTS_HeartBeating;

			ServerClient client = ServerNet.MInstance.GetSClient(pdata.MIpEndPoint);
			if (client != null)
			{
				int seconds = (System.DateTime.Now - client.MLastBeatingTime).Seconds;
				if (seconds > m_timeSpace)
				{
					Console.WriteLine("连接超时");
				}
				else
				{
					client.MLastBeatingTime = System.DateTime.Now;
					STC_HeartBeating stc_heart = new STC_HeartBeating();
					client.SendMsg(STC_HeartBeating.MProtoId, stc_heart);
				}
			}
		}
	}
}
