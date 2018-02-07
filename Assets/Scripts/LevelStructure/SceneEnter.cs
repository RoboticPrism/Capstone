using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnter : MonoBehaviour {

	public GameObject infoTracker;
	public StateSaver.Area togo;

	// Use this for initialization
	void Start () {
//		if (StateSaver.gameState.areasEntered.Contains(StateSaver.areas[(int)togo].name))
//        {
//            Destroy(this.gameObject);
//        }

		infoTracker.SetActive(false);

	}

	// Update is called once per frame
	void Update () {

	}

    void GoToScene()
    {
		if (togo != StateSaver.Area.Base) {
			StateSaver.gameState.AddAreaToList (StateSaver.gameState.areas[(int)togo].name);
		}
		StateSaver.gameState.curArea = StateSaver.gameState.areas [(int)togo];
		SceneManager.LoadSceneAsync(StateSaver.areas[(int)togo].name);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<TopDownPlayer>())
        {
			infoTracker.GetComponent<AreaDisplay>().SetFoodCount(StateSaver.gameState.areas[(int)togo].foodCount);
			infoTracker.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<TopDownPlayer>())
        {
			infoTracker.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
		if(Input.GetAxis("Jump") > 0 && StateSaver.areas[(int)togo].name != null)
        {
            if (col.GetComponent<TopDownPlayer>())
            {
                StartCoroutine(col.GetComponent<TopDownPlayer>().blackout.FadeInBlack());
                GoToScene();
            }
        }
    }
}
