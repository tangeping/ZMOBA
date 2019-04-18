/*
	Generated by KBEngine!
	Please do not modify this file!
	Please inherit this module, such as: (class Matchers : MatchersBase)
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Matchers.def
	public abstract class MatchersBase : EntityComponent
	{
		public EntityBaseEntityCall_MatchersBase baseEntityCall = null;
		public EntityCellEntityCall_MatchersBase cellEntityCall = null;



		public override void createFromStream(MemoryStream stream)
		{
			base.createFromStream(stream);
			baseEntityCall = new EntityBaseEntityCall_MatchersBase(entityComponentPropertyID, ownerID);
			cellEntityCall = new EntityCellEntityCall_MatchersBase(entityComponentPropertyID, ownerID);
		}

		public override ScriptModule getScriptModule()
		{
			return EntityDef.moduledefs["Matchers"];
		}

		public override void onRemoteMethodCall(UInt16 methodUtype, MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Matchers"];

			Method method = sm.idmethods[methodUtype];
			switch(method.methodUtype)
			{
				default:
					break;
			};
		}

		public override void onUpdatePropertys(UInt16 propUtype, MemoryStream stream, int maxCount)
		{
			ScriptModule sm = EntityDef.moduledefs["Matchers"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			while(stream.length() > 0 && maxCount-- != 0)
			{
				UInt16 _t_utype = 0;
				UInt16 _t_child_utype = propUtype;

				if(_t_child_utype == 0)
				{
					if(sm.usePropertyDescrAlias)
					{
						_t_utype = stream.readUint8();
						_t_child_utype = stream.readUint8();
					}
					else
					{
						_t_utype = stream.readUint16();
						_t_child_utype = stream.readUint16();
					}
				}

				Property prop = null;

				prop = pdatas[_t_child_utype];

				switch(prop.properUtype)
				{
					default:
						break;
				};
			}
		}

		public override void callPropertysSetMethods()
		{
			ScriptModule sm = EntityDef.moduledefs["Matchers"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

		}
	}
}