using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : FrameSyncBehaviour
{
    private NavAgent agent;

    int floorMask;

    float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    private void Awake()
    {
        agent = GetComponent<NavAgent>();
    }
    private  void Start()
    {
        floorMask = LayerMask.GetMask("ground");
    }

    public override void OnSyncedInput()
    {
        bool MouseRight = Input.GetMouseButtonDown(1);

        if (MouseRight && !GameUtils.IsCursorOverUserInterface())//如果鼠标不是点击在UI上面
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;

            if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
            {
                FrameSyncInput.SetFPVector((byte)InputCmd.MOUSE_POINT,floorHit.point.ToFPVector());
                FrameSyncInput.SetFPVector((byte)InputCmd.MOUSE_NORMAL, floorHit.normal.ToFPVector());
            }

        }
                  
    }

    public override void OnSyncedUpdate()
    {
        if(!FrameSyncInput.HasFPVector((byte)InputCmd.MOUSE_POINT) && !FrameSyncInput.HasFPVector((byte)InputCmd.MOUSE_NORMAL))
        {
            return;
        }
        FPVector mousePoint = FrameSyncInput.GetFPVector((byte)InputCmd.MOUSE_POINT);
        FPVector mouseNormal = FrameSyncInput.GetFPVector((byte)InputCmd.MOUSE_NORMAL);
        SetIndicatorViaPosition(mousePoint.ToVector(), mouseNormal.ToVector());
        agent.SetDestination(mousePoint);
    }



    [Header("Indicator")]
    public GameObject indicatorPrefab;

    private GameObject indicator;

    public void SetIndicatorViaPosition(Vector3 pos, Vector3 normal)//地图上标记目的地
    {
        if (!owner.isPlayer()) return;
        if (!indicator) indicator = Instantiate(indicatorPrefab);
        indicator.transform.parent = null;
        indicator.transform.position = pos + Vector3.up * 0.01f;
        indicator.transform.up = normal; // adjust to terrain normal
    }
}
