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

	// defined in */scripts/entity_defs/Avatar.def
	public class EntityBaseEntityCall_AvatarBase : EntityCall
	{
		public EntityBaseEntityCall_FrameSyncReportBase component1 = null;
		public EntityBaseEntityCall_OperationBase component2 = null;
		public EntityBaseEntityCall_ChatBase component3 = null;

		public EntityBaseEntityCall_AvatarBase(Int32 eid, string ename) : base(eid, ename)
		{
			component1 = new EntityBaseEntityCall_FrameSyncReportBase(7, id);
			component2 = new EntityBaseEntityCall_OperationBase(10, id);
			component3 = new EntityBaseEntityCall_ChatBase(14, id);
			type = ENTITYCALL_TYPE.ENTITYCALL_TYPE_BASE;
		}

	}

	public class EntityCellEntityCall_AvatarBase : EntityCall
	{
		public EntityCellEntityCall_FrameSyncReportBase component1 = null;
		public EntityCellEntityCall_OperationBase component2 = null;
		public EntityCellEntityCall_ChatBase component3 = null;

		public EntityCellEntityCall_AvatarBase(Int32 eid, string ename) : base(eid, ename)
		{
			component1 = new EntityCellEntityCall_FrameSyncReportBase(7, id);
			component2 = new EntityCellEntityCall_OperationBase(10, id);
			component3 = new EntityCellEntityCall_ChatBase(14, id);
			type = ENTITYCALL_TYPE.ENTITYCALL_TYPE_CELL;
		}

	}
	}
