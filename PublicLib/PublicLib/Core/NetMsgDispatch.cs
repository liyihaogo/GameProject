using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncData;

/***
 * author:lichunlei
 */
namespace PublicLib
{
	public delegate void OnMsgHandle(object data);
	public class NetMsgDispatch : Singleton<NetMsgDispatch>
	{
		private Dictionary<ProtocolEnum, List<OnMsgHandle>> m_netMsgLst = null;
		public NetMsgDispatch()
		{
			m_netMsgLst = new Dictionary<ProtocolEnum, List<OnMsgHandle>>();
		}

		public void RegMsg(ProtocolEnum protoId, OnMsgHandle msgHandle)
		{
			List<OnMsgHandle> msgLst;
			if (!m_netMsgLst.TryGetValue(protoId, out msgLst))
			{
				msgLst = new List<OnMsgHandle>();
				m_netMsgLst.Add(protoId, msgLst);
			}
			msgLst.Add(msgHandle);
		}

		public void UnRegMsg(ProtocolEnum protoId, OnMsgHandle msgHandle)
		{
			List<OnMsgHandle> msgLst;
			if (!m_netMsgLst.TryGetValue(protoId, out msgLst))
			{
				return;
			}

			msgLst.Remove(msgHandle);
			if (msgLst.Count <= 0)
				m_netMsgLst.Remove(protoId);
		}

		public void DispathMsg(ProtocolEnum protoId, object mdata)
		{
			List<OnMsgHandle> msgLst;
			if (!m_netMsgLst.TryGetValue(protoId, out msgLst))
			{
				Console.WriteLine("No Such A ProtoId{0} Registered", protoId);
				return;
			}
			foreach (var msg in msgLst)
			{
				msg(mdata);
			}
		}

		public override void Clear()
		{
			base.Clear();

			m_netMsgLst.Clear();
			m_netMsgLst = null;
		}
	}
}
