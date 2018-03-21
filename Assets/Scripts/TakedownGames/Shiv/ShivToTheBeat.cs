using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShivToTheBeat : TakedownGame {

	public List<Sprite> possibleKeys;

	private DIFFICULTY gameDiff = DIFFICULTY.EASY;
	private STYLE gameStyle = STYLE.RAIN;

	private bool playing = false;

	private List<GameObject> allHitboxes = new List<GameObject>();

	public Sprite[] constantHitboxes = new Sprite[3];

	private float hitRelTimer;

	public List<GameObject> startingToHitAreas;

	private Queue<GameObject> dropping = new Queue<GameObject> ();
	private Queue<GameObject> toDrop = new Queue<GameObject> ();

	private GameObject toHitStartBox;

	private float minSepTime;

	private int toHitOnScreenCap = 10;

	private Vector2 velocity = Vector2.zero;

	private int failsRemaining = 5;
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
					if (!CheckHit ("0")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					if (!CheckHit ("1")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha3)) {
					if (!CheckHit ("3")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha4)) {
					if (!CheckHit ("4")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha5)) {
					if (!CheckHit ("5")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha6)) {
					if (!CheckHit ("6")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha7)) {
					if (!CheckHit ("7")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha8)) {
					if (!CheckHit ("8")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Alpha9)) {
					if (!CheckHit ("9")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.LeftBracket)) {
					if (!CheckHit ("[")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.DownArrow)) {
					if (!CheckHit ("down arrow")) {
						print ("hit");
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.Return)) {
					if (!CheckHit ("enter")) {
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					if (!CheckHit ("left arrow")) {
						print ("hit");
						failsRemaining--;
					}
				} 
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					if (!CheckHit ("right arrow")) {
						print ("hit");
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
				print (failsRemaining);

				if (failsRemaining < 0) {
					print ("failed");
					succeeded = false;
					done = true;
				}

			} else {
				print ("hmmmmm");
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
			velocity = new Vector2 (0.0f, -3.0f);
			minSepTime = 0.9f;
		} else if(weight <= 6){
			gameDiff = DIFFICULTY.MEDIUM;
			velocity = new Vector2 (0.0f, -4.0f);
			minSepTime = 0.6f;
		} else if(weight <= 8){
			gameDiff = DIFFICULTY.HARD;
			velocity = new Vector2 (0.0f, -5.0f);
			minSepTime = 0.3f;
		} else{
			gameDiff = DIFFICULTY.MAKE_IT_STOP;
			velocity = new Vector2 (0.0f, -7.0f);
			minSepTime = 0.1f;
		}
		//gameStyle = (STYLE)Random.Range (0, (int)STYLE.num_opts);
	}

	private void InitHitboxes(){
		GameObject hitBoxZone = null;
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
		int numObj = ((gameDiff == DIFFICULTY.MAKE_IT_STOP) ? 50 : (((int)gameDiff + 1) * 10)); 
		for (int i = 0; i < numObj; i++) {
			GameObject toHit = (GameObject)Instantiate (toHitPrefab);
			toHit.transform.SetParent(toHitStartBox.transform);
			int spriteInd = Random.Range (0, constantHitboxes.Length);
			int hitboxInd = spriteInd;
			int randChance = Random.Range (1, 101);
			if (randChance == 100) {
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
			Destroy (dropping.Dequeue ());
		}
		return hitOK;
	}

	public void OffScreen(){
		succeeded = false;
		done = true;
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

		Destroy(toHitStartBox);
	}
}
