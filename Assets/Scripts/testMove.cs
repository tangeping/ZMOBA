using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMove : FrameSyncBehaviour
{

    [SerializeField] private FP accellRate = 10.0f;

    [SerializeField] private FP steerRate = 10.0f;

    [AddTracking]
    public int deaths = 0;

    public override void OnSyncedInput()
    {
        FP accell = Input.GetAxis("Vertical");
        FP steer = Input.GetAxis("Horizontal");

        FrameSyncInput.SetFP(0, accell);
        FrameSyncInput.SetFP(1, steer);
    }

    public override void OnSyncedUpdate()
    {
        FP accell = FrameSyncInput.GetFP(0);
        FP steer = FrameSyncInput.GetFP(1);

        accell *= accellRate * FrameSyncManager.DeltaTime;
        steer *= steerRate * FrameSyncManager.DeltaTime;

        FPTransform.Translate(0, 0, accell, Space.Self);
        FPTransform.Rotate(0, steer, 0);
    }

    public override void OnSyncedStart()
    {
        FPTransform.position = FPVector.zero + FPVector.forward * teamID;
        
    }
    public void OnSyncedTriggerEnter(FPCollision other)
    {
        Debug.Log(gameObject.name + ".TriggerEnter:" + other.gameObject.name);

    }

}
