using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShivToTheBeat : TakedownGame {

	public List<Sprite> possibleKeys;

	private DIFFICULTY gameDiff;
	private STYLE gameStyle;

	private bool playing = false;

	private List<GameObject> allHitboxes = new List<GameObject>();

	public Sprite[] constantHitboxes = new Sprite[3];

	private float hitRelTimer;

	public List<GameObject> startingToHitAreas;

	private Queue<GameObject> dropping = new Queue<GameObject> ();
	private Queue<GameObject> toDrop = new Queue<GameObject> ();

	private GameObject toHitStartBox;
	private GameObject hitBoxZone;
	private GameObject fightSim;
	private Image playerBar;
	private Image droneBar;
	private Image hunterBar;

	private float minSepTime;

	private int toHitOnScreenCap = 10;

	private Vector2 velocity = Vector2.zero;
	private float maxFails = 5;
	private float failsRemaining = 5;
	private float numObj = 0;
	private float numObjHit = 0;
	public GameObject toHitPrefab;

	private enum DIFFICULTY
	{
		EASY = 0,
		MEDIUM,
		HARD,
		MAKE_IT_STOP,
		num_opts
	};

	private enum STYLE
	{
		RAIN = 0,
		SPIRAL,
		SIDES,
		num_opts

	};

	// Use this for initialization
	new void Start () {
		
		InitConditions();
		fightSim = (GameObject)Instantiate (Resources.Load ("Prefabs/Games/Shiv/FightSimulator"));
		fightSim.transform.SetParent (this.gameObject.transform);
		fightSim.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);
		playerBar = fightSim.transform.Find ("Dog").transform.Find ("DogBar").GetComponent<Image>();
		droneBar = fightSim.transform.Find ("Drone").transform.Find ("DroneBar").GetComponent<Image>();
		hunterBar = fightSim.transform.Find ("Hunter").transform.Find ("HunterBar").GetComponent<Image>();
		droneBar.transform.parent.gameObject.SetActive (drone);
		hunterBar.transform.parent.gameObject.SetActive (!drone);
		InitHitboxes();
		InitPool ();
		hitRelTimer = Time.time;
		succeeded = true;
	}
	
	// Update is called once per frame
	new void Update () {

		if (playing && !done) {
			if (toDrop.Count > 0 || dropping.Count > 0) {
				float curTime = Time.time;
				if (curTime - hitRelTimer >= minSepTime) {
					if (toDrop.Count > 0 && dropping.Count < toHitOnScreenCap) {
						GameObject drop = toDrop.Dequeue ();
						dropping.Enqueue (drop);
						drop.GetComponent<ToHitScript> ().Drop ();
						hitRelTimer = Time.time;
						minSepTime = Random.Range (1.0f, 2.0f);
					}
				}

				if (Input.GetKeyDown (KeyCode.Alpha0)) {
					if (!CheckHit ("zero")) {
						failsRemaining--;
					}
				}
				if (Input.GetKeyDown (KeyCode.A)) {
					if (!CheckHit ("Aarrow")) {
						failsRemaining--;
					}
				}
				if (Input.GetKeyDown (KeyCode.Alpha2)) {
					if (!CheckHit ("two")) {
						failsRemaining--;
					}
				}
				if (Input.GetKeyDown (KeyCode.D)) {
					if (!CheckHit ("Darrow")) {
						failsRemaining--;
					}
				}
				if (Input.GetKeyDown (KeyCode.S)) {
					if (!CheckHit ("Sarrow")) {
						failsRemaining--;
					}
				}
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					if (!CheckHit ("one")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha3)) {
					if (!CheckHit ("three")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha4)) {
					if (!CheckHit ("four")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha5)) {
					if (!CheckHit ("five")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha6)) {
					if (!CheckHit ("six")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha7)) {
					if (!CheckHit ("seven")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha8)) {
					if (!CheckHit ("eight")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha9)) {
					if (!CheckHit ("nine")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.LeftBracket)) {
					if (!CheckHit ("[")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.DownArrow)) {
					if (!CheckHit ("downarrow")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Return)) {
					if (!CheckHit ("enter")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					if (!CheckHit ("leftarrow")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					if (!CheckHit ("rightarrow")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
					if (!CheckHit ("shift")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Space)) {
					if (!CheckHit ("space")) {
						failsRemaining--;
					}
				}
				if (drone) {
					droneBar.fillAmount = numObjHit / numObj;
				} else {
					hunterBar.fillAmount = (numObj - numObjHit) / numObj;
				}
				playerBar.fillAmount = failsRemaining / maxFails;

				if (failsRemaining < 0) {
					succeeded = false;
					done = true;
				}

			} else {
				done = true;
			}

		}
	}

	protected override void RunGame ()
	{
		playing = true;
	}

	private void InitConditions(){
		int weight = Random.Range (0, 10);
		if (weight <= 3) {
			gameDiff = DIFFICULTY.EASY;
			velocity = new Vector2 (0.0f, -5.0f);
			minSepTime = 0.9f;
		} else if(weight <= 6){
			gameDiff = DIFFICULTY.MEDIUM;
			velocity = new Vector2 (0.0f, -7.0f);
			minSepTime = 0.6f;
		} else if(weight <= 8){
			gameDiff = DIFFICULTY.HARD;
			velocity = new Vector2 (0.0f, -10.0f);
			minSepTime = 0.3f;
		} else{
			gameDiff = DIFFICULTY.MAKE_IT_STOP;
			velocity = new Vector2 (0.0f, -15.0f);
			minSepTime = 0.1f;
		}
		gameStyle = STYLE.RAIN;
	}

	private void InitHitboxes(){
		switch (gameStyle) {
		case STYLE.RAIN:
			hitBoxZone = (GameObject)Instantiate (Resources.Load ("Prefabs/Games/Shiv/RainHitboxes"));
			hitBoxZone.transform.SetParent (this.gameObject.transform);
			hitBoxZone.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 50);
			toHitStartBox = (GameObject)Instantiate (startingToHitAreas [0]);
			toHitStartBox.transform.SetParent (this.gameObject.transform);
			toHitStartBox.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 50);
			toHitStartBox.GetComponent<RectTransform> ().sizeDelta = new Vector2(canvas.GetComponent<RectTransform>().rect.width, 100);
			break;
		case STYLE.SIDES:

			break;

		case STYLE.SPIRAL:

			break;
		}
		foreach (Transform hb in hitBoxZone.transform) {
			allHitboxes.Add (hb.gameObject);
		}
	}

	private void InitPool(){
		numObj = ((gameDiff == DIFFICULTY.MAKE_IT_STOP) ? 50 : (((int)gameDiff + 1) * 10)); 
		for (int i = 0; i < numObj; i++) {
			GameObject toHit = (GameObject)Instantiate (toHitPrefab);
			toHit.transform.SetParent(toHitStartBox.transform);
			int spriteInd = Random.Range (0, constantHitboxes.Length);
			int hitboxInd = spriteInd;
			int randChance = Random.Range (1, 101);
			if (randChance > 95) {
				spriteInd = Random.Range (0, possibleKeys.Count);
				toHit.GetComponent<Image> ().sprite = possibleKeys [spriteInd % (possibleKeys.Count)];
			} else {
				toHit.GetComponent<Image> ().sprite = constantHitboxes [spriteInd % (constantHitboxes.Length)];
			}
			toHit.GetComponent<ToHitScript>().SetStartPos(new Vector2(allHitboxes[hitboxInd].GetComponent<RectTransform>().anchoredPosition.x, 0), OffScreen);
			toHit.GetComponent<ToHitScript> ().velocity = velocity;
			toDrop.Enqueue (toHit);
		}
	}

	private bool CheckHit(string keyHit){
		bool hitOK = false;
		GameObject first = dropping.Peek();

		if (first.GetComponent<Image> ().sprite.name == keyHit && first.GetComponent<ToHitScript>().HitSuccess()) {
			hitOK = true;

			GameObject toDest = dropping.Dequeue ();
			toDest.GetComponent<ToHitScript> ().flagForDest = true;
			numObjHit++;
			Destroy (toDest);
		}
		return hitOK;
	}

	public void OffScreen(){
		failsRemaining--;
		GameObject toDest = dropping.Dequeue ();
		Destroy (toDest);
	}

	void OnDestroy(){
		int dropCnt = dropping.Count;
		for (int i = 0; i < dropCnt; i++) {
			Destroy (dropping.Dequeue ());
		}
		int toDropCnt = toDrop.Count;
		for (int i = 0; i < toDropCnt; i++) {
			Destroy (toDrop.Dequeue ());
		}
		for (int i = 0; i < allHitboxes.Count; i++) {
			Destroy (allHitboxes [i]);
		}
		Destroy (fightSim);
		Destroy (hitBoxZone);
		Destroy(toHitStartBox);
		Destroy (this.gameObject);
	}
}
