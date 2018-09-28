using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/***
 * author:lichunlei
 */
namespace PublicLib
{
	public class GameModuleMgr : Singleton<GameModuleMgr>
	{
		private Dictionary<string, BaseModule> m_moduleDic;
		public GameModuleMgr()
		{
			m_moduleDic = new Dictionary<string, BaseModule>();
		}

		public void AddModule(BaseModule module)
		{
			if (m_moduleDic.ContainsKey(module.MModuleName)) return;
			m_moduleDic.Add(module.MModuleName, module);

		}

		public void RemoveModule(string moduleName)
		{
			if (m_moduleDic.ContainsKey(moduleName))
				m_moduleDic.Remove(moduleName);
		}

		public BaseModule FindModule(string module)
		{
			BaseModule moduleEntity;
			if (!m_moduleDic.TryGetValue(module, out moduleEntity)) return null;
			return moduleEntity;
		}
	}
}
