using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using System.Net.Sockets;
using SyncData;
/***
 * author:lichunlei
 */
namespace PublicLib
{
	public class ProtocolMgr : Singleton<ProtocolMgr>
	{
		private Dictionary<ProtocolEnum, Type> m_protoDic = null;
		public ProtocolMgr()
		{
			m_protoDic = new Dictionary<ProtocolEnum, Type>();
		}

		public Type GetTypeByProtoId(ProtocolEnum penum)
		{
			Type type;
			if (m_protoDic.TryGetValue(penum, out type))
			{
				return type;
			}
			return null;
		}

		public void RegAssembly(Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			foreach (var type in types)
			{
				if (type.IsDefined(typeof(ProtocolFlag), true))
				{
					FieldInfo finfo = type.GetField("MProtoId");
					object protoId = finfo.GetValue(type);

					m_protoDic.Add((ProtocolEnum)protoId, type);
				}
			}
		}
	}
}
