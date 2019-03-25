using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRaycast : MonoBehaviour {

    FPVector p1 = new FPVector(FP.FromRaw(221360928661176319), FP.FromRaw(-6686944727578705920), FP.FromRaw(6345679962111999999));
    FPVector p2 = new FPVector(5.154f, -1.557f, 1.4775f);
    
    // Use this for initialization
    void Start () {

        Debug.Log("p1:" + p1 + ",p2:" + p2);

        Vector3Int temp = (p1 * 1000).ToVector3Int();

        p1 = temp.ToFPVector() / 1000;

        p1.Normalize();
        p2.Normalize();

        Debug.Log("p2.Normalize:" + p2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
