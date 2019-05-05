using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KBEngine
{
    public class SnapshotWriter : CBSingleton<SnapshotWriter>
    {
        private List<object> entityList = new List<object>();
        private List<string> nameList = new List<string>();

       public void AddEntity(object ob,string name)
       {
            entityList.Add(ob);
            nameList.Add(name);
       }
        public void RemoveEntity(object ob,string name)
        {
            entityList.Remove(ob);
            nameList.Remove(name);
        }
       
        public void Clear()
        {
            entityList.Clear();
            nameList.Clear();
        }
        public string SerizlizeEntity(object Obj)
        {
            string ss = "";

            Type entity = Obj.GetType();

            foreach (var propertyInfo in entity.GetProperties())
            {
                ss += propertyInfo.Name + "=" + propertyInfo.GetValue(Obj);
            }
            return ss;
        }

       public void Writer()
        {         
            for (int i = 0; i < entityList.Count; i++)
            {
                string s = "[frameID:" + FrameSyncManager.CurrFrameID.ToString() + "]";
                s +=  SerizlizeEntity(entityList[i]);
                Logger.Debug(s, false, nameList[i]);
            }
        }

        public void RecordLog()
        {
            Writer();
        }
    }
}

