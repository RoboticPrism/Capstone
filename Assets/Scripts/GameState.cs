using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class GameState {

    public int foodStorage;
	public AreaInfo[] areas;
	public AreaInfo curArea;
    public List<string> areasEntered;

	private Thread decayThread;
	private float foodTimer;
	private const float decayTime = 60.0f;
	private static bool decaying;

    public GameState()
    {
		areas = new AreaInfo[] { new AreaInfo(StateSaver.homeArea, 0), new AreaInfo(StateSaver.jack, 3), new AreaInfo(StateSaver.overworld, 3), new AreaInfo(StateSaver.sr3, 1)};
        foodStorage = 10;
		areasEntered = new List<string> ();
		startup ();
    }

	public void AddFood(int food)
    {
        foodStorage += food;
		curArea.foodCount -= food;
    }

    public void RemoveFood(int food)
    {
        foodStorage -= food;
    }

    public void AddAreaToList(string newAreaEntered)
    {
        areasEntered.Add(newAreaEntered);
    }

    public List<string> GetAreasEntered()
    {
        return areasEntered;
    }

	private void foodCountdown(){
		while (foodStorage >= 0) {
			if (decaying && Time.time - foodTimer >= decayTime) {
				foodStorage -= 1;
				foodTimer = Time.time;
			}
		}
	}

	public void pauseTimer(){
		decaying = false;
	}

	public void startTimer(){
		decaying = true;
	}

	public void startup(){
		curArea = areas [0];
		decaying = false;
		foodTimer = Time.time;
		decayThread = new Thread (foodCountdown);
		decayThread.Start ();
	}
}
