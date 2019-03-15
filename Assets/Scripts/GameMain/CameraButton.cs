using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class CameraButton : MonoBehaviour {

    private void Start()
    {
         if (SpaceData.Instance.getLocalTeam() == Team.Evil)
         {
            transform.position = new Vector3(140f, 28, 128f);
//              transform.position = new Vector3(153.3f, 23.8f, 151.3f);
//              transform.rotation = new Quaternion(0.1f, -0.9f, 0.3f, 0.4f);
         }
        //         if (SpaceData.Instance.getLocalTeam() == Team.Good)
        //         {
        //             transform.position = new Vector3(12.3f, 28.0f, -9.2f);
        //             transform.rotation = new Quaternion(0.4f, 0.0f, 0.0f, 0.9f);
        //         }
        //Debug.Log(name + ",position:" + transform.position + ",rotation:" + transform.rotation);
    }
    private void OnGUI()
    {
        if(GUILayout.Button("Stop"))
        {
            KBEngine.Event.fireIn("reqStop");
        }
        else if (GUILayout.Button("Run"))
        {
            KBEngine.Event.fireIn("reqRun");
        }
    }
//     private void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             transform.position = new Vector3(86, 28, 66);
//         }
//     }

}
