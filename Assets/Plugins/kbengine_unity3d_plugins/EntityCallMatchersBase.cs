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

	// defined in */scripts/entity_defs/Matchers.def
	public class EntityBaseEntityCall_MatchersBase : EntityCall
	{
		public UInt16 entityComponentPropertyID = 0;

		public EntityBaseEntityCall_MatchersBase(UInt16 ecpID, Int32 eid) : base(eid, "Matchers")
		{
			entityComponentPropertyID = ecpID;
			type = ENTITYCALL_TYPE.ENTITYCALL_TYPE_BASE;
		}

	}

	public class EntityCellEntityCall_MatchersBase : EntityCall
	{
		public UInt16 entityComponentPropertyID = 0;

		public EntityCellEntityCall_MatchersBase(UInt16 ecpID, Int32 eid) : base(eid, "Matchers")
		{
			entityComponentPropertyID = ecpID;
			className = "Matchers";
			type = ENTITYCALL_TYPE.ENTITYCALL_TYPE_CELL;
		}

	}
	}
