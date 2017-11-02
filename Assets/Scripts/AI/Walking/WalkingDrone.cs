﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingDrone : Drone {

    public GameObject wallDetector;
    public GameObject edgeDetector;
    public bool walk = true;
	
    // Use this for initialization
	new void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () {
        base.Update();
	}

    new void FixedUpdate ()
    {
        base.FixedUpdate();
        if (walk)
        {
            rb.velocity = new Vector2(-2 * transform.localScale.x, 0);
        } else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void HitEdge()
    {
        StopCoroutine("TrunAround");
        StartCoroutine("TurnAround");
    }

    IEnumerator TurnAround()
    {
        walk = false;
        yield return new WaitForSeconds(2);
        transform.localScale = new Vector3(
            transform.localScale.x * -1,
            transform.localScale.y,
            transform.localScale.z);
        walk = true;
    }
}