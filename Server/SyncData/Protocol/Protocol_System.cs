using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
/***
 * author:lichunlei
 */
/// <summary>
/// 系统级别协议
/// </summary>
namespace SyncData
{
	[ProtocolFlag]
	[ProtoContract]
	public class CTS_HeartBeating
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.CTS_HeartBeating;
		[ProtoMember(2)]
		public int MBeatNumber = 0;
	}

	[ProtocolFlag]
	[ProtoContract]
	public class STC_HeartBeating
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.STC_HeartBeating;
		[ProtoMember(2)]
		public int MBeatNumber = 0;
	}
}
