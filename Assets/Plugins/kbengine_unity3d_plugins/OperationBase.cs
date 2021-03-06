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

		public Int32 heroID = 0;
		public virtual void onHeroIDChanged(Int32 oldValue) {}

		public abstract void broadGameStart(); 
		public abstract void readyResult(Byte arg1); 
		public abstract void reqHeroListResult(HERO_BAG arg1); 
		public abstract void reqSelectHeroResult(Byte arg1); 
		public abstract void rspHeroInfo(D_HERO_INFOS_LIST arg1); 
		public abstract void rspPropsInfo(D_PROPS_INFOS_LIST arg1); 
		public abstract void rspRoadInfo(D_ROAD_INFOS_LIST arg1); 
		public abstract void rspShopInfo(D_SHOP_INFOS_LIST arg1); 
		public abstract void rspSkillInfo(D_SKILL_INFOS_LIST arg1); 
		public abstract void rspTeamInfo(D_TEAM_INFOS_LIST arg1); 

		public override void createFromStream(MemoryStream stream)
		{
			base.createFromStream(stream);
			baseEntityCall = new EntityBaseEntityCall_OperationBase(entityComponentPropertyID, ownerID);
			cellEntityCall = new EntityCellEntityCall_OperationBase(entityComponentPropertyID, ownerID);
		}

		public override ScriptModule getScriptModule()
		{
			return EntityDef.moduledefs["Operation"];
		}

		public override void onRemoteMethodCall(UInt16 methodUtype, MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Operation"];

			Method method = sm.idmethods[methodUtype];
			switch(method.methodUtype)
			{
				case 19:
					broadGameStart();
					break;
				case 20:
					Byte readyResult_arg1 = stream.readUint8();
					readyResult(readyResult_arg1);
					break;
				case 17:
					HERO_BAG reqHeroListResult_arg1 = ((DATATYPE_HERO_BAG)method.args[0]).createFromStreamEx(stream);
					reqHeroListResult(reqHeroListResult_arg1);
					break;
				case 18:
					Byte reqSelectHeroResult_arg1 = stream.readUint8();
					reqSelectHeroResult(reqSelectHeroResult_arg1);
					break;
				case 22:
					D_HERO_INFOS_LIST rspHeroInfo_arg1 = ((DATATYPE_D_HERO_INFOS_LIST)method.args[0]).createFromStreamEx(stream);
					rspHeroInfo(rspHeroInfo_arg1);
					break;
				case 23:
					D_PROPS_INFOS_LIST rspPropsInfo_arg1 = ((DATATYPE_D_PROPS_INFOS_LIST)method.args[0]).createFromStreamEx(stream);
					rspPropsInfo(rspPropsInfo_arg1);
					break;
				case 21:
					D_ROAD_INFOS_LIST rspRoadInfo_arg1 = ((DATATYPE_D_ROAD_INFOS_LIST)method.args[0]).createFromStreamEx(stream);
					rspRoadInfo(rspRoadInfo_arg1);
					break;
				case 24:
					D_SHOP_INFOS_LIST rspShopInfo_arg1 = ((DATATYPE_D_SHOP_INFOS_LIST)method.args[0]).createFromStreamEx(stream);
					rspShopInfo(rspShopInfo_arg1);
					break;
				case 25:
					D_SKILL_INFOS_LIST rspSkillInfo_arg1 = ((DATATYPE_D_SKILL_INFOS_LIST)method.args[0]).createFromStreamEx(stream);
					rspSkillInfo(rspSkillInfo_arg1);
					break;
				case 26:
					D_TEAM_INFOS_LIST rspTeamInfo_arg1 = ((DATATYPE_D_TEAM_INFOS_LIST)method.args[0]).createFromStreamEx(stream);
					rspTeamInfo(rspTeamInfo_arg1);
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
					case 13:
						Int32 oldval_heroID = heroID;
						heroID = stream.readInt32();

						if(prop.isBase())
						{
							if(owner.inited)
								onHeroIDChanged(oldval_heroID);
						}
						else
						{
							if(owner.inWorld)
								onHeroIDChanged(oldval_heroID);
						}

						break;
					default:
						break;
				};
			}
		}

		public override void callPropertysSetMethods()
		{
			ScriptModule sm = EntityDef.moduledefs["Operation"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			Int32 oldval_heroID = heroID;
			Property prop_heroID = pdatas[4];
			if(prop_heroID.isBase())
			{
				if(owner.inited && !owner.inWorld)
					onHeroIDChanged(oldval_heroID);
			}
			else
			{
				if(owner.inWorld)
				{
					if(prop_heroID.isOwnerOnly() && !owner.isPlayer())
					{
					}
					else
					{
						onHeroIDChanged(oldval_heroID);
					}
				}
			}

		}
	}
}