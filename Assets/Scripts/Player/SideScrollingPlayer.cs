using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SideScrollingPlayer : Player {

    public float speed = 3.0f;
    private bool hasControl = true;
	private Rigidbody2D rb;
	private bool inVent = false;
	private bool dialogueAvail = false;
	private Dialogueable activeDialogue;
	public RoomManager roomMan;
	// Use this for initialization
	new void Start () {
        base.Start();
		rb = this.GetComponent<Rigidbody2D>();
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Default"), LayerMask.NameToLayer ("VentLayer"), true);

	}
	
	// Update is called once per frame
	new void Update () {
		RaycastHit2D left = Physics2D.Raycast (new Vector3(transform.position.x - this.GetComponent<BoxCollider2D> ().bounds.extents.x - 0.01f, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.1f, transform.position.z), Vector2.down, 0.01f);
		RaycastHit2D middle = Physics2D.Raycast (new Vector3(transform.position.x, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.1f, transform.position.z), Vector2.down, 0.01f);
		RaycastHit2D right = Physics2D.Raycast (new Vector3(transform.position.x + this.GetComponent<BoxCollider2D> ().bounds.extents.x + 0.01f, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.1f, transform.position.z), Vector2.down, 0.01f);
		bool grounded = (left && left.collider) || (middle && middle.collider) || (right && right.collider);
		if (hasControl && grounded)
		{
			float jump_speed = 0f;
			if (Input.GetKeyDown (KeyCode.Space)) {
				jump_speed = 5f + (0.1f * rb.velocity.x);
				rb.velocity = new Vector2 (rb.velocity.x, jump_speed);
			} else if (Input.GetKey (KeyCode.D) && rb.velocity.x < speed) {
				rb.velocity = new Vector2 (rb.velocity.x + 1, rb.velocity.y);
			} else if (Input.GetKey (KeyCode.A) && rb.velocity.x > -speed) {
				rb.velocity = new Vector2 (rb.velocity.x - 1, rb.velocity.y);
			} else if (dialogueAvail && Input.GetKey (KeyCode.F)) {
				dialogueAvail = false;
				activeDialogue.BeginDialogue ();
			} else if (Input.GetKey (KeyCode.Q)) {
				roomMan.RevealSniffablesInCurRoom ();
			}
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

    

    // Fades in the blackout object, waits a bit, then moves the player to the new room
    IEnumerator WalkBetweenRoomsCoroutine(Door door)
    {
        // Remove player control
        hasControl = false;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        
        // Assign new current room
        RoomManager rm = FindObjectOfType<RoomManager>();
		Room newRoom = door.GetDestinationDoor().GetMyRoom();
		CameraControl cc = FindObjectOfType<CameraControl> ();
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
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position,
                                                                     goToPosition,
                                                                     speed/100.0f);
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
		if (other.GetComponent<Dialogueable> () != null)
		{
			dialogueAvail = true;
			activeDialogue = other.gameObject.GetComponent<Dialogueable>();
			activeDialogue.FToInteract (true);
		}
		else if (other.GetComponent<Door> () != null)
		{
			WalkBetweenRooms (other.GetComponent<Door> ());
		} 
		else if (other.GetComponent<Vent> () != null) 
		{
			if (inVent) 
			{
				inVent = false;
				ExitVent (other.GetComponent<Vent> ());
			}
			else
			{
				inVent = true;
				EnterVent (other.GetComponent<Vent> ());
			}
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.GetComponent<Dialogueable> () != null) {
			dialogueAvail = false;
			activeDialogue.FToInteract (false);
		}
	}

}
