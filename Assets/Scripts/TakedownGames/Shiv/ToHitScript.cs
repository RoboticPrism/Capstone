using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ToHitScript : MonoBehaviour {

	private bool hitSuccess = false;

	private bool startSet = false;

	private bool drop = false;

	private Action missed;

	public Vector3 velocity = Vector3.zero;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (startSet && drop) {
			this.gameObject.transform.position += velocity;
			
		}
	}

	public void OnTriggerEnter2D(Collider2D other){
		if (other.GetComponent<HitBoxScript>() != null) {
			hitSuccess = true;
		}
	}

	public void OnTriggerExit2D(Collider2D other){
		if (other.GetComponent<HitBoxScript>() != null && !hitSuccess) {
			hitSuccess = false;
			missed ();
		}
	}

	public bool HitSuccess(){
		return hitSuccess;
	}

	public void SetStartPos(Vector2 pos, Action callback){
		this.gameObject.GetComponent<RectTransform>().anchoredPosition = pos;
		startSet = true;
		missed = callback;
	}

	public void Drop(){
		drop = startSet;
	}

}
