/*
	Generated by KBEngine!
	Please do not modify this file!
	Please inherit this module, such as: (class Operation : OperationBase)
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Operation.def
	public abstract class OperationBase : EntityComponent
	{
		public EntityBaseEntityCall_OperationBase baseEntityCall = null;
		public EntityCellEntityCall_OperationBase cellEntityCall = null;


		public abstract void readyResult(Byte arg1); 

		public override void createFromStream(MemoryStream stream)
		{
			base.createFromStream(stream);
			baseEntityCall = new EntityBaseEntityCall_OperationBase(entityComponentPropertyID, ownerID);
			cellEntityCall = new EntityCellEntityCall_OperationBase(entityComponentPropertyID, ownerID);
		}

		public override void onRemoteMethodCall(UInt16 methodUtype, MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Operation"];

			Method method = sm.idmethods[methodUtype];
			switch(method.methodUtype)
			{
				case 6:
					Byte readyResult_arg1 = stream.readUint8();
					readyResult(readyResult_arg1);
					break;
				default:
					break;
			};
		}

		public override void onUpdatePropertys(UInt16 propUtype, MemoryStream stream, int maxCount)
		{
			ScriptModule sm = EntityDef.moduledefs["Operation"];
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
			ScriptModule sm = EntityDef.moduledefs["Operation"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

		}
	}
}