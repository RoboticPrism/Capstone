using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackout : MonoBehaviour {

    SpriteRenderer sprite;

    // Use this for initialization
    void Start () {
        sprite = GetComponent<SpriteRenderer>();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Drop the blackout object over the camera
    public IEnumerator FadeInBlack()
    {
        while (sprite.color.a < 1.0f)
        {
            sprite.color = new Color(0.0f, 0.0f, 0.0f, sprite.color.a + 0.05f);
            yield return null;
        }
    }

    // Pull the blackout object off the camera
    public IEnumerator FadeOutBlack()
    {
        // This is here because this could run before sprite is set, so just wait until its there
        while (sprite == null)
        {
            yield return null;
        }
        while (sprite.color.a > 0.0f)
        {
            sprite.color = new Color(0.0f, 0.0f, 0.0f, sprite.color.a - 0.01f);
            yield return null;
        }
    }
}
