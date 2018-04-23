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
	private float pickupGap = 0.0f;
	private bool paused = false;

	public RuntimeAnimatorController idleAnim;
	public Sprite idleSprite;
	public Animation forwardAnim;
	public Animation jumpAnim;

	private AbilityUIControl abilityCont;

	private AudioSource mySound;
	public AudioClip barkSound;
	public AudioClip pantSound;

    // Use this for initialization
    new void Start () {
        base.Start();
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Default"), LayerMask.NameToLayer ("VentLayer"), true);
        pickupUIBar = FindObjectOfType<PickupUIBar>();
		abilityCont = ((GameObject)Instantiate (Resources.Load ("Prefabs/AbilityUI"))).GetComponent<AbilityUIControl>();
		roomManager = FindObjectOfType<RoomManager>();
		mySound = this.GetComponent<AudioSource> ();
    }
	
	// Update is called once per frame
	new void Update () {
		base.Update ();
		if (!paused) {
			checkOutOfBounds ();
		}
		if (!mySound.isPlaying) {
			mySound.clip = this.pantSound;
			mySound.loop = true;
			mySound.Play ();
		}
		LayerMask ignore = ~(1 << LayerMask.NameToLayer ("Detection"));
		RaycastHit2D left = Physics2D.Raycast (new Vector3(transform.position.x - this.GetComponent<BoxCollider2D> ().bounds.extents.x - 0.01f, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f, ignore);
		RaycastHit2D middle = Physics2D.Raycast (new Vector3(transform.position.x, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f, ignore);
		RaycastHit2D right = Physics2D.Raycast (new Vector3(transform.position.x + this.GetComponent<BoxCollider2D> ().bounds.extents.x + 0.01f, transform.position.y - this.GetComponent<BoxCollider2D> ().bounds.extents.y - 0.01f, transform.position.z), Vector2.down, 0.1f, ignore);

		Vector2 velToAdd = Vector2.zero;
		if (left.rigidbody != null && left.rigidbody.isKinematic && !left.collider.isTrigger) {
			velToAdd = left.rigidbody.velocity;
		} else if (middle.rigidbody != null && middle.rigidbody.isKinematic && !middle.collider.isTrigger) {
			velToAdd = middle.rigidbody.velocity;
		} else if (right.rigidbody != null && right.rigidbody.isKinematic && !right.collider.isTrigger) {
			velToAdd = right.rigidbody.velocity;
		}


		bool grounded = (left && left.collider && !left.collider.isTrigger) || (middle && middle.collider && !middle.collider.isTrigger) || (right && right.collider && !right.collider.isTrigger);
		if (paused || !hasControl) {
			rb.velocity = Vector2.zero + velToAdd;
		} else {
			float jump_speed = 0f;
			rb.velocity = new Vector2(0.0f, rb.velocity.y) + velToAdd;
			if ((Input.GetKeyDown (KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && grounded) {
				jump_speed = jump + (0.1f * rb.velocity.x);
				rb.velocity = velToAdd + new Vector2 (rb.velocity.x, jump_speed);
			} else if ((Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && rb.velocity.x < speed) {
				rb.velocity = velToAdd + new Vector2 (speed, rb.velocity.y);
			} else if ((Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && rb.velocity.x > -speed) {
				rb.velocity = velToAdd + new Vector2 (-speed, rb.velocity.y);
			}

			if (dialogueAvail && Input.GetKey (KeyCode.F)) {
				dialogueAvail = false;
				activeDialogue.BeginDialogue ();
			} else if (Input.GetKeyUp (KeyCode.E) && abilityCont.SniffAvailable ()) {
				hasControl = false;
				roomManager.RevealSniffablesInCurRoom ();
			} else if (Input.GetKeyUp (KeyCode.Q) && abilityCont.BarkAvailable ()) {
				if (!mySound.isPlaying || mySound.clip.Equals(pantSound)) {
					mySound.clip = barkSound;
					mySound.loop = false;
					mySound.Play ();
				}
				Bark ();
			} else if (Input.GetKeyUp (KeyCode.R) && abilityCont.SaveAvailable ()) {
				StateSaver.Save ();
			} else if (Input.GetKeyUp (KeyCode.T)) {
				RaycastHit2D botInFront = Physics2D.Raycast (new Vector3(transform.position.x + (this.transform.localScale.x * (this.GetComponent<BoxCollider2D>().bounds.extents.x + 0.01f)), transform.position.y, transform.position.z), new Vector2(this.transform.localScale.x, 0), 1.0f);
				if (botInFront.collider != null && botInFront.transform.GetComponent<Drone> () != null && abilityCont.TakedownAvailable ()) {
					StartCoroutine ("PreformTakedown", botInFront.transform.GetComponent<Drone>());
				}
			} else if (Input.GetKeyUp (KeyCode.I)) {
				abilityCont.ToggleInfo ();
			}
		}


        if (rb.velocity.x < 0) {
            this.transform.localScale = new Vector3(-1, this.transform.localScale.y, this.transform.localScale.z);
        } else if (rb.velocity.x > 0)
        {
            this.transform.localScale = new Vector3(1, this.transform.localScale.y, this.transform.localScale.z);
        }
	}

	void FixedUpdate(){
		float dist = new Vector2(rb.velocity.x, 0).magnitude * Time.fixedDeltaTime;
		RaycastHit2D[] inside = Physics2D.BoxCastAll (((Vector2)this.transform.position), new Vector2(1, 1), 0, (Vector2)rb.velocity, dist);
		bool willCollide = false;
		foreach (RaycastHit2D hit in inside) {
			willCollide = willCollide || !hit.collider.isTrigger;
			if (willCollide) {
				break;
			}
		}
		if(willCollide){
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}
	}

	private void checkOutOfBounds(){
		Vector2 minPt = roomManager.currentRoom.roomSizeMin;
		Vector2 maxPt = roomManager.currentRoom.roomSizeMax;
		Vector3 myPos = this.transform.position;
		if (myPos.x <= (minPt.x - 5) || myPos.x >= (maxPt.x + 5) || myPos.y <= (minPt.y - 5) || myPos.y >= (maxPt.y + 5)) {
			foodCollected = 0;
			StartCoroutine (blackout.FadeInBlack ());
			SceneManager.LoadSceneAsync ("OverworldExampleScene");
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
		Collider2D[] results = Physics2D.OverlapCircleAll (myPos, 1000.0f, 1 << LayerMask.NameToLayer("Enemies"));
		int numEnemies = results.Length;
		for (int i = 0; i < numEnemies; i++) {
			WalkingDrone drone = results [i].gameObject.GetComponent<WalkingDrone>();
			Hunter hunt = results [i].gameObject.GetComponent<Hunter>();
			if (drone != null) {
				drone.ReactToBark (this.gameObject.transform.position);
			} 
			if (hunt != null) {
				hunt.ReactToBark (this.gameObject.transform.position);
			}
		}
	}

	public void ElevatorRide(Elevator target){
		if (hasControl) {
			StartCoroutine ("ElevatorCoroutine", target);
		}
	}

    // Fades in the blackout object, waits a bit, then moves the player to the new room
    IEnumerator WalkBetweenRoomsCoroutine(Door door)
    {
        // Remove player control
		roomManager = FindObjectOfType<RoomManager>();
		roomManager.currentRoom.TogHuntPause (true);
		Room newRoom = door.GetDestinationDoor().GetMyRoom();
		roomManager.SetCurrentRoom(newRoom);
        hasControl = false;
		paused = true;
		roomManager.roomTransition = true;
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        
        // Assign new current room
        // Drop the blackout object over the camera
        yield return StartCoroutine(blackout.FadeInBlack());

        // Set the camera bounds to the new room
        newRoom.SetLimits();

        // Set the position to move towards (Note that we use the player's z location)
        Vector3 goToPosition = new Vector3(door.GetDestinationDoor().GetMyDestination().position.x, 
			door.GetDestinationDoor().GetMyDestination().position.y, this.gameObject.transform.position.z); 

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
		roomManager.CleanUpRooms();

        // Reenable Control

        hasControl = true;
		paused = false;
		roomManager.currentRoom.TogHuntPause (false);
		roomManager.roomTransition = false;
    }

	IEnumerator ElevatorCoroutine(Elevator target){
		// Remove player control
		roomManager = FindObjectOfType<RoomManager>();
		Room newRoom = target.GetMyRoom();
		roomManager.SetCurrentRoom(newRoom);
		hasControl = false;
		roomManager.roomTransition = true;
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);

		// Assign new current room
		// Drop the blackout object over the camera
		yield return StartCoroutine(blackout.FadeInBlack());

		// Set the camera bounds to the new room
		newRoom.SetLimits();

		// Set the position to move towards (Note that we use the player's z location)
		Vector3 goToPosition = new Vector3(target.GetMyDestination().position.x, 
			target.GetMyDestination().position.y, this.gameObject.transform.position.z); 

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
		roomManager.CleanUpRooms();

		// Reenable Control

		hasControl = true;
		roomManager.roomTransition = false;
	}

	public void EnterVent(Vent vent)
	{
		if (hasControl)
		{
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
			StartCoroutine ("HunterAttack", other.GetComponent<Hunter> ());
		} else if (other.GetComponent<SavePoint> () != null) {
			other.GetComponent<SavePoint> ().revSave ();
			abilityCont.SaveZone (true);
		}
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (other.GetComponent<Dialogueable> () != null) {
			dialogueAvail = false;
			activeDialogue.FToInteract (false);
		} else if (other.GetComponent<SavePoint> () != null) {
			other.GetComponent<SavePoint> ().hideSave ();
			abilityCont.SaveZone (false);
		}
	}

	public void RevealDone(){
		hasControl = true;
	}

	IEnumerator PreformTakedown(Drone target){
		ToggleAbilityControl ();
		StartCoroutine (blackout.FadeInBlack ());
		bool succeeded = false;
		StopTime ();
		TakedownGame minigame = TakedownGame.GetRandGame();
		if (minigame == null) {
			print ("No Game Received!");
			yield break;
		}
		minigame.drone = true;
		minigame.BeginGame();
		while (!minigame.CheckFinished(ref succeeded)) {
			yield return new WaitForSecondsRealtime (1);
		}
		if (succeeded) {
			Destroy (target.gameObject);
		}
		Destroy (minigame);
		StartTime ();
		ToggleAbilityControl ();
		StartCoroutine (blackout.FadeOutBlack ());
	}

	IEnumerator HunterAttack(Hunter target){
		ToggleAbilityControl ();
		StartCoroutine (blackout.FadeInBlack ());
		bool succeeded = false;
		StopTime ();
		TakedownGame minigame = TakedownGame.GetRandGame();
		if (minigame == null) {
			print ("No Game Received!");
			yield break;
		}
		minigame.drone = false;
		minigame.BeginGame();
		while (!minigame.CheckFinished(ref succeeded)) {
			yield return new WaitForSecondsRealtime (1);
		}
		Destroy (minigame);
		StartTime ();
		ToggleAbilityControl ();
		if (succeeded) {
			Destroy (target.gameObject);
			StartCoroutine (blackout.FadeOutBlack ());
		} else {
			foodCollected = 0;
			StartCoroutine (blackout.FadeInBlack ());
			SceneManager.LoadSceneAsync (StateSaver.gameState.curArea.name);
		}
	}

	public void StopTime(){
		StateSaver.gameState.paused = true;
		hasControl = false;
	}

	public void StartTime(){
		StateSaver.gameState.paused = false;
		hasControl = true;
	}

	public void ToggleAbilityControl(){
		abilityCont.gameObject.SetActive (!abilityCont.gameObject.activeSelf);
	}


}
