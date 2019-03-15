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

    //unity��vector3��debug��־�Ǿ�ȷ��0.1�ģ���˱����øú�������ӡ��־�����ǹٷ��Ƽ���
    public static void DebugVector3(Vector3 v)
    {
        Log("(" + v.x + "," + v.y + "," + v.z + ")");
    }
}
