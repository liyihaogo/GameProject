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
/// 登录协议
/// </summary>
namespace SyncData
{
	[ProtocolFlag]
	[ProtoContract]
	public class CTS_GetUserInfo
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.CTS_GetUserInfo;
		[ProtoMember(2)]
		public int MUserId = 1;
	}

	[ProtocolFlag]
	[ProtoContract]
	public class STC_UserInfo
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.STC_UserInfo;
		[ProtoMember(2)]
		public int MUserId = 1;
		[ProtoMember(3)]
		public string MUserName;
		[ProtoMember(4)]
		public List<int> MAges = new List<int>() { 1, 2, 3 };
		[ProtoMember(5)]
		public Dictionary<int, string> MBooks = new Dictionary<int, string>();
	}

	[ProtocolFlag]
	[ProtoContract]
	public class CTS_CreateRegRole
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.CTS_CreateRegRole;
		[ProtoMember(2)]
		public int MUserId = 1;
		[ProtoMember(3)]
		public string MUserName;
		[ProtoMember(4)]
		public string MPassWord;
	}

	[ProtocolFlag]
	[ProtoContract]
	public class STC_CreateRegRole
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.STC_CreateRegRole;
		[ProtoMember(2)]
		public bool Res;
	}

	[ProtocolFlag]
	[ProtoContract]
	public class CTS_UpdateRole
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.CTS_UpdateRole;
		[ProtoMember(2)]
		public int MUserId = 1;
		[ProtoMember(3)]
		public string MUserName;
		[ProtoMember(4)]
		public string MPassWord;
	}

	[ProtocolFlag]
	[ProtoContract]
	public class STC_UpdateRole
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.STC_UpdateRole;
		[ProtoMember(2)]
		public bool Res;
	}

	[ProtocolFlag]
	[ProtoContract]
	public class CTS_DeleteRole
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.CTS_DeleteRole;
		[ProtoMember(2)]
		public int MUserId = 1;
	}

	[ProtocolFlag]
	[ProtoContract]
	public class STC_DeleteRole
	{
		[ProtoMember(1)]
		public static ProtocolEnum MProtoId = ProtocolEnum.STC_DeleteRole;
		[ProtoMember(2)]
		public bool Res;
	}

}
