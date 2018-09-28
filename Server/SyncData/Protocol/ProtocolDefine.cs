using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/***
 * author:lichunlei
 */
namespace SyncData
{
	public enum ProtocolEnum
	{
		CTS_HeartBeating = 1001,
		STC_HeartBeating = 1002,

		CTS_GetUserInfo = 1003,
		STC_UserInfo = 1004,

		CTS_CreateRegRole = 1005,
		STC_CreateRegRole = 1006,

		CTS_UpdateRole = 1007,
		STC_UpdateRole = 1008,

		CTS_DeleteRole = 1009,
		STC_DeleteRole = 1010,
	}

	public class ProtocolData
	{
		public ProtocolEnum MProtocolId { get; set; }
		public string MIpEndPoint { get; set; }
		public object MData { get; set; }
		public ProtocolData() { }
		/// <summary>
		/// 创建协议
		/// </summary>
		/// <param name="pId">协议id</param>
		/// <param name="pData">协议体</param>
		/// <param name="pPoint">tcp的ipEndPoint（服务器用)</param>
		/// <returns></returns>
		public static ProtocolData MakeProtocol(ProtocolEnum pId, object pData, string pPoint = "")
		{
			ProtocolData protocolData = new ProtocolData();
			protocolData.MProtocolId = pId;
			protocolData.MData = pData;
			protocolData.MIpEndPoint = pPoint;
			return protocolData;
		}
	}
}
