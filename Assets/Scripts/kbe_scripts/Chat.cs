namespace KBEngine
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class Chat : ChatBase
    {

        public Chat() : base()
        {

        }

        public override void onAttached(Entity ownerEntity)
        {
            if (ownerEntity.isPlayer())
            {
                KBEngine.Event.registerIn("say", this, "say");
            }
        }

        public override void onDetached(Entity ownerEntity)
        {
            base.onDetached(ownerEntity);
            KBEngine.Event.deregisterOut(this);
        }

        public void say(string context)
        {
            cellEntityCall.say(((GameObject)(this.owner.renderObj)).name, context);
        }

        public override void reply(string name, string context)
        {
            KBEngine.Event.fireOut("reply", new object[] { name, context });
        }


    }
}
