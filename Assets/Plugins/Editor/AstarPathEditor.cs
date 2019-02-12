using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KBEngine
{
    [CustomEditor(typeof(AstarPath))]
    public class AstarPathEditor : Editor
    {
        /// <summary>
        /// Holds various URLs and text for the editor.
        /// This info can be updated when a check for new versions is done to ensure that there are no invalid links.
        /// </summary>
        static Dictionary<string, string> URLData = new Dictionary<string, string> {
            { "URL:forum", "https://bbs.comblockengine.com/" },
            { "URL:download", "https://github.com/kbengine/kbengine.git" },
            { "URL:documentation", "https://www.comblockengine.com/docs/api/" },
            { "URL:homepage", "https://www.comblockengine.com/" }
        };

        public static string GetURL(string tag)
        {
            string url;
            URLData.TryGetValue("URL:" + tag, out url);
            return url ?? "";
        }



        /// <summary>AstarPath instance that is being inspected</summary>
        public AstarPath script { get; private set; }
     

        /// <summary>Tell Unity that we want to use the whole inspector width</summary>
//         public override bool UseDefaultMargins()
//         {
//             return false;
//         }

        private void DrawGridGraph()
        {
            script = target as AstarPath;

            EditorGUILayout.LabelField("Grid Graph", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            script.grapType = (GridShapeType)EditorGUILayout.EnumPopup("Grap Type", script.grapType);

            script.gridX = EditorGUILayout.IntField(new GUIContent("Width (nodes)", "Width of the graph in nodes"), script.gridX);
            if (!(script.gridX >0 && script.gridX %2 == 0))
            {
                throw new ArgumentOutOfRangeException("Width should be grater 0 and Divided by 2", "x");
            }

            script.gridY = EditorGUILayout.IntField(new GUIContent("Height (nodes)", "Height of the graph in nodes"), script.gridY);
            if (!(script.gridY > 0 && script.gridY % 2 == 0))
            {
                throw new ArgumentOutOfRangeException("Height should be grater 0 and Divided by 2", "x");
            }

            EditorGUILayout.BeginHorizontal();
            script.m_terrian = EditorGUILayout.ObjectField("Terrain", script.m_terrian, typeof(Terrain), true) as Terrain;
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
            GUILayout.Space(10);

            if (GUILayout.Button(new GUIContent("Scan", "Recalculate all graphs.")))
            {
                if (script != null)
                {
                    script.LoadMap();
                }
            }
        }



        TextAsset SaveGraphData(string bytes, TextAsset target = null)
        {
            string projectPath = System.IO.Path.GetDirectoryName(Application.dataPath) + "/";

            string path;

            if (target != null)
            {
                path = AssetDatabase.GetAssetPath(target);
            }
            else
            {
                // Find a valid file name
                int i = 0;
                do
                {
                    path = "Assets/GraphCaches/GraphCache" + (i == 0 ? "" : i.ToString()) + ".json";
                    i++;
                } while (System.IO.File.Exists(projectPath + path));
            }

            string fullPath = projectPath + path;
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(fullPath));
            var fileInfo = new System.IO.FileInfo(fullPath);
            // Make sure we can write to the file
            if (fileInfo.Exists && fileInfo.IsReadOnly)
                fileInfo.IsReadOnly = false;
            System.IO.File.WriteAllText(fullPath, bytes);

            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath<TextAsset>(path);
        }

        private void DrawFileSetting()
        {
            GUILayout.Space(10);

            EditorGUILayout.LabelField("Save & Load", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            script.m_data.file_cachedStartup = EditorGUILayout.ObjectField("Cache File", script.m_data.file_cachedStartup, typeof(TextAsset), false) as TextAsset;
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Generate cache"))
            {
                //var serializationSettings = new Pathfinding.Serialization.SerializeSettings();
                // serializationSettings.nodes = true;

                if (EditorUtility.DisplayDialog("Scan before generating cache?", "Do you want to scan the graphs before saving the cache.\n" +
                        "If the graphs have not been scanned then the cache may not contain node data and then the graphs will have to be scanned at startup anyway.", "Scan", "Don't scan"))
                {

                    string jstring =  script.SerializeGrapData();

                    script.m_data.file_cachedStartup = SaveGraphData(jstring);
                }
            }

            if (GUILayout.Button("Load from cache"))
            {
                if (EditorUtility.DisplayDialog("Are you sure you want to load from cache?", "Are you sure you want to load graphs from the cache, this will replace your current graphs?", "Yes", "Cancel"))
                {
                    script.m_data.LoadFromCache();
                }
            }

            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }
        public override void OnInspectorGUI()
        {
            // Do some loading and checking
            //serializedObject.Update();

            DrawGridGraph();

            DrawFileSetting();

            DrawAbout();

        }

        private void DrawAbout()
        {
            GUILayout.Space(10);

            EditorGUILayout.LabelField("About", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            //EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("Documentation", "Open the documentation for the api")))
            {
                Application.OpenURL(GetURL("documentation"));
            }

            if (GUILayout.Button(new GUIContent("Project Homepage", "Open the homepage for kbengine")))
            {
                Application.OpenURL(GetURL("homepage"));
            }
            //EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }

    }

}

