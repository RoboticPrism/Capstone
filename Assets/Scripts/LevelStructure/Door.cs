using UnityEngine;
using System.Collections;

// Use for connections between rooms
public class Door : MonoBehaviour {

    // The room this door appears within
    public Room myRoom { get; set; }

    // The door this door leads to
    public Door destinationDoor;

    // The point in space the player ends up at after going through this door
    public Transform myDestination { get; set; }

	// Direction this door is going (Left or right value)
	// Left = -1.1, Right = 1.1
	public float doorDirection;

	// Use this for initialization
	void Start () {
        myDestination = transform.Find("Destination");
        myRoom = transform.parent.GetComponent<Room>();
        if (transform.position.x < destinationDoor.transform.position.x) {
            doorDirection = 1.1f;
        } else
        {
            doorDirection = -1.1f;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Transform GetMyDestination()
    {
        return myDestination;
    }

    public float GetDoorDirection()
    {
        return doorDirection;
    }

    // Get the door this door leads to
    public Door GetDestinationDoor()
    {
        return destinationDoor;
    }

    // Get the room this door is in
    public Room GetMyRoom()
    {
        return myRoom;
    }		
}
