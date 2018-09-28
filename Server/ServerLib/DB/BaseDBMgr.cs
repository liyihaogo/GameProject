using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Cfg;
using PublicLib;

/***
 * author:lichunlei
 */
namespace ServerLib.DB
{
	public class BaseDBMgr : Singleton<BaseDBMgr>
	{
		public delegate void OnInitDBSuccess();
		public OnInitDBSuccess onInitDBSuccess;

		private ISessionFactory m_sessionFactory = null;
		public BaseDBMgr()
		{

		}

		public virtual void InitializeDB(string dbIp, int dbPort, string db, string dbUserName, string dbPassWord)
		{
			string dbStr = string.Format("Server={0}; Port={1}; Database={2}; User={3}; Password={4}; Charset=utf8; Pooling=true; SslMode=None", dbIp, dbPort, db, dbUserName, dbPassWord);
			try
			{
				var cfg = Fluently.Configure()
						 .Database(MySQLConfiguration.Standard.ConnectionString(dbStr))
						 .Mappings(ConfigMappings)
						 .BuildConfiguration();

				cfg.DataBaseIntegration(dbConfig =>
				{
					dbConfig.LogFormattedSql = true;
					dbConfig.LogSqlInConsole = false;
					dbConfig.OrderInserts = true;
					dbConfig.AutoCommentSql = true;
					dbConfig.PrepareCommands = true;
					dbConfig.SchemaAction = SchemaAutoAction.Update;
					dbConfig.Dialect<NHibernate.Dialect.MySQL55InnoDBDialect>();
				});

				m_sessionFactory = cfg.BuildSessionFactory();
			}
			catch (Exception exp)
			{
				Console.WriteLine("Init DB Failed:{0}", exp.Message);
				return;
			}
			if (onInitDBSuccess != null)
				onInitDBSuccess();
		}

		public IStatelessSession DBSession()
		{
			return m_sessionFactory.OpenStatelessSession();
		}

		protected virtual void ConfigMappings(MappingConfiguration mapConfig)
		{
		}
	}
}
