using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneWarp : MonoBehaviour {

    public string sceneToLoad;
    bool transitioning = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (sceneToLoad != null)
        {
            if (col.GetComponent<SideScrollingPlayer>())
            {
                StartCoroutine(col.GetComponent<SideScrollingPlayer>().blackout.FadeInBlack());
                SceneManager.LoadSceneAsync(sceneToLoad);
            }
            else if (col.GetComponent<TopDownPlayer>())
            {
                StartCoroutine(col.GetComponent<TopDownPlayer>().blackout.FadeInBlack());
                SceneManager.LoadSceneAsync(sceneToLoad);
            }
        }
    }
}
