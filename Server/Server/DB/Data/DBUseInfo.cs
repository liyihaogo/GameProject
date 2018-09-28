using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Mapping;
using SyncData;

/***
 * author:lichunlei
 */

namespace Server.DB.Data
{
	public class DBUserInfo
	{
		public virtual int MUserId { get; set; }
		public virtual string MUserName { get; set; }
		public virtual string MPassWord { get; set; }

		public static DBUserInfo CreateUserInfo(CTS_CreateRegRole role)
		{
			DBUserInfo info = new DBUserInfo();
			info.MUserId = role.MUserId;
			info.MUserName = role.MUserName;
			info.MPassWord = role.MPassWord;
			return info;
		}
	}

	public class DBUserInfoMap : ClassMap<DBUserInfo>
	{
		public static string TableName = "userinfo";
		public DBUserInfoMap()
		{
			Table(TableName);
			//Id(x => x.MUserId).GeneratedBy.Assigned(); 通过用户指定id来直接赋值
			Id(x => x.MUserId);
			Map(x => x.MUserName);
			Map(x => x.MPassWord);
		}
	}
}
