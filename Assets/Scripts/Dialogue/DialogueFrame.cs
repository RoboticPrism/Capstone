using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFrame : MonoBehaviour {

	public string id;
	public string[] allText;
	private int curInd = -1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string getNext(){
		curInd++;
		return ((curInd < allText.Length) ? allText[curInd] : "exit");
	}

	public void ResetFrame(){
		curInd = 0;
	}
}
