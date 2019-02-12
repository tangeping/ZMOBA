using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTransform : MonoBehaviour {

	// Use this for initialization
	void Start () {

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Debug.Log(child.gameObject.name + "---------position:" + child.transform.position + ",rotation:" + child.transform.rotation
                +",Euler:"+child.transform.eulerAngles);
        }
        
	}
	

}
