using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour {

    SideScrollingPlayer target;
    RoomManager roomManager;
	// Use this for initialization
	void Start () {
        target = FindObjectOfType<SideScrollingPlayer>();
        roomManager = FindObjectOfType<RoomManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate ()
    {
		if (!roomManager.roomTransition && !StateSaver.gameState.paused)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 0.03f);
        }
    }

}
