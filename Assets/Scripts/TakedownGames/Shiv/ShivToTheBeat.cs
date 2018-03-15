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

	private float hitRelTimer;

	public List<GameObject> startingToHitAreas;

	private Queue<GameObject> dropping = new Queue<GameObject> ();
	private Queue<GameObject> toDrop = new Queue<GameObject> ();

	private GameObject toHitStartBox;

	private float minSepTime = 1f;

	private int toHitOnScreenCap = 10;
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
		
		//InitConditions();
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
				
				foreach (GameObject hb in allHitboxes) {
					if (dropping.Count > 0) {
						hb.GetComponent<Image> ().sprite = dropping.Peek ().GetComponent<Image> ().sprite;
					}
				}

				if (Input.GetKeyDown (KeyCode.Alpha0)) {
					done = !CheckHit ("0");
				} 
				if (Input.GetKeyDown (KeyCode.Alpha1)) {
					done = !CheckHit ("1");
				} 
				if (Input.GetKeyDown (KeyCode.Alpha3)) {
					done = !CheckHit ("3");
				} 
				if (Input.GetKeyDown (KeyCode.Alpha4)) {
					done = !CheckHit ("4");
				} 
				if (Input.GetKeyDown (KeyCode.Alpha5)) {
					done = !CheckHit ("5");
				} 
				if (Input.GetKeyDown (KeyCode.Alpha6)) {
					done = !CheckHit ("6");
				} 
				if (Input.GetKeyDown (KeyCode.Alpha7)) {
					done = !CheckHit ("7");
				} 
				if (Input.GetKeyDown (KeyCode.Alpha8)) {
					done = !CheckHit ("8");
				} 
				if (Input.GetKeyDown (KeyCode.Alpha9)) {
					done = !CheckHit ("9");
				} 
				if (Input.GetKeyDown (KeyCode.LeftBracket)) {
					done = !CheckHit ("[");
				} 
				if (Input.GetKeyDown (KeyCode.DownArrow)) {
					done = !CheckHit ("down arrow");
				} 
				if (Input.GetKeyDown (KeyCode.Return)) {
					done = !CheckHit ("enter");
				} 
				if (Input.GetKeyDown (KeyCode.LeftArrow)) {
					done = !CheckHit ("left arrow");
				} 
				if (Input.GetKeyDown (KeyCode.RightArrow)) {
					done = !CheckHit ("right arrow");
				} 
				if (Input.GetKeyDown (KeyCode.LeftShift) || Input.GetKeyDown (KeyCode.RightShift)) {
					done = !CheckHit ("shift");
				} 
				if (Input.GetKeyDown (KeyCode.Space)) {
					done = !CheckHit ("space");
				}

				if (dropping.Count > 0) {
					GameObject first = dropping.Peek ();

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
		} else if(weight <= 6){
			gameDiff = DIFFICULTY.MEDIUM;
		} else if(weight <= 8){
			gameDiff = DIFFICULTY.HARD;
		} else{
			gameDiff = DIFFICULTY.MAKE_IT_STOP;
		}
		gameStyle = (STYLE)Random.Range (0, (int)STYLE.num_opts);
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
			int spriteInd = Random.Range (0, possibleKeys.Count);
			int hitRange = gameDiff != DIFFICULTY.MAKE_IT_STOP ? possibleKeys.Count - 1 : possibleKeys.Count;
			toHit.GetComponent<Image> ().sprite = possibleKeys [spriteInd % (hitRange)];
			int hitboxInd = Random.Range (0, allHitboxes.Count);
			toHit.GetComponent<ToHitScript>().SetStartPos(new Vector2(allHitboxes[hitboxInd].GetComponent<RectTransform>().anchoredPosition.x, 0), OffScreen);
			if (gameStyle == STYLE.RAIN) {
				toHit.GetComponent<ToHitScript> ().velocity = new Vector2 (0, -3);
			}
			toDrop.Enqueue (toHit);
		}
	}

	private bool CheckHit(string keyHit){
		bool hitOK = false;
		GameObject first = dropping.Peek();
		//print (first.GetComponent<Image> ().sprite.name == keyHit);
		print (first.GetComponent<ToHitScript>().HitSuccess());
		if (first.GetComponent<Image> ().sprite.name == keyHit && first.GetComponent<ToHitScript>().HitSuccess()) {
			hitOK = true;
			Destroy (dropping.Dequeue ());
		}
		return succeeded &= hitOK;
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
