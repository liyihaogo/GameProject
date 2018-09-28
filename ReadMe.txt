1.PublicLib 工程为服务器和客户端公用代码
2.Server 工程为服务器代码
3.UnityClient 工程为客户端代码

实现了心跳机制、协议收发、粘包问题、数据库存取。

此为数据库配置信息：需要建立数据库如下：
//DBMgr.MInstance.InitializeDB("数据库ip", 数据库端口, "数据库名称", "数据库用户名", "数据库密码");
DBMgr.MInstance.InitializeDB("127.0.0.1", 3306, "database_test", "bc", "123456");

客户端入口：
GameEntrance.cs