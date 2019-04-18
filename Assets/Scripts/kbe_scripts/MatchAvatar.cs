
namespace KBEngine
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class MatchAvatar : MatchAvatarBase
    {

        public MatchAvatar() : base()
        {

        }

        public override void onAttached(Entity ownerEntity)
        {
            if (ownerEntity.isPlayer())
            {
                registerEvent();
            }
        }

        public void registerEvent()
        {
            KBEngine.Event.registerIn("reqJoinMatch", this, "reqJoinMatch");
            KBEngine.Event.registerIn("reqExitMatch", this, "reqExitMatch");
        }
        public void reqJoinMatch(D_MATCH_REQUEST request)
        {
            baseEntityCall.reqJoinMatch(request);
        }
        public void reqExitMatch()
        {
            baseEntityCall.reqExitMatch();
        }

        public override void onDetached(Entity ownerEntity)
        {
            base.onDetached(ownerEntity);
            KBEngine.Event.deregisterOut(this);
        }
    }
}
