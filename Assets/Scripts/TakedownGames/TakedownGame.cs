using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakedownGame : MonoBehaviour {
	protected bool done = false;
	protected bool succeeded = false;
	protected bool waitingForInput = false;
	protected static GameObject canvas = null;
	public bool drone = false;

	// Use this for initialization
	protected void Start () {
		
	}
	
	// Update is called once per frame
	protected void Update () {
		
	}

	public void BeginGame(){
		RunGame ();
	}

	public bool CheckFinished(ref bool result){
		result = succeeded;
		return done;
	}

	protected virtual void RunGame(){

	}

	public static TakedownGame GetRandGame(){
		TakedownGame TdownGame = null;
		canvas = GameObject.Find ("Canvas");
		int l = (int)StateSaver.Minigames.num_opts;
		int r = Random.Range (1, l);
		StateSaver.Minigames cur = (StateSaver.Minigames)r;
		switch (cur) {
		case StateSaver.Minigames.PatternSquares:
			TdownGame = ((GameObject)Instantiate (Resources.Load ("Prefabs/Games/PatternSquaresPrefab"))).GetComponent<PatternSquares> ();
			break;
		case StateSaver.Minigames.ShivToTheBeat:
			TdownGame = ((GameObject)Instantiate (Resources.Load ("Prefabs/Games/Shiv/ShivToTheBeat"))).GetComponent<ShivToTheBeat> ();
			break;
		}
		RectTransform canvasT = canvas.GetComponent<RectTransform> ();
		TdownGame.transform.SetParent (canvas.transform);
		TdownGame.transform.localPosition = Vector3.zero;
		TdownGame.gameObject.GetComponent<RectTransform> ().sizeDelta = new Vector2(canvasT.rect.width, canvasT.rect.height);
		return TdownGame;
	}
}
