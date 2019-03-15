using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testShooting : MonoBehaviour {

    public Transform target;
    public ParticleSystem gunParticles;
    public LineRenderer gunLine;

    public float timeBetweenBullets = 0.15f;
    float effectsDisplayTime = 0.2f;

    //     private void Awake()
    //     {
    //         gunParticles = GetComponent<ParticleSystem>();
    /*        gunLine = GetComponent<LineRenderer>();*/
    //     }
    //     // Use this for initialization
    void Start()
    {
        /*Debug.Log("transform.position:" + transform.position +",local_position:" + transform.localPosition);*/
    }

    float timer; 
    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }

        //if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
    }

    void Shoot()
    {
        if (target)
        {
            gunParticles.Stop();
            gunParticles.Play();

            gunLine.enabled = true;
            gunLine.SetPosition(0, gunParticles.transform.position);
            gunLine.SetPosition(1, target.position);
        }

    }

}
