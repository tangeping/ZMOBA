/*
	Generated by KBEngine!
	Please do not modify this file!
	
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Operation.def
	public class EntityBaseEntityCall_OperationBase : EntityCall
	{
		public UInt16 entityComponentPropertyID = 0;

		public EntityBaseEntityCall_OperationBase(UInt16 ecpID, Int32 eid) : base(eid, "Operation")
		{
			entityComponentPropertyID = ecpID;
			type = ENTITYCALL_TYPE.ENTITYCALL_TYPE_BASE;
		}

	}

	public class EntityCellEntityCall_OperationBase : EntityCall
	{
		public UInt16 entityComponentPropertyID = 0;

		public EntityCellEntityCall_OperationBase(UInt16 ecpID, Int32 eid) : base(eid, "Operation")
		{
			entityComponentPropertyID = ecpID;
			className = "Operation";
			type = ENTITYCALL_TYPE.ENTITYCALL_TYPE_CELL;
		}

		public void reqGamePause()
		{
			Bundle pBundle = newCall("reqGamePause", entityComponentPropertyID);
			if(pBundle == null)
				return;

			sendCall(null);
		}

		public void reqGameRunning()
		{
			Bundle pBundle = newCall("reqGameRunning", entityComponentPropertyID);
			if(pBundle == null)
				return;

			sendCall(null);
		}

		public void reqHeroConf()
		{
			Bundle pBundle = newCall("reqHeroConf", entityComponentPropertyID);
			if(pBundle == null)
				return;

			sendCall(null);
		}

		public void reqHeroList()
		{
			Bundle pBundle = newCall("reqHeroList", entityComponentPropertyID);
			if(pBundle == null)
				return;

			sendCall(null);
		}

		public void reqPropsConf()
		{
			Bundle pBundle = newCall("reqPropsConf", entityComponentPropertyID);
			if(pBundle == null)
				return;

			sendCall(null);
		}

		public void reqReady(Byte arg1)
		{
			Bundle pBundle = newCall("reqReady", entityComponentPropertyID);
			if(pBundle == null)
				return;

			bundle.writeUint8(arg1);
			sendCall(null);
		}

		public void reqRoadConf()
		{
			Bundle pBundle = newCall("reqRoadConf", entityComponentPropertyID);
			if(pBundle == null)
				return;

			sendCall(null);
		}

		public void reqSelectHero(Int32 arg1)
		{
			Bundle pBundle = newCall("reqSelectHero", entityComponentPropertyID);
			if(pBundle == null)
				return;

			bundle.writeInt32(arg1);
			sendCall(null);
		}

		public void reqShopConf()
		{
			Bundle pBundle = newCall("reqShopConf", entityComponentPropertyID);
			if(pBundle == null)
				return;

			sendCall(null);
		}

		public void reqSkillConf()
		{
			Bundle pBundle = newCall("reqSkillConf", entityComponentPropertyID);
			if(pBundle == null)
				return;

			sendCall(null);
		}

		public void reqTeamConf()
		{
			Bundle pBundle = newCall("reqTeamConf", entityComponentPropertyID);
			if(pBundle == null)
				return;

			sendCall(null);
		}

	}
	}
