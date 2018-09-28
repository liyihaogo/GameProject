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
	public interface ISingleton
	{
		void Clear();
	}

	/// <summary>
	/// 泛型单例
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Singleton<T> : ISingleton where T : Singleton<T>, new()
	{

		private static T m_instance = null;
		private static readonly object m_lockObj = new object();

		public static T MInstance
		{
			get
			{
				return m_instance;
			}
		}

		public Singleton()
		{
			m_instance = this as T;
		}

		public virtual void Clear()
		{
			m_instance = null;
		}
	}
}
