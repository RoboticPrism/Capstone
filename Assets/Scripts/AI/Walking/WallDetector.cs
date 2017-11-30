using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour {

    public WalkingDrone drone;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<SideScrollingPlayer>() == null)
        {
            Debug.Log(col.gameObject.name);
            drone.HitEdge();
        }
    }
}
