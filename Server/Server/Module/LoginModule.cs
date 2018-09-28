using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PublicLib;
using SyncData;
using Server.DB;
using Server.DB.Data;

namespace Server.Module
{
	public class LoginModule : BaseModule
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
		public LoginModule()
		{
			MModuleName = typeof(LoginModule).Name;
			ModuleName = MModuleName;
		}

		protected override void InitModule()
		{
			RegMsg(CTS_CreateRegRole.MProtoId, OnRegRole);
			RegMsg(CTS_GetUserInfo.MProtoId, OnGetUserInfo);
			RegMsg(CTS_UpdateRole.MProtoId, OnUpdateRole);
			RegMsg(CTS_DeleteRole.MProtoId, OnDeleteRole);
		}

		protected override void UnInitModule()
		{
			UnRegMsg(CTS_CreateRegRole.MProtoId, OnRegRole);
			UnRegMsg(CTS_GetUserInfo.MProtoId, OnGetUserInfo);
			UnRegMsg(CTS_UpdateRole.MProtoId, OnUpdateRole);
			UnRegMsg(CTS_DeleteRole.MProtoId, OnDeleteRole);
		}

		private void OnGetUserInfo(object data)
		{
			ProtocolData pdata = data as ProtocolData;

			CTS_GetUserInfo uinfo = pdata.MData as CTS_GetUserInfo;
			int uid = uinfo.MUserId;

			try
			{
				DBUserInfo info = UserMgr.MInstance.GetUserInfoById(uid);
				STC_UserInfo stc_userinfo = new STC_UserInfo();
				stc_userinfo.MUserName = info.MUserName;
				ServerNet.MInstance.SendMsg(pdata.MIpEndPoint, STC_UserInfo.MProtoId, stc_userinfo);
			}
			catch (Exception exp)
			{
				ServerLog.Log(string.Format("{0}", exp.Message));
			}
		}

		private void OnRegRole(object data)
		{
			ProtocolData pdata = data as ProtocolData;

			CTS_CreateRegRole roleInfo = pdata.MData as CTS_CreateRegRole;
			DBUserInfo info = DBUserInfo.CreateUserInfo(roleInfo);
			bool isOK = false;
			isOK = UserMgr.MInstance.AddUserInfo(info);

			STC_CreateRegRole stc_reg = new STC_CreateRegRole();
			stc_reg.Res = isOK;
			ServerNet.MInstance.SendMsg(pdata.MIpEndPoint, STC_CreateRegRole.MProtoId, stc_reg);
		}

		private void OnUpdateRole(object data)
		{
			ProtocolData pdata = data as ProtocolData;
			CTS_UpdateRole cts_update = pdata.MData as CTS_UpdateRole;

			bool res = false;
			DBUserInfo uinfo = UserMgr.MInstance.GetUserInfoById(cts_update.MUserId);
			if (uinfo != null)
			{
				uinfo.MUserName = cts_update.MUserName;
				res = UserMgr.MInstance.UpdateUserInfo(uinfo);
			}

			STC_UpdateRole stc_update = new STC_UpdateRole();
			stc_update.Res = res;
			ServerNet.MInstance.SendMsg(pdata.MIpEndPoint, STC_UpdateRole.MProtoId, stc_update);
		}

		private void OnDeleteRole(object data)
		{
			ProtocolData pdata = data as ProtocolData;
			CTS_DeleteRole cts = pdata.MData as CTS_DeleteRole;

			bool res = UserMgr.MInstance.DeleteUserInfo(cts.MUserId); ;
			STC_DeleteRole stc_update = new STC_DeleteRole();
			stc_update.Res = res;
			ServerNet.MInstance.SendMsg(pdata.MIpEndPoint, STC_DeleteRole.MProtoId, stc_update);
		}
	}
}
