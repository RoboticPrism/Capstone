using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour {

	private RectTransform myTransf;

	// Use this for initialization
	void Start () {
		myTransf = this.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		myTransf.Rotate (new Vector3(0, 0, 5));
	}
}
