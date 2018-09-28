using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;

/***
 * author:lichunlei
 */
namespace PublicLib
{
	public class GameLog
	{
		[Conditional("ENABLELOG")]
		public static void Log(string info)
		{
			UnityEngine.Debug.Log("info:" + info);
		}
	}
}
