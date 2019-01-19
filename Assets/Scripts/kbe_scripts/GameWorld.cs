using KBEngine;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class GameWorld : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        installEvents();
    }

    void installEvents()
    {
        // in world
        KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
        KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
        KBEngine.Event.registerOut("updatePosition", this, "updatePosition"); 
        KBEngine.Event.registerOut("set_position", this, "set_position");
        KBEngine.Event.registerOut("set_direction", this, "set_direction");
    }

    void OnDestroy()
    {
        KBEngine.Event.deregisterOut(this);
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    public void onEnterWorld(KBEngine.Entity entity)
    {
        Debug.Log("WorldEvent:entity." + entity.id + ",claseName:" + entity.className);

        if (entity.className == "Avatar")
        {
            SpaceData.Instance.SpacePlayers.Add(((KBEngine.Avatar)entity).component1);

            SpaceData.Instance.SpacePlayers = SpaceData.Instance.SpacePlayers.OrderBy(s => s.seatNo).ToList();
        }
    }
    public void onLeaveWorld(KBEngine.Entity entity)
    {
        if (entity.renderObj == null)
            return;
        UnityEngine.GameObject.Destroy((UnityEngine.GameObject)entity.renderObj);
        entity.renderObj = null;
    }


    public void updatePosition(KBEngine.Entity entity)
    {
        if (entity.className == "Avatar")
        {
            FrameSyncReportBase comp = SpaceData.Instance.SpacePlayers.Find(s => s.ownerID == entity.id);

            if(comp != null)
            {
                //Debug.Log("updatePosition comp.owner.position:" + comp.owner.position +",entity.posiont:"+entity.position);
            }
        }

    }
    public void set_position(KBEngine.Entity entity)
    {
        if (entity.className == "Avatar")
        {
            FrameSyncReportBase comp = SpaceData.Instance.SpacePlayers.Find(s => s.ownerID == entity.id);

            if (comp != null)
            {
                //Debug.Log("comp.owner.position:" + comp.owner.position + ",entity.position:" + entity.position);
            }
        }
    }

    public void set_direction(KBEngine.Entity entity)
    {
        if (entity.className == "Avatar")
        {
            FrameSyncReportBase comp = SpaceData.Instance.SpacePlayers.Find(s => s.ownerID == entity.id);

            if (comp != null)
            {
                //Debug.Log("comp.owner.direction:" + comp.owner.direction + ",entity.direction:" + entity.direction);
            }
        }
    }


}
