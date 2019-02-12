using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgent : FrameSyncBehaviour
{
    private KBEngine.Grid gridMap;

    //private AStarPathfinder pathfinder = null;

    private FPVector nodePostion = FPVector.zero;

    private Vector2Int[] path = null;
   
    private FP speed = 10.0f;

    private int stepIndex = 0;

    private FPVector velocity = FPVector.zero;

    public FPVector NodePostion
    {
        get
        {
            return nodePostion;
        }
    }

    public FPVector Velocity
    {
        get
        {
            return velocity;
        }
    }

    public KBEngine.Grid GridMap
    {
        get
        {
            if(gridMap == null)
            {
                gridMap = AstarPath.gridGraph;
            }
            return gridMap;
        }

    }

    public FP Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = FPMath.Min(1.0f,value);
        }
    }

    void ClearPath()
    {
        path = null;
        stepIndex = 0;
        velocity = FPVector.zero;    
    }

    // Use this for initialization
    void Start () {
        RaycastHit floorHit;
        int layer = 1 << LayerMask.NameToLayer("ground");
        if (Physics.Raycast(new Vector3(FPTransform.position.x.AsFloat(), 100, FPTransform.position.z.AsFloat()), -Vector3.up,
            out floorHit, Mathf.Infinity, layer))
        {
            FPTransform.position = new FPVector(FPTransform.position.x,floorHit.point.y, FPTransform.position.z);
        }
    }
	
    public void SetDestination(FPVector dest)
    {
        ClearPath();//清除标记

        Vector2Int start = GridMap.getIndex(FPTransform.position);
        Vector2Int end = GridMap.getIndex(dest);
        path = GridMap.GetPath(start, end/*, new AStarPathfinder(gridMap, 0)*/); // if you use Roy-TAstar navigate,you should call GetPath(start,end) function.
        //if you use Justinhj navigate , you should call GetPath(start,end,new AstarPathfinder(gridMap,0) function.
    }

    private void MoveStep()
    {
        if (!(path != null && path.Length > 0 && stepIndex < path.Length))
        {
            return;
        }

        nodePostion = GridMap.GetCenterPoint(path[stepIndex].x, path[stepIndex].y);
        RaycastHit floorHit;
        int layer = 1 << LayerMask.NameToLayer("ground");
        if (Physics.Raycast(new Vector3(nodePostion.x.AsFloat(),100, nodePostion.z.AsFloat()), -Vector3.up, 
            out floorHit, Mathf.Infinity, layer))
        {
            nodePostion.y = floorHit.point.y;
        }
        else
        {
            nodePostion.y = 0;
        }

        if(FPVector.Distance(FPTransform.position, nodePostion) < Speed* FrameSyncManager.DeltaTime)
        {
            FPTransform.position = nodePostion;
            if(path.Length > 0)
            {
                if(stepIndex < path.Length - 1)
                {
                    
                    stepIndex++;
                    
                }
                else if (stepIndex == path.Length - 1)
                {
                    velocity = FPVector.zero;
                }
            }
        }
        else
        {
            velocity = (nodePostion - FPTransform.position).normalized * Speed * FrameSyncManager.DeltaTime;
            FPTransform.LookAt(new FPVector(nodePostion.x, FPTransform.position.y, nodePostion.z));
            FPTransform.Translate(velocity, Space.World);
        }
    }

    public void StopStep()
    {
        ClearPath();
    }

    public bool isCompleted()
    {
        return path != null && path.Length >0 && stepIndex == (path.Length-1) ;      
    }

    public override void OnSyncedUpdate()
    {
        MoveStep();
    }
}
