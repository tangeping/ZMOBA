using UnityEngine;
using System.Collections;

public class MyLog 
{
    public static void Log(object msg)
    {
#if UNITY_EDITOR || UNITY_WEBPLAYER
        Debug.Log(msg);
#endif
    }

    public static void LogWarning(object msg)
    {
#if UNITY_EDITOR || UNITY_WEBPLAYER
        Debug.LogWarning(msg);
#endif
    }

    public static void LogError(object msg)
    {
#if UNITY_EDITOR || UNITY_WEBPLAYER
        Debug.LogError(msg);
#endif
    }

    //unity里vector3的debug日志是精确到0.1的，因此必须用该函数来打印日志，这是官方推荐的
    public static void DebugVector3(Vector3 v)
    {
        Log("(" + v.x + "," + v.y + "," + v.z + ")");
    }
}
