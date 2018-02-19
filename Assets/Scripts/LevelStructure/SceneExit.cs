using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour {

    public GameObject exitUI;
	public StateSaver.Area togo;
	private bool canLeave;
	//private Collider2D player;
	private SideScrollingPlayer player;
	// Use this for initialization
	void Start () {
		canLeave = false;
		player = FindObjectOfType<SideScrollingPlayer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (canLeave && Input.GetKeyUp(KeyCode.Space) && StateSaver.areas[(int)togo].name != null)
		{
			canLeave = false;
			StartCoroutine(player.blackout.FadeInBlack());
			GoToScene();
		}
	}

    // Add food to stores, save the game, and leave the area
    void GoToScene()
    {
		StateSaver.gameState.pauseTimer ();
		StateSaver.gameState.AddFood(player.GetFoodFound());
        StateSaver.Save();
		StateSaver.gameState.curArea = StateSaver.gameState.areas [(int)togo];
		SceneManager.LoadSceneAsync(StateSaver.areas[(int)togo].name);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<SideScrollingPlayer>())
        {
            exitUI.SetActive(true);
			canLeave = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<SideScrollingPlayer>())
        {
            exitUI.SetActive(false);
			canLeave = false;
        }
    }
		
}
