using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCollier : MonoBehaviour {

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
    }

}
