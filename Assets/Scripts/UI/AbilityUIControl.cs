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
	public Text abilityDesc;
	private CanvasGroup cg;

	private string sniffDesc = "Sniff - Reveals all food items in the room.";
	private string barkDesc = "Bark - All enemies within a radius will move towards the sound.";
	private string takedownDesc = "Takedown - Attempt to destroy an enemy when next to them.";
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
		TakedownButton.image.color = takedownAvail ? Color.white : Color.grey;
	}

	public bool BarkAvailable(){
		return true;
	}

	public bool SniffAvailable(){
		return true;
	}

	public bool TakedownAvailable(){
		return takedownAvail;
	}

	public bool SaveAvailable(){
		return saveAvail;
	}

	public void SaveZone(bool entered){
		saveAvail = entered;
		SaveButton.image.color = saveAvail ? Color.white : Color.grey;
	}

	public void onPointerEnterBark(){
		abilityDesc.text = barkDesc;
		cg.alpha = 1.0f;
		ToggleInfo ();
	}

	public void onPointerExitBark(){
		cg.alpha = 0.5f;
		ToggleInfo ();
	}
	public void onPointerEnterSniff(){
		abilityDesc.text = sniffDesc;
		cg.alpha = 1.0f;
		ToggleInfo ();
	}

	public void onPointerExitSniff(){
		cg.alpha = 0.5f;
		ToggleInfo ();
	}

	public void onPointerEnterTakedown(){
		abilityDesc.text = takedownDesc;
		cg.alpha = 1.0f;
		ToggleInfo ();
	}

	public void onPointerExitTakedown(){
		cg.alpha = 0.5f;
		ToggleInfo ();
	}

	public void onPointerEnterSave(){
		abilityDesc.text = "Press X to Save";
		cg.alpha = 1.0f;
		ToggleInfo ();
	}

	public void onPointerExitSave(){
		cg.alpha = 0.5f;
		ToggleInfo ();
	}


	public void ToggleInfo(){
		cg.alpha = !AbilityDescription.activeInHierarchy ? 1.0f : cg.alpha;
		AbilityDescription.SetActive (!AbilityDescription.activeInHierarchy);
	}
		
}
