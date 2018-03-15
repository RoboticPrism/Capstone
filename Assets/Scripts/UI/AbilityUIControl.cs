using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUIControl : MonoBehaviour {

	public Button BarkButton;
	public Button SniffButton;
	public Button TakedownButton;
	public Button SaveButton;
	private Text BarkText;
	private Text SniffText;
	private Text TakedownText;
	public GameObject AbilityDescription;
	public GameObject toggleText;
	private CanvasGroup cg;

	private const float barkCooldown = 2.0f;
	private float lastBark = -60.0f;
	private bool barkAvail = false;
	private const float sniffCooldown = 50.0f;
	private float lastSniff = -60.0f;
	private bool sniffAvail = false;
	private const float takedownCooldown = 10.0f;
	private float lastTakedown = -60.0f;
	private bool takedownAvail = false;
	private bool saveAvail;

	// Use this for initialization
	void Start () {
		this.gameObject.transform.SetParent (GameObject.Find ("Canvas").transform);
		BarkText = BarkButton.GetComponentInChildren<Text> ();
		SniffText = SniffButton.GetComponentInChildren<Text> ();
		TakedownText = TakedownButton.GetComponentInChildren<Text> ();
		this.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 30);
		cg = this.gameObject.transform.Find("TransparentGroup").gameObject.GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - lastBark >= barkCooldown) {
			barkAvail = true;
			BarkText.text = "'Q'\nBark";
		} else {
			barkAvail = false;
			BarkText.text = "" + Mathf.Ceil(barkCooldown - (Time.time - lastBark));
		}
		if (Time.time - lastSniff >= sniffCooldown) {
			sniffAvail = true;
			SniffText.text = "'E'\nSniff";
		} else {
			sniffAvail = false;
			SniffText.text = "" + Mathf.Ceil(sniffCooldown - (Time.time - lastSniff));
		}
		if (Time.time - lastTakedown >= takedownCooldown) {
			takedownAvail = true;
			TakedownText.text = "'T'\nTakedown";
		} else {
			takedownAvail = false;
			TakedownText.text = "" + Mathf.Ceil(takedownCooldown - (Time.time - lastTakedown));
		}

		BarkButton.image.color = barkAvail ? Color.white : Color.grey;
		SniffButton.image.color = sniffAvail ? Color.white : Color.grey;
		TakedownButton.image.color = takedownAvail ? Color.white : Color.grey;
	}

	public bool BarkAvailable(){
		if (barkAvail) {
			lastBark = Time.time;
		}
		return barkAvail;
	}

	public bool SniffAvailable(){
		if (sniffAvail) {
			lastSniff = Time.time;
		}
		return sniffAvail;
	}

	public bool TakedownAvailable(){
		return takedownAvail;
	}

	public void NotifyTakedown(){
		lastTakedown = Time.time;
	}

	public bool SaveAvailable(){
		return saveAvail;
	}

	public void SaveZone(bool entered){
		saveAvail = entered;
		SaveButton.image.color = saveAvail ? Color.white : Color.grey;
	}
		
	public void onPointerEnter(){
		cg.alpha = 1.0f;
	}

	public void onPointerExit(){
		cg.alpha = 0.5f;
	}

	public void ToggleInfo(){
		toggleText.SetActive (AbilityDescription.activeInHierarchy);
		cg.alpha = !AbilityDescription.activeInHierarchy ? 1.0f : cg.alpha;
		AbilityDescription.SetActive (!AbilityDescription.activeInHierarchy);
	}
		
}
