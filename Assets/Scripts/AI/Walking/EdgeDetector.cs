using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetector : MonoBehaviour {

    public WalkingDrone drone;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<SideScrollingPlayer>() == null)
        {
            Debug.Log(col.gameObject.name);
            drone.HitEdge();
        }
    }
}
