using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour {

    public GameObject exitUI;
	public StateSaver.Area togo;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Add food to stores, save the game, and leave the area
    void GoToScene()
    {
		StateSaver.gameState.AddFood(FindObjectOfType<SideScrollingPlayer>().GetFoodFound());
        StateSaver.Save();
		StateSaver.gameState.curArea = StateSaver.gameState.areas [(int)togo];
		SceneManager.LoadSceneAsync(StateSaver.areas[(int)togo].name);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<SideScrollingPlayer>())
        {
            exitUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<SideScrollingPlayer>())
        {
            exitUI.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
		if (Input.GetAxis("Jump") > 0 && StateSaver.areas[(int)togo].name != null)
        {
            if (col.GetComponent<SideScrollingPlayer>())
            {
                StartCoroutine(col.GetComponent<SideScrollingPlayer>().blackout.FadeInBlack());
                GoToScene();
            }
        }
    }
}
