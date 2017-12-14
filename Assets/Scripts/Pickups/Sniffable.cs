using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniffable : MonoBehaviour {

	public bool sniffed;
	public bool onscreen;

	// Use this for initialization
	void Start () {
		sniffed = false;
		onscreen = false;
		TurnMeOff ();
	}
	
	// Update is called once per frame
	void Update () {

	}



	public void LightMeUp(){
	}

	public void TurnMeOff(){
	}

	public void Sniffed(){
		sniffed = true;
		if (onscreen) {
			LightMeUp ();
		}
	}

	void OnBecameVisible(){
		onscreen = true;
		if (sniffed) {
			LightMeUp ();
		}
	}

	void OnBecameInvisible(){
		onscreen = false;
		TurnMeOff ();
	}
}
