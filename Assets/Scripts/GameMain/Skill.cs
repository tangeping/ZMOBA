using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill 
{
    public D_SKILL_INFOS info = null;

    public bool learned;
    public int level;
    public UInt32 castTimeEnd; // server time
    public UInt32 cooldownEnd; // server time
    public UInt32 buffTimeEnd; // server time

    public Skill(D_SKILL_INFOS f)
    {
        this.info = f;
        castTimeEnd = cooldownEnd = buffTimeEnd = FrameSyncManager.CurrFrameID;
    }

    public float CastTimeRemaining()
    {
        // how much time remaining until the casttime ends? (using server time)
        return FrameSyncManager.CurrFrameID >= castTimeEnd ? 0 : castTimeEnd - FrameSyncManager.CurrFrameID;
    }

    public UInt32 CooldownRemaining()
    {
        // how much time remaining until the cooldown ends? (using server time)
        return FrameSyncManager.CurrFrameID >= cooldownEnd ? 0 : cooldownEnd - FrameSyncManager.CurrFrameID;
    }
    public float BuffTimeRemaining()
    {
        // how much time remaining until the buff ends? (using server time)
        return FrameSyncManager.CurrFrameID >= buffTimeEnd ? 0 : buffTimeEnd - FrameSyncManager.CurrFrameID;
    }

    public bool IsCasting()
    {
        // we are casting a skill if the casttime remaining is > 0
        return CastTimeRemaining() > 0;
    }

    public bool IsReady()
    {
        return CooldownRemaining() == 0;
    }

}
