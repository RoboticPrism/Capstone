using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public Blackout blackout;
    protected Rigidbody2D rb;

    // Use this for initialization
    protected void Start () {
        rb = GetComponent<Rigidbody2D>();
		blackout.gameObject.SetActive(true);
        StartCoroutine(blackout.FadeOutBlack());
		StateSaver.gameState.startTimer ();
    }
	
	// Update is called once per frame
	protected void Update () {
		
	}
}
