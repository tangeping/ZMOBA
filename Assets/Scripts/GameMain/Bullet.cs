﻿using KBEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // Use this for initialization
    private float speed = 24f;
    private float Delay = 12f;
    [HideInInspector] public Transform target;
	void Start () {

        Destroy(gameObject, Delay);
	}
	

	// Update is called once per frame
	void Update () {

        if(target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized * speed * Time.deltaTime;
            transform.Translate(dir);
        }      
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.CompareTo("Ground") == 0)
        {
            Destroy(gameObject);
        }
    }
}