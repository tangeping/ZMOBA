﻿namespace KBEngine
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class FrameSyncReport : FrameSyncReportBase
    {
        public FrameSyncReport() : base()
        {

        }

        public override void onAttached(Entity ownerEntity)
        {
            if (ownerEntity.isPlayer())
            {
                SpaceData.Instance.localPlayer = this;
                KBEngine.Event.registerIn("reportFrame", this, "reportFrame");
            }
        }

        public override void onFrameMessage(FS_FRAME_DATA framedata)
        {
            SpaceData.Instance.frameList.Enqueue(framedata);
            //Debug.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff")+",onFrameMessage:" + framedata.frameid + framedata.operation.Count );
        }



        public virtual void reportFrame(FS_ENTITY_DATA operation)
        {
            operation.entityid = ownerID;
            operation.cmd_type = 1;
            cellEntityCall.reportFrame(operation);
            //Debug.Log(System.DateTime.Now + "." + System.DateTime.Now.Millisecond+",reportFrame:" + operation.entityid + operation.cmd_type + operation.datas);
        }
    }
}
