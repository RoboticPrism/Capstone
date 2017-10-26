using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightRange : MonoBehaviour {

    public Drone drone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.GetComponent<SideScrollingPlayer>())
        {
            drone.PlayerEnteredSight();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<SideScrollingPlayer>())
        {
            drone.PlayerLeftSight();
        }
    }
}
