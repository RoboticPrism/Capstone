using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneWarp : MonoBehaviour {

    public string sceneToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.GetComponent<PlayerMovement>() != null ||
            col.GetComponent<TopDownPlayer>() != null) &&  
            sceneToLoad != null)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
