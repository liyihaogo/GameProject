using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

/***
 * author:lichunlei
 */
namespace PublicLib
{
	public class ServerLog
	{
		[Conditional("ENABLELOG")]
		public static void Log(string info)
		{
			Console.WriteLine("{0}", info);
		}
	}
}
