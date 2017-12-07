using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniffable : MonoBehaviour {

	public Texture2D myIndicator;

	private GameObject me;
	private SpriteRenderer whatILookLike;
	public bool sniffed;
	public bool onscreen;
	private Camera mainCam;
	public GUIStyle stylish;

	// Use this for initialization
	void Start () {
		me = this.gameObject;
		whatILookLike = me.GetComponent<SpriteRenderer> ();
		sniffed = false;
		onscreen = false;
		mainCam = Camera.main;
		stylish.normal.textColor = new Vector4 (0, 0, 0, 0);
		TurnMeOff ();
	}
	
	// Update is called once per frame
	void Update () {
//		if (sniffed) {
//			if (onscreen) {
//				if (litUp) {
//
//				} 
//			} else {
//
//			}
//		}
	}

//	public void UpdateFromRoomMan(bool stillOnscreen, float angToTarget, Vector3 centerOfPlayer, Vector3 targetPos){
//		if (sniffed) {
//			if (onscreen) {
//				if (litUp) {
//					if (!stillOnscreen) {
//						LightMyDirectionUp (angToTarget, centerOfPlayer, targetPos);
//					}
//				} else {
//					if (stillOnscreen) {
//						LightMeUp ();
//					} else {
//						LightMyDirectionUp (angToTarget, centerOfPlayer, targetPos);
//					}
//				}
//			} else {
//				LightMyDirectionUp (angToTarget, centerOfPlayer, targetPos);
//			}
//		}
//		onscreen = stillOnscreen;
//	}

	public void LightMeUp(){
		whatILookLike.color = Color.yellow;
	}

	public void TurnMeOff(){
		whatILookLike.color = Color.cyan;
	}

	public void Sniffed(){
		sniffed = true;
		if (onscreen) {
			LightMeUp ();
		}
	}

//	void OnGUI(){
//		if (sniffed && (!onscreen)) {
//			int halfSW = Screen.width / 2;
//			int halfSH = Screen.height / 2;
//
//			Vector3 dir = Vector3.Normalize(me.transform.position - mainCam.transform.position) * -1f;
//
//			Vector2 indPos = new Vector2 (((halfSW) * dir.x) + halfSW, ((halfSH) * dir.y) + halfSH);
//
//			Vector3 theDirection = Vector3.Normalize(me.transform.position - mainCam.ScreenToWorldPoint(new Vector3(indPos.x, indPos.y)));
//
//			float angle = Mathf.Atan2(theDirection.x, theDirection.y) * Mathf.Rad2Deg;
//
//			GUIUtility.RotateAroundPivot(angle, indPos);
//			GUI.Box (new Rect (indPos.x, indPos.y, 25, 25), myIndicator, stylish);
//			GUIUtility.RotateAroundPivot(0, indPos);
//		}
//	}

	void OnBecameVisible(){
		onscreen = true;
		if (sniffed) {
			LightMeUp ();
		}
	}

	void OnBecameInvisible(){
		onscreen = false;
		TurnMeOff ();
	}
}
