using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using ServerLib.DB;
using Server.DB.Data;

/***
 * author:lichunlei
 */
namespace Server.DB
{
	public class DBMgr : BaseDBMgr
	{
		public DBMgr()
		{
		}

		protected override void ConfigMappings(MappingConfiguration mapConfig)
		{
			mapConfig.FluentMappings
				.Add<DBUserInfoMap>();
		}
	}
}
