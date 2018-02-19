using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEnter : MonoBehaviour {

	public GameObject infoTracker;
	public StateSaver.Area togo;
	private bool canLeave;
	private TopDownPlayer player;
	// Use this for initialization
	void Start () {
//		if (StateSaver.gameState.areasEntered.Contains(StateSaver.areas[(int)togo].name))
//        {
//            Destroy(this.gameObject);
//        }

		infoTracker.SetActive(false);
		canLeave = false;
	}

	// Update is called once per frame
	void Update () {
		if (canLeave && player.GetComponent<TopDownPlayer>() && Input.GetKeyUp(KeyCode.Space) && StateSaver.areas[(int)togo].name != null){
			canLeave = false;
			StartCoroutine(player.GetComponent<TopDownPlayer>().blackout.FadeInBlack());
			GoToScene();
		}
	}

    void GoToScene()
    {
		StateSaver.gameState.pauseTimer ();
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
			player = col.GetComponent<TopDownPlayer>();
			canLeave = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<TopDownPlayer>())
        {
			infoTracker.SetActive(false);
			canLeave = false;
        }
    }
		
}
