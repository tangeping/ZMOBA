using KBEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTransform : MonoBehaviour {

    // Use this for initialization
    public FPVector dest;
    FPTransform tsTransform;
    FP speed = 40f;
    Actions animator; 

    private void Awake()
    {
        tsTransform = GetComponent<FPTransform>();
        animator = GetComponent<Actions>();
    }
    void Start ()
    {
        tsTransform.position = new FPVector(96.84546f,0f, 93.43513f);
        dest = tsTransform.position;
        StartCoroutine(SyncedUpdate());
        
	}

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit))
            {
                dest = floorHit.point.ToFPVector();
            }
        }
    }

    private void LateUpdate()
    {
        if(dest != tsTransform.position)
        {
            animator.SetState("Run");
        }
        else
        {
            animator.SetState("Idle");
        }
    }

    IEnumerator SyncedUpdate()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.02f);

            if (dest != tsTransform.position)
            {
                if (FPVector.Distance(dest, tsTransform.position) > speed * 0.02f)
                {
                    FPVector velocity = (dest - tsTransform.position).normalized * speed * 0.02f;
                    FPVector dest_position = tsTransform.position + velocity;
                    tsTransform.LookAt(new FPVector(dest_position.x, tsTransform.position.y, dest_position.z));
                    tsTransform.Translate(velocity, Space.World);
                }
                else
                {
                    tsTransform.position = dest;
                }    
            }
        }

    }
}
