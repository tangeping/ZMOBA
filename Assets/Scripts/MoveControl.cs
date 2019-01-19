using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour {

    // Use this for initialization

    public Transform dst;

    private KBEngine.Grid navigate;

    public float[,] cost;
    private void Start()
    {
        if (AstarPath.gridGraph == null)
        {
            Debug.LogError("AstarPath.gridGraph is null ");
        }

        navigate = AstarPath.gridGraph;

        cost = new float[navigate.DimX, navigate.DimY];

//         for (int i = 0; i < navigate.DimX; i++)
//         {
//             for (int j = 0; j < navigate.DimY; j++)
//             {
//                 cost[i, j] = navigate.GetCellCost(new Vector2Int(i, j)).AsFloat();
//             }
// 
//         }

        Vector2Int start = navigate.getIndex(transform.position.ToFPVector());
        Vector2Int end = navigate.getIndex(dst.position.ToFPVector());

        Vector2Int[]  path  = navigate.GetPath(start,end);
        Debug.Log(gameObject.name + ",start:" + start + ",end:" + end + ",path.step:" + path.Length);

//         for (int i = 0; i < path.Length; i++)
//         {
//             Debug.Log("node:" + path[i] +",position:"+ navigate.GetCenterPoint(path[i].x,path[i].y));
//         }
        
    }

}
