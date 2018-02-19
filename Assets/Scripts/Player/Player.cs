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
    }
	
	// Update is called once per frame
	protected void Update () {
		StateSaver.gameState.update ();
	}

	protected void OnGUI() {
		string foodLeft = StateSaver.gameState.foodStorage.ToString() + " Rations of Food Remaining\n" + StateSaver.gameState.timeLeft.ToString();
		GUI.Label (new Rect(Screen.width/2, 0 + (Screen.height/20), 100, 100), foodLeft);
	}
}
