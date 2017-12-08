using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnter : MonoBehaviour {

    public string sceneToLoad;
    bool transitioning = false;
    public int foodCountForArea;
    AreaInfo areaInfo;

	// Use this for initialization
	void Start () {
        areaInfo = FindObjectOfType<AreaInfo>();
        areaInfo.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}

    void GoToScene()
    {
        StateSaver.gameState.AddAreaToList(sceneToLoad);
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<TopDownPlayer>())
        {
            areaInfo.SetFoodCount(foodCountForArea);
            areaInfo.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<TopDownPlayer>())
        {
            areaInfo.gameObject.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(Input.GetAxis("Jump") > 0 && sceneToLoad != null)
        {
            if (col.GetComponent<TopDownPlayer>())
            {
                StartCoroutine(col.GetComponent<TopDownPlayer>().blackout.FadeInBlack());
                GoToScene();
            }
        }
    }
}
