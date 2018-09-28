using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PublicLib;
using SyncData;
using Game.Net;

/***
 * author:lichunlei
 */
namespace Game.Module
{
	/// <summary>
	/// 心跳模块
	/// </summary>
	public class HeartBeatModule : BaseModule
	{
		public int MHeartBeatingTimeOut = 10;	//心跳超时
		public int MHeartBeatingSpace = 5000;	//心跳间隔

		private DateTime m_curHeartTime;
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

		public HeartBeatModule()
		{
			MModuleName = typeof(HeartBeatModule).Name;
			ModuleName = MModuleName;
		}

		protected override void InitModule()
		{
			RegMsg(STC_HeartBeating.MProtoId, OnHeartBeat);
		}

		protected override void UnInitModule()
		{
			UnRegMsg(STC_HeartBeating.MProtoId, OnHeartBeat);
		}

		private void OnHeartBeat(object data)
		{
			if((DateTime.Now - m_curHeartTime).Seconds > MHeartBeatingTimeOut)
			{
				GameLog.Log("HeartBeat Time Out");
				GameNet.MInstance.StartConnect();
			}
			else
			{
				m_curHeartTime = DateTime.Now;
				//GameLog.Log("Get Server HeartBeating");
			}
		}

		public void RequestHeartBeating()
		{
			CTS_HeartBeating cts_heart = new CTS_HeartBeating();
			GameNet.MInstance.SendMsg(CTS_HeartBeating.MProtoId, cts_heart);
			m_curHeartTime = DateTime.Now;
		}
	}
}