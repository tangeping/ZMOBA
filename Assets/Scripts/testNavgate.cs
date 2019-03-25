using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavAgent))]
public class testNavgate : MonoBehaviour {

    private NavAgent _agent;
    private FPTransform tsTransfrom;

    private KBEngine.Grid gridMap;
    public KBEngine.Grid GridMap { get { if (gridMap == null) gridMap = AStarPath.GridGraph; return gridMap; } }

    public NavAgent Agent
    {
        get
        {
            if(_agent==null)
            {
                _agent = GetComponent<NavAgent>();
            }
            return _agent;
        }

        set
        {
            _agent = value;
        }
    }

    private void Awake()
    {
        tsTransfrom = GetComponent<FPTransform>();
    }

    // Use this for initialization
    void Start () {
		
	}

    FPVector GetOnGroundPoint(FPVector point)
    {
        FPVector result = point;
        RaycastHit floorHit;
        int layer = 1 << LayerMask.NameToLayer("ground");
        if (Physics.Raycast(new Vector3(result.x.AsFloat(), 100, result.z.AsFloat()), -Vector3.up,
            out floorHit, Mathf.Infinity, layer))
        {
            result.y = floorHit.point.y;
        }
        return result;
    }

    public void SetDestination(FPVector dest)
    {
        dest = GetOnGroundPoint(dest);
        Vector2Int startNode = GridMap.getIndex(tsTransfrom.position);
        Vector2Int endNode = GridMap.getIndex(dest);
   
        TestPathFinder pathFinder = new TestPathFinder(GridMap);

        List<Vector2Int> path = pathFinder.PathFind(startNode, endNode);

        Debug.Log("startNode:"+ startNode + ",endNode:"+ endNode+ ",paht.length:" + (path != null ? path.Count.ToString() : "null"));
    }
        // Update is called once per frame
        void Update ()                                              
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit,Mathf.Infinity,LayerMask.GetMask("ground")))
            {
                var dest = floorHit.point.ToFPVector();

                if (Agent)
                {
                    SetDestination(dest);
                }
                else
                {
                    Debug.Log(name + "-------------agent=-----------:" + Agent);
                }
            }
        }
    }
}
