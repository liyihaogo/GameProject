using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PublicLib;
using SyncData;
using Game.Net;

/***
 * author:lichunlei
 */
namespace Game.Module
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
			RegMsg(ProtocolEnum.STC_CreateRegRole, OnRegRole);
			RegMsg(ProtocolEnum.STC_UserInfo, OnGetUserInfo);
			RegMsg(ProtocolEnum.STC_UpdateRole, OnUpdateRole);
			RegMsg(ProtocolEnum.STC_DeleteRole, OnDeleteRole);
		}

		protected override void UnInitModule()
		{
			UnRegMsg(ProtocolEnum.STC_CreateRegRole, OnRegRole);
			UnRegMsg(ProtocolEnum.STC_UserInfo, OnGetUserInfo);
			UnRegMsg(ProtocolEnum.STC_UpdateRole, OnUpdateRole);
			UnRegMsg(ProtocolEnum.STC_DeleteRole, OnDeleteRole);
		}

		private void OnRegRole(object data)
		{
			ProtocolData pdata = data as ProtocolData;
			STC_CreateRegRole role = pdata.MData as STC_CreateRegRole;
			GameLog.Log(string.Format("注册:{0}", role.Res ? "ok" : "fail"));
		}
		
		private void OnGetUserInfo(object data)
		{
			ProtocolData pData = data as ProtocolData;

			STC_UserInfo userInfo = pData.MData as STC_UserInfo;
			GameLog.Log(string.Format("userId:{0} username:{1}", userInfo.MUserId, userInfo.MUserName));
		}

		private void OnUpdateRole(object data)
		{
			ProtocolData pdata = data as ProtocolData;
			STC_UpdateRole stc_update = pdata.MData as STC_UpdateRole;
			GameLog.Log(string.Format("Update Role:{0}", stc_update.Res ? "ok" : "fail"));
		}

		private void OnDeleteRole(object data)
		{
			ProtocolData pdata = data as ProtocolData;
			STC_DeleteRole stc_delete = pdata.MData as STC_DeleteRole;
			GameLog.Log(string.Format("delete Role:{0}", stc_delete.Res ? "ok" : "fail"));
		}

		public void RequestCreateRole(string name, string password)
		{
			CTS_CreateRegRole regRole = new CTS_CreateRegRole();
			regRole.MUserName = name;
			regRole.MPassWord = password;

			GameNet.MInstance.SendMsg(CTS_CreateRegRole.MProtoId, regRole);
		}

		public void RequestGetUserInfo(int id)
		{
			CTS_GetUserInfo getInfo = new CTS_GetUserInfo();
			getInfo.MUserId = id;

			GameNet.MInstance.SendMsg(CTS_GetUserInfo.MProtoId, getInfo);
		}

		public void RequestUpdateRole(int id, string name)
		{
			CTS_UpdateRole cts_update = new CTS_UpdateRole();
			cts_update.MUserId = id;
			cts_update.MUserName = name;
			GameNet.MInstance.SendMsg(CTS_UpdateRole.MProtoId, cts_update);
		}

		public void RequestDeleteRole(int id)
		{
			CTS_DeleteRole cts_update = new CTS_DeleteRole();
			cts_update.MUserId = id;
			GameNet.MInstance.SendMsg(CTS_DeleteRole.MProtoId, cts_update);
		}
	}
}
