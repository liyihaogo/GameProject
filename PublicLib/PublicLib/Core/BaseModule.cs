using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SyncData;
/***
 * author:lichunlei
 */
namespace PublicLib
{
	public abstract class BaseModule
	{
		public virtual string MModuleName {get;set;}
		public BaseModule() { InitModule(); }
		protected abstract void InitModule();
		protected abstract void UnInitModule();

		protected virtual void RegMsg(ProtocolEnum protoId, OnMsgHandle handle)
		{
			NetMsgDispatch.MInstance.RegMsg((ProtocolEnum)protoId, handle);
		}

		protected virtual void UnRegMsg(ProtocolEnum protoId, OnMsgHandle handle)
		{
			NetMsgDispatch.MInstance.UnRegMsg((ProtocolEnum)protoId, handle);
		}
	}
}
