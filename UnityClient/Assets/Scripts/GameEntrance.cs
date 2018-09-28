using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PublicLib;
using Game.Net;
using Game.Module;
using SyncData;

/***
 * author:lichunlei
 */
namespace Game
{
	/// <summary>
	/// 游戏总入口
	/// </summary>
	public class GameEntrance : MonoBehaviour,ISingleton
	{
		private static GameEntrance m_instance = null;
		public static GameEntrance MInstance { get { return m_instance; } }

		private void Awake()
		{
			Initialize();
		}

		private void Initialize()
		{
			if (m_instance == null)
			{
				m_instance = this;

				new NetMsgDispatch();
				new ProtocolMgr().RegAssembly(typeof(STC_UserInfo).Assembly);

				new GameModuleMgr();
				GameModuleMgr.MInstance.AddModule(new LoginModule());
				GameModuleMgr.MInstance.AddModule(new HeartBeatModule());

				new GameNet();

				GameNet.MInstance.onConnected = OnConnectServerResult;
				GameNet.MInstance.SetIpPort("127.0.0.1", 12000);
				GameNet.MInstance.StartConnect();
			}
		}

		private void OnConnectServerResult(bool isRes)
		{
			if (isRes)
			{
				GameLog.Log("Success To Connect");
			}
			else
				Debug.Log("Fail To Connect");
		}

		// Use this for initialization
		private void Start()
		{
			//测试用例
			m_loginModule = GameModuleMgr.MInstance.FindModule(LoginModule.ModuleName) as LoginModule;
			int beginTime = Random.Range(5, 10);
			int repeatTime = Random.Range(20, 30);
			InvokeRepeating("TestServer", beginTime, repeatTime);
		}

		private void TestServer()
		{
			int res = Random.Range(0, 3);
			if(res == 0)
				m_loginModule.RequestGetUserInfo(2);
			else if(res == 1)
				m_loginModule.RequestUpdateRole(3, "bbb");
			else
				m_loginModule.RequestDeleteRole(4);
		}

		private LoginModule m_loginModule;
		// Update is called once per frame
		public void Update()
		{
		}

		private void FixedUpdate()
		{
			
		}

		private void OnApplicationQuit()
		{
			Clear();
		}
	
		public void Clear()
		{
			GameNet.MInstance.Clear();
			m_instance = null;
		}
	}
}