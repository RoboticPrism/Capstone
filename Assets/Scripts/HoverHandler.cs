using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public AbilityUIControl control;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerExit(PointerEventData dat){
		control.onPointerExit ();
	}

	public void OnPointerEnter(PointerEventData dat){
		control.onPointerEnter ();
	}
}
