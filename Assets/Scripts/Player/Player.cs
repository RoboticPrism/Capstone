using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    public Blackout blackout;
    public Rigidbody2D rb;

    // Use this for initialization
    protected void Start () {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(blackout.FadeOutBlack());
    }
	
	// Update is called once per frame
	protected void Update () {
		
	}
}
