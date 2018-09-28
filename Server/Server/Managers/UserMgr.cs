using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PublicLib;
using Server.DB;
using Server.DB.Data;
/***
 * author:lichunlei
 */
namespace Server
{
	/// <summary>
	/// 玩家数据操作
	/// </summary>
	public class UserMgr : Singleton<UserMgr>
	{
		private IQueryable<DBUserInfo> m_userQuery;
		public UserMgr() { }

		public void LoadAllUser()
		{
			m_userQuery = DBMgr.MInstance.DBSession().Query<DBUserInfo>();
		}

		/// <summary>
		/// 添加玩家
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public bool AddUserInfo(DBUserInfo info)
		{
			try
			{
				DBMgr.MInstance.DBSession().Insert(info);
				return true;
			}
			catch (Exception exp)
			{
				ServerLog.Log(string.Format("Insert Wrong:{0}", exp.Message));
				return false;
			}
		}

		/// <summary>
		/// 根据id查找UserInfo
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public DBUserInfo GetUserInfoById(int id)
		{
			DBUserInfo info = DBMgr.MInstance.DBSession().Get<DBUserInfo>(id);
			return info;
		}

		/// <summary>
		///更新某玩家
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public bool UpdateUserInfo(DBUserInfo info)
		{
			try
			{
				DBMgr.MInstance.DBSession().Update(info);
				return true;
			}
			catch (Exception exp)
			{
				ServerLog.Log(string.Format("Update Wrong:{0}", exp.Message));
				return false;
			}
		}

		/// <summary>
		/// 删除某项
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool DeleteUserInfo(int id)
		{
			try
			{
				DBMgr.MInstance.DBSession().Delete(new DBUserInfo() { MUserId = id });
				return true;
			}
			catch (Exception exp)
			{
				ServerLog.Log(string.Format("Delete Wrong:{0}", exp.Message));
				return false; 
			}
		}
	}
}
