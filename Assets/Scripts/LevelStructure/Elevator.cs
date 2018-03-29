using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour {

	public Transform myDestination;

	public Room myRoom;

	public bool up;
	public bool down;

	public GameObject upObj;
	public GameObject downObj;

	public Elevator downDest;
	public Elevator upDest;

	private SideScrollingPlayer player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoingUp(){
		player.ElevatorRide (upDest);
	}

	public void GoingDown(){
		player.ElevatorRide (downDest);
	}

	public Transform GetMyDestination()
	{
		return myDestination;
	}

	public void OnTriggerEnter2D(Collider2D other){
		SideScrollingPlayer player = other.GetComponent<SideScrollingPlayer> ();
		if (player != null)
		{
			this.player = player;
			downObj.gameObject.SetActive (down);
			upObj.gameObject.SetActive (up);
		}
	}

	public void OnTriggerExit2D(Collider2D other){
		SideScrollingPlayer player = other.GetComponent<SideScrollingPlayer> ();
		if (player != null)
		{
			downObj.gameObject.SetActive (false);
			upObj.gameObject.SetActive (false);
		}
	}

	// Get the room this door is in
	public Room GetMyRoom()
	{
		return myRoom;
	}	
}
