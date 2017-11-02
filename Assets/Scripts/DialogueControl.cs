using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueControl : MonoBehaviour {

	private Text fToInteract;
	private Image dialogueBox;
	private Dictionary<string, DialogueFrame> frameMap = new Dictionary<string, DialogueFrame> ();
	private bool inDialogue = false;
	private DialogueFrame curDialogue;

	// Use this for initialization
	void Start () {
		fToInteract = this.gameObject.transform.Find ("FToInteract").gameObject.GetComponent<Text>();
		dialogueBox = this.gameObject.GetComponent<Image> ();
		DialogueFrame[] allFrames = this.gameObject.GetComponents<DialogueFrame> ();
		foreach (DialogueFrame frame in allFrames) {
			if (!frameMap.ContainsKey (frame.id)) {
				frameMap.Add (frame.id, frame);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (inDialogue && Input.GetKey ("F")) {
			NextDialogue ();
		}
	}

	public void ToggleFToInteract(){
		fToInteract.enabled = !(fToInteract.enabled);
	}

	public void ToggleDialogueBox(){
		dialogueBox.enabled = !(dialogueBox.enabled);
	}

	public void ExitDialogue(){

	}

	public void BeginDialogue(string firstFrameID){
		inDialogue = true;
		curDialogue = frameMap [firstFrameID];
		//frameMap [firstFrameID];
	}

	private void NextDialogue(){
		string nextText = curDialogue.getNext ();
		if (nextText == "exit") {

			inDialogue = false;
		} else {

		}
	}
}
