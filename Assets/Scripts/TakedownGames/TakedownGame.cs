using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakedownGame : MonoBehaviour {
	protected bool done = false;
	protected bool succeeded = false;
	protected bool waitingForInput = false;

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
		StateSaver.Minigames[] games = (StateSaver.Minigames[])System.Enum.GetValues (typeof(StateSaver.Minigames));
		int l = games.Length;
		int r = Random.Range (0, l);
		StateSaver.Minigames cur = games [r];
		switch (cur) {
		case StateSaver.Minigames.PatternSquares:
			return ((GameObject)Instantiate (Resources.Load ("Prefabs/Games/PatternSquaresPrefab"))).GetComponent<PatternSquares>();
		default:
			return null;
		}
	}
}
