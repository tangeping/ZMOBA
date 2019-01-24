using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : FrameSyncBehaviour
{
    private KBEngine.Grid gridMap;

    private FPVector dest = FPVector.zero;

    private AStarPathfinder pathfinder = null;

    private FPVector nodePostion = FPVector.zero;

    private Vector2Int[] path = null;

    [HideInInspector]
    public FP Speed = 2.0f;

    private int stepIndex = 0;

    public FPVector NodePostion
    {
        get
        {
            return nodePostion;
        }
    }

    // Use this for initialization
    void Start () {

        gridMap = AstarPath.gridGraph;

        pathfinder = new AStarPathfinder(gridMap, 0);

        if (AstarPath.gridGraph == null)
        {
            Debug.LogError("AstarPath.gridGraph is null ");
        }
    }
	
    public void SetDestination(FPVector dest)
    {
        Vector2Int start = gridMap.getIndex(FPTransform.position);
        Vector2Int end = gridMap.getIndex(dest);
        path = gridMap.GetPath(start, end, pathfinder);
        stepIndex = 0;
    }

    private void MoveStep()
    {
        if (!(path != null && path.Length > 0 && stepIndex < path.Length))
        {
            return;
        }

        nodePostion = gridMap.GetCenterPoint(path[stepIndex].x, path[stepIndex].y);

        FPVector horizonPos = new FPVector(FPTransform.position.x, 0, FPTransform.position.z);

        if(FPVector.Distance(horizonPos,nodePostion) < Speed* FrameSyncManager.DeltaTime)
        {
            FPTransform.position = new FPVector(nodePostion.x, FPTransform.position.y, nodePostion.z);
            if(path.Length > 0 && stepIndex < path.Length-1)
            {
                stepIndex++;
            }
        }
        else
        {
            FPVector moveDir = (nodePostion - horizonPos).normalized;
            FPTransform.LookAt(new FPVector(nodePostion.x, FPTransform.position.y, nodePostion.z));
            FPTransform.Translate(moveDir * Speed * FrameSyncManager.DeltaTime, Space.World);
        }
    }

    public override void OnSyncedUpdate()
    {
        MoveStep();
    }
}
