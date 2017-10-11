using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackout : MonoBehaviour {

    SpriteRenderer blackout;

    // Use this for initialization
    void Start () {
        blackout = GetComponent<SpriteRenderer>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Drop the blackout object over the camera
    public IEnumerator FadeInBlack()
    {
        while (blackout.color.a < 1.0f)
        {
            blackout.color = new Color(0.0f, 0.0f, 0.0f, blackout.color.a + 0.05f);
            yield return null;
        }
    }

    // Pull the blackout object off the camera
    public IEnumerator FadeOutBlack()
    {
        while (blackout.color.a > 0.0f)
        {
            blackout.color = new Color(0.0f, 0.0f, 0.0f, blackout.color.a - 0.01f);
            yield return null;
        }
    }
}
