using KBEngine;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameWorld : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("login");
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
        KBEngine.Event.registerOut("onStreamDataStarted", this, "onStreamDataStarted");
        KBEngine.Event.registerOut("onStreamDataRecv", this, "onStreamDataRecv");
        KBEngine.Event.registerOut("onStreamDataCompleted", this, "onStreamDataCompleted");

        // common
        KBEngine.Event.registerOut("onKicked", this, "onKicked");
        KBEngine.Event.registerOut("onDisconnected", this, "onDisconnected");
        KBEngine.Event.registerOut("onConnectionState", this, "onConnectionState");
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
    /// <summary>
    /// Start downloading data.
    /// <para> param1(uint16): resource id</para>
    /// <para> param2(uint32): data size</para>
    /// <para> param3(string): description</para>
    /// </summary>
    public void onStreamDataStarted(UInt16 resource_id, UInt32 size,string description)
    {
        if(!SpaceData.Instance.files.ContainsKey(resource_id))
        {
            FileBlock fileBlock = new FileBlock(resource_id,size);
            SpaceData.Instance.files[resource_id] = fileBlock;
        }
        
    }

    /// <summary>
    /// Receive data.
    /// <para> param1(uint16): resource id</para>
    /// <para> param2(bytes): datas</para>
    /// </summary>
    public void onStreamDataRecv(UInt16 resource_id,byte[]datas)
    {
        if (SpaceData.Instance.files.ContainsKey(resource_id))
        {
            //BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count);
            FileBlock fileBlock = SpaceData.Instance.files[resource_id];
            fileBlock.WriteBuffer(datas);
        }
    }
    /// <summary>
    /// The downloaded data is completed.
    /// <para> param1(uint16): resource id</para>
    /// </summary>
    public void onStreamDataCompleted(UInt16 resource_id)
    {
        if (SpaceData.Instance.files.ContainsKey(resource_id))
        {
            //BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count);
            FileBlock fileBlock = SpaceData.Instance.files[resource_id];
            fileBlock.compeleted = true;
        }
    }

    public void onKicked(UInt16 failedcode)
    {
        Debug.Log("kick, disconnect!, reason=" + KBEngineApp.app.serverErr(failedcode));
        SceneManager.LoadScene("login");
        SpaceData.Instance.Clear();

    }
    public void onDisconnected()
    {
        Debug.Log("disconnect! will try to reconnect...(你已掉线，尝试重连中!)");
        //Invoke("onReloginBaseappTimer", 1.0f);
    }

    public void onConnectionState(bool success)
    {
        if (!success)
            Debug.Log("connect(" + KBEngineApp.app.getInitArgs().ip + ":" + KBEngineApp.app.getInitArgs().port + ") is error! (连接错误)");
        else
            Debug.Log("connect successfully, please wait...(连接成功，请等候...)");
    }

}
