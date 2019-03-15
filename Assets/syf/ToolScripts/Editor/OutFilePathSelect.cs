using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text;

public class OutFilePathSelect : MonoBehaviour {

    static int index = 0;

    static void Execute(string savepath)
    {
        index = 0;
        string writePath = Directory.GetCurrentDirectory() + savepath;
        if (File.Exists(writePath))
            File.Delete(writePath);
        FileInfo file = new FileInfo(writePath);
        StreamWriter sw = file.AppendText();
        foreach (UnityEngine.Object tmp in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.DeepAssets))
        {
            if (!(tmp is GameObject))
                continue;
            string tempShowPath = AssetDatabase.GetAssetPath(tmp);
            if (!tempShowPath.Contains(".prefab"))
                continue;

            FindScript(tmp as GameObject,tempShowPath, sw);
        }
        sw.Flush();
        sw.Dispose();
        sw.Close();
        EditorUtility.DisplayDialog("提示", "成功！输出路径 ： " + writePath, "OK");
    }


    static void FindScript(GameObject go, string tempShowPath, StreamWriter sw)
    {
        MonoBehaviour[] scripts = go.GetComponentsInChildren<MonoBehaviour>(true);

        if (scripts.Length > 0)
        {
            
            for (int i = 0; i < scripts.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(index);
                sb.Append("  :  ");
                sb.Append(tempShowPath);
                sb.Append("\t\t");

                if (scripts[i] != null)
                {
					continue;
                    sb.Append(scripts[i].gameObject.name);
                    sb.Append("\t\t");
                    sb.Append("Has \"" + scripts[i].GetType().ToString() + "\" Script");
                }
                else
                    sb.Append("Has Null Script");
				index++;
                sb.Append(sw.NewLine);
                sb.Append(sw.NewLine);

                sw.Write(sb.ToString());
            }
        }
    }

    [MenuItem("[OutFilePath] / ForEffect")]
    static void ExecuteForEffect()
    {
        Execute("/Effect.txt");
    }

    [MenuItem("[OutFilePath] / ForMode")]
    static void ExecuteForMode()
    {
        Execute("/Mode.txt");
    }

    [MenuItem("[OutFilePath] / ForScene")]
    static void ExecuteForScene()
    {
        Execute("/Scene.txt");
    }
}
