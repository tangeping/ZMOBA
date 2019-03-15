using UnityEngine;
using System;
using System.Collections;


//该类用来跨程序集创建实例的过渡类
//public class DataConfig
//{
//    public static object CreateInstance(string ClassName,params object[] args)
//    {
//        Type type = Type.GetType(ClassName);
//
//        if (type == null)
//        {
//            return null;
//        }
//
//        return Activator.CreateInstance(type, args);
//    }
//}

//用来存储FileConfig.csv的内容
public class CConfigAttribute
{
    public string m_ResourceName;
    public string m_PackageName;
    public string m_ManagerName;

    public bool bUpdateSelf(ArrayList list)
    {
        if (list[0].ToString() == "") return false;
        if (list.Count < 3) return false;

        this.m_ResourceName = list[0].ToString();
        this.m_PackageName = list[1].ToString();
        this.m_ManagerName = list[2].ToString();

        return true;
    }

}
