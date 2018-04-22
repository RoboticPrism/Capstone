using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingMM : MonoBehaviour {

	public Vector2 start;
	public Vector2 end;
	private bool towardsEnd = true;
	public float yVariation;
	public float travelTime;
	private bool yup = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 tmpPt = towardsEnd ? end : start;
		Vector2 myPt = this.gameObject.GetComponent<RectTransform> ().anchoredPosition;
		float ypt = yup ? tmpPt.y + yVariation : tmpPt.y - yVariation;
		if (Mathf.Abs (myPt.y - ypt) <= 0.01f) {
			yup = !yup;
		}
		ypt = yup ? tmpPt.y + yVariation : tmpPt.y - yVariation;
		tmpPt = new Vector2 (tmpPt.x, ypt);
		if (Mathf.Abs (myPt.x - tmpPt.x) >= 0.01f) {
			float nextX = Vector2.MoveTowards(myPt, tmpPt, (Mathf.Abs(start.x - end.x)/travelTime)).x;
			float nextY = Vector2.MoveTowards(myPt, tmpPt, (Mathf.Abs(end.y - (end.y + yVariation))/(travelTime/2))).y;
			this.gameObject.GetComponent<RectTransform> ().anchoredPosition = new Vector2(nextX, nextY);
		} else {
			towardsEnd = !towardsEnd;
		}

	}


}
