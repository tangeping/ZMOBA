/*
	Generated by KBEngine!
	Please do not modify this file!
	Please inherit this module, such as: (class Chat : ChatBase)
	tools = kbcmd
*/

namespace KBEngine
{
	using UnityEngine;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	// defined in */scripts/entity_defs/Chat.def
	public abstract class ChatBase : EntityComponent
	{
		public EntityBaseEntityCall_ChatBase baseEntityCall = null;
		public EntityCellEntityCall_ChatBase cellEntityCall = null;

		public FRIEND_LIST friends = new FRIEND_LIST();
		public virtual void onFriendsChanged(FRIEND_LIST oldValue) {}

		public abstract void reply(string arg1, string arg2); 

		public override void createFromStream(MemoryStream stream)
		{
			base.createFromStream(stream);
			baseEntityCall = new EntityBaseEntityCall_ChatBase(entityComponentPropertyID, ownerID);
			cellEntityCall = new EntityCellEntityCall_ChatBase(entityComponentPropertyID, ownerID);
		}

		public override ScriptModule getScriptModule()
		{
			return EntityDef.moduledefs["Chat"];
		}

		public override void onRemoteMethodCall(UInt16 methodUtype, MemoryStream stream)
		{
			ScriptModule sm = EntityDef.moduledefs["Chat"];

			Method method = sm.idmethods[methodUtype];
			switch(method.methodUtype)
			{
				case 28:
					string reply_arg1 = stream.readString();
					string reply_arg2 = stream.readUnicode();
					reply(reply_arg1, reply_arg2);
					break;
				default:
					break;
			};
		}

		public override void onUpdatePropertys(UInt16 propUtype, MemoryStream stream, int maxCount)
		{
			ScriptModule sm = EntityDef.moduledefs["Chat"];
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
					case 16:
						FRIEND_LIST oldval_friends = friends;
						friends = ((DATATYPE_FRIEND_LIST)EntityDef.id2datatypes[23]).createFromStreamEx(stream);

						if(prop.isBase())
						{
							if(owner.inited)
								onFriendsChanged(oldval_friends);
						}
						else
						{
							if(owner.inWorld)
								onFriendsChanged(oldval_friends);
						}

						break;
					default:
						break;
				};
			}
		}

		public override void callPropertysSetMethods()
		{
			ScriptModule sm = EntityDef.moduledefs["Chat"];
			Dictionary<UInt16, Property> pdatas = sm.idpropertys;

			FRIEND_LIST oldval_friends = friends;
			Property prop_friends = pdatas[4];
			if(prop_friends.isBase())
			{
				if(owner.inited && !owner.inWorld)
					onFriendsChanged(oldval_friends);
			}
			else
			{
				if(owner.inWorld)
				{
					if(prop_friends.isOwnerOnly() && !owner.isPlayer())
					{
					}
					else
					{
						onFriendsChanged(oldval_friends);
					}
				}
			}

		}
	}
}