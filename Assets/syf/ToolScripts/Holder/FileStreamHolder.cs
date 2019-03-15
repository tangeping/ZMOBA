using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Text;
using System;

[System.Serializable]
public class FileStreamElement
{
    public string[] content;
}

public class FileStreamHolder : ScriptableObject
{
    public List<FileStreamElement> content;
	 
    public void Init(ArrayList list)
    {
        content = new List<FileStreamElement>();
        foreach (ArrayList array in list)
        {
            FileStreamElement fse = new FileStreamElement();
            fse.content = new string[array.Count];
            for(int j = 0,count = array.Count;j < count;++j)
            {
                fse.content[j] = (string)array[j];
            }
            content.Add(fse);
        }
    }
}

//public class FileStreamHolder : ScriptableObject
//{
//    public string content;
   
//    public FileStreamHolder(ArrayList content)
//    {
//        StringBuilder sb = new StringBuilder();
//        foreach (ArrayList i in content)
//        {
//            foreach (string j in i)
//            {
//                sb.Append(j);

//                if(i[i.Count-1].ToString() != j)
//                {
//                    sb.Append(",");
//                }
                
//            }
//            sb.Append(Environment.NewLine);
//        }
//        this.content = sb.ToString();
//    }
//}

//public class FileStreamTranslate
//{
//    public ArrayList m_ArrayList;

//    public void BeginTranslate( string content )
//    {
//        m_ArrayList = new ArrayList();

//        string[] contents = content.Split(Environment.NewLine.ToCharArray());
//        foreach (string s1 in contents)
//        {
//            if (s1.Length <= 0) continue;

//            ArrayList array = new ArrayList();
//            string[] field = s1.Split(',');

//            foreach (string s2 in field)
//            {
//                array.Add(s2);
//            }

//            m_ArrayList.Add(array);
//        }

//    }

//    public void EndTranslate()
//    {
//        m_ArrayList.Clear();
//    }
//}