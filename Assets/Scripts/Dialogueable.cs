using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogueable : MonoBehaviour {

	public Dialogue myDialogue;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BeginDialogue()
	{
		myDialogue.Begin ();
	}
}
