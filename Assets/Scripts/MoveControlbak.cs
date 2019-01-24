using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControlbak : MonoBehaviour {

    // Use this for initialization

    public Transform dst;

    private KBEngine.Grid navigate;

    public float Speed = 0.1f;

    Vector2Int[] path;

    Vector3 destination = Vector3.zero;

    int stepIndex = 0;

    CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        if (AstarPath.gridGraph == null)
        {
            Debug.LogError("AstarPath.gridGraph is null ");
        }

        navigate = AstarPath.gridGraph;
        Vector2Int start = navigate.getIndex(transform.position.ToFPVector());
        Vector2Int end = navigate.getIndex(dst.position.ToFPVector());

        /*Vector2Int[]  path  = navigate.GetPath(start,end);*/

        path = navigate.GetPath(start, end, new AStarPathfinder(navigate,1));
        Debug.Log(gameObject.name + ",start:" + start + ",end:" + end + ",path.step:" + path.Length);
        
        if(path.Length > 0)
        {
            destination = navigate.GetCenterPoint(path[stepIndex].x, path[stepIndex].y).ToVector();
        }
    }


    private void FixedUpdate()
    {
        if(!(path != null && path.Length > 0 && stepIndex < path.Length-1))
        {
            return;
        }

        float distance = Vector3.Distance(new Vector3(transform.position.x, 0.0f, transform.position.z), destination);
        if (distance >= Speed * Time.deltaTime)
        {
            return;
        }

        stepIndex++;

        destination = navigate.GetCenterPoint(path[stepIndex].x, path[stepIndex].y).ToVector();

        Debug.Log("------------------------------->destination:" + destination + ",stepIndex:" + stepIndex);
    }

    void moveOnGround()
    {
        RaycastHit hitinfo;
        if (Physics.Raycast(transform.position, -Vector3.up, out hitinfo))
        {
            Debug.Log(hitinfo.collider.name+":" + hitinfo.point + ",destination:" + destination);
            //transform.position = new Vector3(transform.position.x, hitinfo.point.y,transform.position.z);
        }
    }

    void Move()
    {
        Vector3 Horizontal = new Vector3(transform.position.x, 0, transform.position.z);

        float distance = Vector3.Distance(Horizontal, destination);

        float step = Speed * Time.deltaTime;

        if (distance < step)
        {
            Horizontal = destination;
        }
        else
        {
            Horizontal += (destination - Horizontal).normalized * step;
        }

        transform.position = new Vector3(Horizontal.x, transform.position.y, Horizontal.z);
    }

    const float gravity = 20.0f;
    private Vector3 moveDirection = Vector3.zero;

    void ControllMove()
    {
        if(controller.isGrounded)
        {
            //moveDirection.y -= 0.01f;

            Vector3 Horizontal = new Vector3(transform.position.x, 0, transform.position.z);
            moveDirection = (destination - Horizontal).normalized * Speed;
            //Debug.Log("on ground");
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
            //Debug.Log("on the air");
        }

        //Debug.Log("moveDirection:" + moveDirection);

        controller.Move(moveDirection * Time.deltaTime);
    }
    private void Update()
    {
        // Move our position a step closer to the target.

        //transform.position = Vector3.MoveTowards(transform.position, destination, Speed * Time.deltaTime);

        //moveOnGround();

        ControllMove();

        //Move();
    }
}
