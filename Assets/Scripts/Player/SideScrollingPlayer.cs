using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SideScrollingPlayer : Player {

    public float speed = 3.0f;
    public float jump = 5.0f;
    private bool hasControl = true;
	private bool inVent = false;
	private bool dialogueAvail = false;
    private int foodCollected = 0;
	private Dialogueable activeDialogue;
	public RoomManager roomManager;
    private PickupUIBar pickupUIBar;
	private bool saveable = false;
	private float pickupGap = 0.0f;

    // Use this for initialization
    new void Start () {
        base.Start();
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Default"), LayerMask.NameToLayer ("VentLayer"), true);
        pickupUIBar = FindObjectOfType<PickupUIBar>();
    }
	
	// Update is called once per frame
	new void Update () {
		base.Update ();
		RaycastHit2D left = Physics2D.Raycast (new Vector3(transform.position.x - this.GetComponent<BoxCollider2D> ().bounds.extents.x - 0.01f, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f);
		RaycastHit2D middle = Physics2D.Raycast (new Vector3(transform.position.x, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f);
		RaycastHit2D right = Physics2D.Raycast (new Vector3(transform.position.x + this.GetComponent<BoxCollider2D> ().bounds.extents.x + 0.01f, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f);
		bool grounded = (left && left.collider) || (middle && middle.collider) || (right && right.collider);
		if (hasControl && grounded) {
			float jump_speed = 0f;
			if (Input.GetKeyDown (KeyCode.Space)) {
				jump_speed = jump + (0.1f * rb.velocity.x);
				rb.velocity = new Vector2 (rb.velocity.x, jump_speed);
			} else if (Input.GetKey (KeyCode.D) && rb.velocity.x < speed) {
				rb.velocity = new Vector2 (speed, rb.velocity.y);
			} else if (Input.GetKey (KeyCode.A) && rb.velocity.x > -speed) {
				rb.velocity = new Vector2 (-speed, rb.velocity.y);
			} else if (dialogueAvail && Input.GetKey (KeyCode.F)) {
				dialogueAvail = false;
				activeDialogue.BeginDialogue ();
			} else if (Input.GetKeyUp (KeyCode.Q)) {
				hasControl = false;
				roomManager.RevealSniffablesInCurRoom ();
			} else if (Input.GetKeyUp (KeyCode.E)) {
				Bark ();
			} else if (Input.GetKeyUp (KeyCode.X) && saveable) {
				StateSaver.Save ();
			}
		} else if (hasControl && !grounded) {
			if (Input.GetKey (KeyCode.D) && rb.velocity.x < speed * 0.5f) {
				rb.velocity = new Vector2 (speed * 0.5f, rb.velocity.y);
			} else if (Input.GetKey (KeyCode.A) && rb.velocity.x > -speed * 0.5f) {
				rb.velocity = new Vector2 (-speed * 0.5f, rb.velocity.y);
			}
		} else if (hasControl && !grounded) {
			if (Input.GetKey (KeyCode.D) && rb.velocity.x < speed * 0.5f) {
				rb.velocity = new Vector2 (speed * 0.5f, rb.velocity.y);
			} else if (Input.GetKey (KeyCode.A) && rb.velocity.x > -speed * 0.5f) {
				rb.velocity = new Vector2 (-speed * 0.5f, rb.velocity.y);
			}
		} else if (hasControl && !grounded) {
			if (Input.GetKey (KeyCode.D) && rb.velocity.x < speed * 0.5f) {
				rb.velocity = new Vector2 (speed * 0.5f, rb.velocity.y);
			} else if (Input.GetKey (KeyCode.A) && rb.velocity.x > -speed * 0.5f) {
				rb.velocity = new Vector2 (-speed * 0.5f, rb.velocity.y);
			}
		} else if (hasControl && !grounded) {
			if (Input.GetKey (KeyCode.D) && rb.velocity.x < speed * 0.5f) {
				rb.velocity = new Vector2 (speed * 0.5f, rb.velocity.y);
			} else if (Input.GetKey (KeyCode.A) && rb.velocity.x > -speed * 0.5f) {
				rb.velocity = new Vector2 (-speed * 0.5f, rb.velocity.y);
			}
		}

        if (rb.velocity.x >= 0) {
            this.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
        } else if (rb.velocity.x < 0)
        {
            this.transform.localScale = new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
        }
	}

    public int GetFoodFound()
    {
        return foodCollected;
    }

	public void foundFood(){
		if (Time.time - pickupGap > 1.0f) {
			pickupGap = Time.time;
			foodCollected += 1;
		}
	}

    // Walk between doors
    public void WalkBetweenRooms(Door door)
    {
        // if the player doesn't have control, they're probably already moving between rooms
        if (hasControl)
        {
            StartCoroutine("WalkBetweenRoomsCoroutine", door);
        }
    }

	public void Bark(){
		Vector2 myPos = new Vector2 (this.gameObject.transform.position.x, this.gameObject.transform.position.y);
		ContactFilter2D filter = new ContactFilter2D ();
		LayerMask layermask = 10;
		//Debug.Log (LayerMask.LayerToName (layermask));
		filter.SetLayerMask (layermask);
		Collider2D[] results = Physics2D.OverlapCircleAll (myPos, 1000.0f);
		int numEnemies = results.Length;
		//print (numEnemies);
		for (int i = 0; i < numEnemies; i++) {
			WalkingDrone drone = results [i].gameObject.GetComponent<WalkingDrone>();
			if (drone != null){
				drone.ReactToBark (this.gameObject.transform.position);
			}
		}
	}

    // Fades in the blackout object, waits a bit, then moves the player to the new room
    IEnumerator WalkBetweenRoomsCoroutine(Door door)
    {
        // Remove player control
        hasControl = false;
        roomManager.roomTransition = true;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        
        // Assign new current room
        RoomManager rm = FindObjectOfType<RoomManager>();
		Room newRoom = door.GetDestinationDoor().GetMyRoom();
		rm.SetCurrentRoom(newRoom);
        // Drop the blackout object over the camera
        yield return StartCoroutine(blackout.FadeInBlack());

        // Set the camera bounds to the new room
        newRoom.SetLimits();

        // Set the position to move towards (Note that we use the player's z location)
        Vector3 goToPosition = new Vector3(door.GetDestinationDoor().GetMyDestination().position.x, 
			this.gameObject.transform.position.y, this.gameObject.transform.position.z); 

        // Move player to new room
        while (Vector3.Distance(this.gameObject.transform.position, goToPosition) > 0.01f)
        {
			this.gameObject.transform.position = goToPosition;
            yield return null;
        }

		rb.velocity = new Vector2 (0, 0);
        // Wait half a second for dramatic flair
        yield return new WaitForSeconds(0.5f);

        // Pull the blackout object off the camera
        yield return StartCoroutine(blackout.FadeOutBlack());

        // Now that the old room is offscree we can clean it up
        rm.CleanUpRooms();

        // Reenable Control
        hasControl = true;
        roomManager.roomTransition = false;
    }

	public void EnterVent(Vent vent)
	{
		if (hasControl)
		{
			RoomManager rm = FindObjectOfType<RoomManager>();
			Room curRoom = rm.currentRoom;
			foreach (RoomObject r in curRoom.roomObjects) 
			{
				if (r.gameObject.GetComponentInChildren<SpriteRenderer> () != null && r.gameObject.GetComponent<Vent>() == null)
				{
					r.gameObject.GetComponentInChildren<SpriteRenderer> ().color = new Color (.1f, .1f, .1f, 1f);
				}
				else if(r.gameObject.GetComponent<SpriteRenderer> () != null && r.gameObject.GetComponent<Vent>() == null)
				{
					r.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
				}
			}

			//this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, .5f);
			this.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			this.gameObject.transform.position = vent.gameObject.transform.position;
			this.gameObject.layer = LayerMask.NameToLayer ("VentLayer");
		}
	}

	public void ExitVent(Vent vent)
	{
		if (hasControl)
		{
			RoomManager rm = FindObjectOfType<RoomManager>();
			Room curRoom = rm.currentRoom;
			foreach (RoomObject r in curRoom.roomObjects) 
			{
				if (r.gameObject.GetComponentInChildren<SpriteRenderer> () != null && r.gameObject.GetComponent<Vent>() == null)
				{
					r.gameObject.GetComponentInChildren<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
				}
				else if(r.gameObject.GetComponent<SpriteRenderer> () != null && r.gameObject.GetComponent<Vent>() == null)
				{
					r.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 1f);
				}
			}

			this.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			this.gameObject.transform.position = vent.gameObject.transform.position;
			this.gameObject.layer = LayerMask.NameToLayer ("Default");
		}
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Dialogueable> () != null) {
			dialogueAvail = true;
			activeDialogue = other.gameObject.GetComponent<Dialogueable> ();
			activeDialogue.FToInteract (true);
		} else if (other.GetComponent<Door> () != null) {
			WalkBetweenRooms (other.GetComponent<Door> ());
		} else if (other.GetComponent<PickupItem> () != null) {
			if(other.GetComponent<Food>()){
				foundFood();
			}
			pickupUIBar.AddItem (other.GetComponent<PickupItem> ());
			Destroy (other.gameObject);
		} else if (other.GetComponent<Vent> () != null) {
			if (inVent) {
				inVent = false;
				ExitVent (other.GetComponent<Vent> ());
			} else {
				inVent = true;
				EnterVent (other.GetComponent<Vent> ());
			}
		} else if (other.GetComponent<Hunter> () != null && !inVent) {
			foodCollected = 0;
			StartCoroutine (blackout.FadeInBlack ());
			SceneManager.LoadSceneAsync ("OverworldExampleScene"); // Change this later to a scene with an animation when we have animations
		} else if (other.GetComponent<SavePoint> () != null) {
			other.GetComponent<SavePoint> ().revSave ();
			saveable = true;
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.GetComponent<Dialogueable> () != null) {
			dialogueAvail = false;
			activeDialogue.FToInteract (false);
		} else if (other.GetComponent<SavePoint> () != null) {
			other.GetComponent<SavePoint> ().hideSave ();
			saveable = false;
		}
	}

	public void RevealDone(){
		hasControl = true;
	}
}
