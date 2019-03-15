using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KBEngine;


[CustomEditor(typeof(AStarPath))]
public class AStarPathEditor : Editor
{
    public Dictionary<int, GraphicEditorBase> graphEditors = new Dictionary<int, GraphicEditorBase>();
    static List<string> layersName = new List<string>();

    AStarPath aStarPathScript;
    public override void OnInspectorGUI()
    {
        int editorNum = graphEditors.Count;
        int graphNum = aStarPathScript.Graphs.Count;

        if (editorNum == 0 && graphNum == 0)
        {
            AddGraph(AStarData.GraphType.GridGraph);
            graphNum = 1;
        }

        if (editorNum < graphNum)
        {
            for (int i = editorNum; i < graphNum; i++)
            {
                var graphObj = aStarPathScript.Graphs[i];
                CreateGraphEditor(graphObj.graphType, graphObj);
            }
        }

        Dictionary<int, GraphicEditorBase>.KeyCollection keyCol = graphEditors.Keys;
        foreach(int graphIndex in keyCol)
        { 
            if (graphEditors[graphIndex] == null)
            {
                Debug.Log("graphEditors graphIndex[" + graphIndex + "] is null!" );
                continue;
            }
            graphEditors[graphIndex].DrawInspectorGUI();
        }

        bool flag = aStarPathScript.ShowGraphs;
        aStarPathScript.ShowGraphs = EditorGUILayout.Toggle("Show Graphs", aStarPathScript.ShowGraphs);
        if (flag != aStarPathScript.ShowGraphs)
        {
            if (!Application.isPlaying || EditorApplication.isPaused) SceneView.RepaintAll();
        }

        if (GUILayout.Button(new GUIContent("Scan", "Recaculate all graphs")))
        {
            for(int i = 0; i < graphNum; i++)
            {
                if (aStarPathScript.Graphs[i] == null)
                {
                    Debug.Log("aStarPathScript.Graphs[" + i + "] is null!");
                    continue;
                }
                aStarPathScript.Graphs[i].ScanGraphicInternal();
            }
        }

        EditorGUILayout.BeginHorizontal();
        aStarPathScript.file_cachedStartup = EditorGUILayout.ObjectField("Cache File", aStarPathScript.file_cachedStartup, typeof(TextAsset), false) as TextAsset;
        EditorGUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Export", "Export graph data")))
        {
            for (int i = 0; i < graphNum; i++)
            {
                if (aStarPathScript.Graphs[i] == null)
                {
                    Debug.Log("Export_aStarPathScript.Graphs[" + i + "] is null!");
                    continue;
                }
                aStarPathScript.Graphs[i].ExportGraphData();
            }
        }

        if (GUILayout.Button("Load from cache"))
        {
            if (EditorUtility.DisplayDialog("Are you sure you want to load from cache?", "Are you sure you want to load graphs from the cache, this will replace your current graphs?", "Yes", "Cancel"))
            {
                aStarPathScript.LoadFromCache();
            }
        }

        GUILayout.EndHorizontal();
        EditorGUI.indentLevel--;

    }

    public void OnEnable()
    {
        aStarPathScript = target as AStarPath;
        aStarPathScript.OnDrawGizmosCallback = OnDrawGizmos;
    }

    public void AddGraph(AStarData.GraphType type)
    {
        aStarPathScript.AddGraph(type);
    }

    private void CreateGraphEditor(int type, GraphBase graphObj)
    {
        CreateGraphEditor((AStarData.GraphType)(type), graphObj);
    }

    private void CreateGraphEditor(AStarData.GraphType type, GraphBase graphObj)
    {
        switch (type)
        {
            case AStarData.GraphType.GridGraph:
                {
                    GridGraphEditor gridGraph = new GridGraphEditor();
                    gridGraph.targetGraph = graphObj;
                    graphEditors.Add(graphObj.graphIndex, gridGraph);
                }
                break;
            default:
                break;
        }
    }

    public void OnDrawGizmos()
    {
        int graphNum = aStarPathScript.Graphs.Count;
        for (int i = 0; i < graphNum; i++)
        {
            var graphObj = aStarPathScript.Graphs[i];
            graphObj.OnDrawGizmos(true);
        }
    }


    public static string[] GetLayerMaskField(bool refreshFlag = false)
    {
        if (!refreshFlag && layersName.Count != 0) return layersName.ToArray();
        if (layersName.Count != 0)
        {
            layersName.Clear();
        }

        int emptyLayers = 0;
        for (int i = 0; i < 32; i++)
        {
            string layerName = LayerMask.LayerToName(i);

            if (layerName != "")
            {

                for (; emptyLayers > 0; emptyLayers--) layersName.Add("Layer " + (i - emptyLayers));
                layersName.Add(layerName);
            }
            else
            {
                emptyLayers++;
            }
        }

        return layersName.ToArray();
    }
}

