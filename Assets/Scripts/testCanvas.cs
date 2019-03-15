using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCanvas : MonoBehaviour {

    public GameObject perfab;
    private GameObject victoryPerfab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, -2, 4);
        transform.position = mousePos;

        if(Input.GetMouseButtonDown(0))
        {
            if (victoryPerfab == null)
            {
                victoryPerfab = Instantiate(perfab, transform.position, transform.rotation);
            }
            else
            {
                victoryPerfab.transform.position = transform.position;
                victoryPerfab.transform.rotation = transform.rotation;
            }
        }
	}
}
