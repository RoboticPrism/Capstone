using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatternSquares : TakedownGame {

	private enum SquareCode {Red, Green, Blue, Yellow};

	private Button Red;
	private Button Green;
	private Button Blue;
	private Button Yellow;
	private Text message;
	private int patternLength = 0;

	private List<SquareCode> pattern;

	// Use this for initialization
	new void Start () {
		this.gameObject.transform.SetParent (GameObject.Find ("Canvas").transform);
		this.gameObject.GetComponent<RectTransform> ().localPosition = Vector3.zero;
		Red = this.gameObject.transform.Find ("Red").gameObject.GetComponent<Button>();
		Green = this.gameObject.transform.Find ("Green").gameObject.GetComponent<Button>();
		Blue = this.gameObject.transform.Find ("Blue").gameObject.GetComponent<Button>();
		Yellow = this.gameObject.transform.Find ("Yellow").gameObject.GetComponent<Button>();
		message = this.gameObject.transform.Find ("Message").gameObject.GetComponent<Text>();
		HideColors ();
		message.text = "Remember the Pattern";

		patternLength = Random.Range (3, 10);

		pattern = new List<SquareCode> ();

		for (int i = 0; i < patternLength; i++) {
			int r = Random.Range (0, 4);
			switch (r) {
			case 0:
				pattern.Add (SquareCode.Red);
				break;
			case 1:
				pattern.Add (SquareCode.Green);
				break;
			case 2:
				pattern.Add (SquareCode.Blue);
				break;
			case 3:
				pattern.Add (SquareCode.Yellow);
				break;
			}
		}
	}
	
	//Update is called once per frame
	new void Update () {

	}

	private void HideColors(){
		Red.image.color = Color.black;
		Green.image.color = Color.black;
		Blue.image.color = Color.black;
		Yellow.image.color = Color.black;
	}

	protected override void RunGame ()
	{
		StartCoroutine ("DisplayPattern");
	}

	private IEnumerator DisplayPattern(){
		yield return new WaitForSecondsRealtime (1);
		for (int i = 0; i < patternLength; i++) {
			SquareCode cur = pattern [i];
			switch (cur) {
			case SquareCode.Red:
				Red.image.color = Color.red;
				break;
			case SquareCode.Green:
				Green.image.color = Color.green;
				break;
			case SquareCode.Blue:
				Blue.image.color = Color.blue;
				break;
			case SquareCode.Yellow:
				Yellow.image.color = Color.yellow;
				break;
			}
			yield return new WaitForSecondsRealtime (0.5f);
			HideColors ();
			yield return new WaitForSecondsRealtime (0.5f);
		}
		waitingForInput = true;
		Red.image.color = Color.red;
		Green.image.color = Color.green;
		Blue.image.color = Color.blue;
		Yellow.image.color = Color.yellow;
		message.text = "Input the Same Pattern";
	}

	public void RedSelected(){
		print ("Red Clicked");
		SquareSelected (SquareCode.Red);
	}

	public void BlueSelected(){
		print ("Blue Clicked");
		SquareSelected (SquareCode.Blue);
	}

	public void GreenSelected(){
		print ("Green Clicked");
		SquareSelected (SquareCode.Green);
	}

	public void YellowSelected(){
		print ("Yellow Clicked");
		SquareSelected (SquareCode.Yellow);
	}

	private void SquareSelected(SquareCode code){
		if (pattern.Count > 0 && waitingForInput) {
			SquareCode cur = pattern [0];
			if (cur == code) {
				pattern.RemoveAt (0);
				if (pattern.Count == 0) {
					message.text = "Success!";
					StartCoroutine ("EndWait");
					succeeded = true;
				}
			} else {
				message.text = "Failed!";
				StartCoroutine ("EndWait");
				succeeded = false;
			}
		}
	}

	private IEnumerator EndWait(){
		yield return new WaitForSecondsRealtime (1);
		cleanup ();
		done = true;
	}

	private void cleanup(){
		this.gameObject.SetActive (false);
		Destroy (this.gameObject);
	}
}
