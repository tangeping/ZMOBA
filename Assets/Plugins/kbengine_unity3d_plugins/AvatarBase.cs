/*
	Generated by KBEngine!
	Please do not modify this file!
	Please inherit this module, such as: (class Avatar : AvatarBase)
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Avatar.def
	// Please inherit and implement "class Avatar : AvatarBase"
	public abstract class AvatarBase : Entity
	{
		public EntityBaseEntityCall_AvatarBase baseEntityCall = null;
		public EntityCellEntityCall_AvatarBase cellEntityCall = null;

		public FrameSyncReportBase component1 = null;
		public OperationBase component2 = null;
		public string name = "";
		public virtual void onNameChanged(string oldValue) {}
		public SByte teamID = 0;
		public virtual void onTeamIDChanged(SByte oldValue) {}


		public AvatarBase()
		{
			foreach (System.Reflection.Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type entityComponentScript = ass.GetType("KBEngine.FrameSyncReport");
				if(entityComponentScript != null)
				{
					component1 = (FrameSyncReportBase)Activator.CreateInstance(entityComponentScript);
					component1.owner = this;
					component1.entityComponentPropertyID = 7;
				}
			}

			if(component1 == null)
				throw new Exception("Please inherit and implement, such as: \"class FrameSyncReport : FrameSyncReportBase\"");

			foreach (System.Reflection.Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type entityComponentScript = ass.GetType("KBEngine.Operation");
				if(entityComponentScript != null)
				{
					component2 = (OperationBase)Activator.CreateInstance(entityComponentScript);
					component2.owner = this;
					component2.entityComponentPropertyID = 10;
				}
			}

			if(component2 == null)
				throw new Exception("Please inherit and implement, such as: \"class Operation : OperationBase\"");

		}

		public override void onGetBase()
		{
			baseEntityCall = new EntityBaseEntityCall_AvatarBase(id, className);
		}

		public override void onGetCell()
		{
			cellEntityCall = new EntityCellEntityCall_AvatarBase(id, className);
		}

		public override void onLoseCell()
		{
			cellEntityCall = null;
		}

		public override EntityCall getBaseEntityCall()
		{
			return baseEntityCall;
		}

		public override EntityCall getCellEntityCall()
		{
			return cellEntityCall;
		}

		public override void attachComponents()
		{
			component1.onAttached(this);
			component2.onAttached(this);
		}

		public override void detachComponents()
		{
			component1.onDetached(this);
			component2.onDetached(this);
		}

		public override void onRemoteMethodCall(MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Avatar"];

			UInt16 methodUtype = 0;
			UInt16 componentPropertyUType = 0;

			if(sm.useMethodDescrAlias)
			{
				componentPropertyUType = stream.readUint8();
				methodUtype = stream.readUint8();
			}
			else
			{
				componentPropertyUType = stream.readUint16();
				methodUtype = stream.readUint16();
			}

			Method method = null;

			if(componentPropertyUType == 0)
			{
				method = sm.idmethods[methodUtype];
			}
			else
			{
				Property pComponentPropertyDescription = sm.idpropertys[componentPropertyUType];
				switch(pComponentPropertyDescription.properUtype)
				{
					case 7:
						component1.onRemoteMethodCall(methodUtype, stream);
						break;
					case 10:
						component2.onRemoteMethodCall(methodUtype, stream);
						break;
					default:
						break;
				}

				return;
			}

			switch(method.methodUtype)
			{
				default:
					break;
			};
		}

		public override void onUpdatePropertys(MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Avatar"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			while(stream.length() > 0)
			{
				UInt16 _t_utype = 0;
				UInt16 _t_child_utype = 0;

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

				if(_t_utype == 0)
				{
					prop = pdatas[_t_child_utype];
				}
				else
				{
					Property pComponentPropertyDescription = pdatas[_t_utype];
					switch(pComponentPropertyDescription.properUtype)
					{
						case 7:
							component1.onUpdatePropertys(_t_child_utype, stream, -1);
							break;
						case 10:
							component2.onUpdatePropertys(_t_child_utype, stream, -1);
							break;
						default:
							break;
					}

					return;
				}

				switch(prop.properUtype)
				{
					case 7:
						component1.createFromStream(stream);
						break;
					case 10:
						component2.createFromStream(stream);
						break;
					case 40001:
						Vector3 oldval_direction = direction;
						direction = stream.readVector3();

						if(prop.isBase())
						{
							if(inited)
								onDirectionChanged(oldval_direction);
						}
						else
						{
							if(inWorld)
								onDirectionChanged(oldval_direction);
						}

						break;
					case 4:
						string oldval_name = name;
						name = stream.readUnicode();

						if(prop.isBase())
						{
							if(inited)
								onNameChanged(oldval_name);
						}
						else
						{
							if(inWorld)
								onNameChanged(oldval_name);
						}

						break;
					case 40000:
						Vector3 oldval_position = position;
						position = stream.readVector3();

						if(prop.isBase())
						{
							if(inited)
								onPositionChanged(oldval_position);
						}
						else
						{
							if(inWorld)
								onPositionChanged(oldval_position);
						}

						break;
					case 40002:
						stream.readUint32();
						break;
					case 6:
						SByte oldval_teamID = teamID;
						teamID = stream.readInt8();

						if(prop.isBase())
						{
							if(inited)
								onTeamIDChanged(oldval_teamID);
						}
						else
						{
							if(inWorld)
								onTeamIDChanged(oldval_teamID);
						}

						break;
					default:
						break;
				};
			}
		}

		public override void callPropertysSetMethods()
		{
			ScriptModule sm = EntityDef.moduledefs["Avatar"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			component1.callPropertysSetMethods();

			component2.callPropertysSetMethods();

			Vector3 oldval_direction = direction;
			Property prop_direction = pdatas[2];
			if(prop_direction.isBase())
			{
				if(inited && !inWorld)
					onDirectionChanged(oldval_direction);
			}
			else
			{
				if(inWorld)
				{
					if(prop_direction.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onDirectionChanged(oldval_direction);
					}
				}
			}

			string oldval_name = name;
			Property prop_name = pdatas[6];
			if(prop_name.isBase())
			{
				if(inited && !inWorld)
					onNameChanged(oldval_name);
			}
			else
			{
				if(inWorld)
				{
					if(prop_name.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onNameChanged(oldval_name);
					}
				}
			}

			Vector3 oldval_position = position;
			Property prop_position = pdatas[1];
			if(prop_position.isBase())
			{
				if(inited && !inWorld)
					onPositionChanged(oldval_position);
			}
			else
			{
				if(inWorld)
				{
					if(prop_position.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onPositionChanged(oldval_position);
					}
				}
			}

			SByte oldval_teamID = teamID;
			Property prop_teamID = pdatas[7];
			if(prop_teamID.isBase())
			{
				if(inited && !inWorld)
					onTeamIDChanged(oldval_teamID);
			}
			else
			{
				if(inWorld)
				{
					if(prop_teamID.isOwnerOnly() && !isPlayer())
					{
					}
					else
					{
						onTeamIDChanged(oldval_teamID);
					}
				}
			}

		}
	}
}