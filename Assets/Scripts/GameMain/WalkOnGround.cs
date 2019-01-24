using UnityEngine;

public class WalkOnGround : MonoBehaviour 
{
    CharacterController controller;

    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        if (controller.isGrounded)
        {
            //moveDirection.y -= 0.01f;
        }
        else
        {
            moveDirection.y -= FrameSyncManager.Config.gravity3D.y.AsFloat() * Time.deltaTime;
        }

        //Debug.Log("moveDirection:" + moveDirection);

        controller.Move(moveDirection * Time.deltaTime);
    }
}
