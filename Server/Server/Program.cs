using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using PublicLib;
using Server.DB;
using Server.Module;
using SyncData;
using Server.DB.Data;

/***
 * author:lichunlei
 */
namespace Server
{
	class Program
	{
		static void Main(string[] args)
		{
			new NetMsgDispatch();
			new ProtocolMgr().RegAssembly(typeof(STC_UserInfo).Assembly);

			new DBMgr();
			DBMgr.MInstance.onInitDBSuccess = delegate ()
			{
				ServerLog.Log("DataBase Initialize OK");
				new GameModuleMgr();
				GameModuleMgr.MInstance.AddModule(new LoginModule());
				GameModuleMgr.MInstance.AddModule(new HeartBeatModule());

				new UserMgr();
				new ServerNet();
			};
			DBMgr.MInstance.InitializeDB("127.0.0.1", 3306, "database_test", "bc", "123456");

			Console.ReadKey();
		}
	}
}
