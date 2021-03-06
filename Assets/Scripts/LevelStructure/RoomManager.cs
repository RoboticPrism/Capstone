﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Manages room loading/cleanup
public class RoomManager : MonoBehaviour {

    // The player instance
    public GameObject player;

    // All the room prefabs go here
    public List<Room> allRooms;

    // The room instance the player is currently in
    public Room currentRoom;

    // The rooms adjacent to the current room that should be loaded
    public List<Room> adjacentRooms;
	public Camera mainCam;

	// Use this for initialization
    public bool roomTransition = false;

    public Hunter hunterPrefab;

    // Use this for initialization
    void Start ()
    {
		Room[] getRooms = Object.FindObjectsOfType<Room> ();

		foreach (Room r in getRooms) {
			r.hunterPrefab = this.hunterPrefab;
			allRooms.Add(r);
			if (r.spawnRoom) {
				SetCurrentRoom(r);
                // Set the camera limits to the spawn room
                r.SetLimits();
			}
		}
		/*
		// Find all the rooms in the game
		GameObject[] roomsInScene = GameObject.FindGameObjectsWithTag(Tags.ROOM);
		foreach (GameObject room in roomsInScene) {
			// Get the Room component from the object
			Room roomComponent = room.GetComponent<Room>();

			// Add the room to the list of all rooms, and check if it's the spawn room
			// If spawn room, make it our initial current room
			allRooms.Add(roomComponent);
			if (roomComponent.spawnRoom) {
				SetCurrentRoom(roomComponent);
			}
		}
		*/

		// Needs at least one room in the game
		if (allRooms.Count == 0) {
			Debug.LogError("PLEASE PROVIDE AT LEAST ONE ROOM");
			Application.Quit();
		}
			
		// Initialize the player's starting room
		// !IMPORTANT!
		// Can't use SetCurrentRoom() here because we have no adjacent rooms yet
		if (currentRoom == null) {
			Debug.LogError("NO STARTING ROOM GIVEN");
			Application.Quit();
		}
			
		// Clean and deactivate all other rooms
		CleanUpRooms();
	}


	
	// Update is called once per frame
	void Update ()
    {
		
	}

    // Sets a new current room, prepares adjacent room
    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
        
        // Do nothing if active
        if (room.GetState() == Room.RoomState.ACTIVE)
        {

        }
        // If prepared, show
        else if (room.GetState() == Room.RoomState.PREPARED)
        {
            room.ActivateRoom();
        }
        // If deactive, we need to prepare this room then show it
        else if (room.GetState() == Room.RoomState.DEACTIVE)
        {
            room.PrepareRoom();
            room.ActivateRoom();
        }

        // Set state to active    
		room.SetState(Room.RoomState.ACTIVE);

		// Get new adjacent rooms and prepare them
		SetAdjacentRooms (room);
    }

	// Prepare all rooms adjacent to the given room, and set them to the PREPARED state
	public void SetAdjacentRooms(Room room)
	{
		// Get all rooms adjacent to the given room
		adjacentRooms = room.GetAdjacentRooms();

		// Prepare all adjacent Rooms r, to the given room
		foreach (Room r in adjacentRooms)
		{
            // Prepare
            if (r.GetState() == Room.RoomState.DEACTIVE)
			{
				r.PrepareRoom();
			}

            // Set state to prepared
            r.SetState(Room.RoomState.PREPARED);
        }
	}

	public void RevealSniffablesInCurRoom(){
		List<Sniffable> sniffables = currentRoom.GetSniffables ();
		List<Vector3> points = new List<Vector3> ();
		foreach (Sniffable s in sniffables) {
			s.Sniffed ();
			if (!s.onscreen) {
				points.Add (s.gameObject.transform.position);
			}
		}
		CameraControl camCont = mainCam.GetComponent<CameraControl> ();
		if (camCont) {
			camCont.PanToSniffables (points);
		}
	}

    // Use after setting a new current room to clean up old rooms
    public void CleanUpRooms()
    {
        foreach (Room r in allRooms)
        {
            // Don't do anything for the current room
            if (r == currentRoom)
            {

            }
            // Ensure all adjacent rooms are deactivated
            else if (adjacentRooms.Contains(r))
            {
                r.DeactivateRoom();
            }
            // Clear all rooms that aren't current or adjacent
            else
            {
                // if this room isn't deactive, it should be, clear and deactivate it
                if (r.GetState() != Room.RoomState.DEACTIVE)
                {
                    r.ClearRoom();
                    r.DeactivateRoom();
                    r.SetState(Room.RoomState.DEACTIVE);
                }
            }
        }
    } 

//    public void SpawnHunter(Room currentRoom)
//    {
////        if (instantiatedHunter == null)
////        {
////            instantiatedHunter = Instantiate(hunterPrefab);
////            instantiatedHunter.transform.position = new Vector3(currentRoom.roomSizeMax.x, currentRoom.roomSizeMax.y, 0);
////			instantiatedHunter.transform.SetParent (currentRoom.transform);
////        }
//    }
}
