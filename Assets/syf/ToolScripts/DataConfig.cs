using UnityEngine;
using System;
using System.Collections;


//������������򼯴���ʵ���Ĺ�����
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

//�����洢FileConfig.csv������
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
