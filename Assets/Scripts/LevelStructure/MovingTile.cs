﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTile : MonoBehaviour {

	Vector3 point1;
	Vector3 point2;
	bool towPt1 = true;
	bool doneMove = false;
	public MovingTile[] group;
	public bool leader = true;
	public bool matchX = false;
	public bool matchY = false;
	private bool matched = false;
	// Use this for initialization
	void Start () {
		this.point1 = transform.Find ("Point 1").position;
		this.point2 = transform.Find ("Point 2").position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!matched && leader) {
			for (int i = 0; i < group.Length; i++) {
				if (matchX) {
					group [i].point1.x = this.point1.x;
				}
				if (matchY) {
					group [i].point1.y = this.point1.y;
				}
			}
			matched = true;
		}
		Vector3 tmpPt = towPt1 ? point1 : point2;
		if (Vector3.Distance (this.gameObject.transform.position, tmpPt) > 0.01f) {
			this.gameObject.transform.position = Vector3.MoveTowards (this.gameObject.transform.position,
				tmpPt,
				5 / 100.0f);
		} else {
			doneMove = true;
			if (leader) {
				bool allDone = doneMove;
				for (int i = 0; i < group.Length; i++) {
					allDone = (allDone && group [i].doneMove);
				}
				if (allDone) {
					swapDir ();
				}
			}
		}
	}

	void swapDir(){
		towPt1 = !towPt1;
		doneMove = false;
		for (int i = 0; i < group.Length; i++) {
			group [i].doneMove = false;
			group [i].towPt1 = !group [i].towPt1;
		}
	}
}