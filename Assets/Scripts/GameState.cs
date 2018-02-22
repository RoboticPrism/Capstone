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

	private float foodTimer;
	private const float decayTime = 60.0f;
	public float timeLeft = decayTime;

    public GameState()
    {
		areas = (AreaInfo[])StateSaver.areas.Clone();
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

	public void update(){
		timeLeft = decayTime - (Time.time - foodTimer);
		if (timeLeft <= 0 && foodStorage >= 0) {
			foodStorage -= 1;
			foodTimer = Time.time;
		} 
	}

	public void startup(){
		curArea = areas [0];
		foodTimer = Time.time;
	}
}
