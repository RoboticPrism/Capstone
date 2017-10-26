using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour {

	private GameObject fToInteract;
	private bool fSwitch = false;
	private DialogueFrame[] allFrames;

	// Use this for initialization
	void Start () {
		fToInteract = GameObject.Find ("FToInteract");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
