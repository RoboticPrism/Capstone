using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogueable : MonoBehaviour {

	public string[] myDialogueFrames;
	private TextMesh myText;
	private bool inDialogue = false;
	// Use this for initialization
	void Start () {
		myText = this.gameObject.transform.Find ("MyDialogue").gameObject.GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FToInteract(bool set){
		if (!set) {
			myText.text = "";
		} else if(!inDialogue){
			myText.text = "Press 'F'";
		}
	}

	public void BeginDialogue(){
		if (!inDialogue) {
			StartCoroutine (Dialogue ());
		}
	}

	IEnumerator Dialogue()
	{
		inDialogue = true;
		for(int i = 0; i <= myDialogueFrames.Length; i++){
			if (i == myDialogueFrames.Length) {
				myText.text = "";
				inDialogue = false;
			} else {
				myText.text = myDialogueFrames [i];
				yield return new WaitForSeconds (2f);
			}
		}
	}
}
