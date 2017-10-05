using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour {

    public GameObject follow;
    public float distanceFromPlayer; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(
            follow.transform.position.x,
            follow.transform.position.y,
            distanceFromPlayer
        );
	}
}
