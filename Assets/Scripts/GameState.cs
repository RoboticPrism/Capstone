using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState {

    public int foodStorage;
	public AreaInfo[] areas;
	public AreaInfo curArea;
    public List<string> areasEntered;

    public GameState()
    {
		areas = new AreaInfo[] { new AreaInfo(StateSaver.homeArea, 0), new AreaInfo(StateSaver.jack, 3), new AreaInfo(StateSaver.overworld, 3), new AreaInfo(StateSaver.sr3, 1)};
        foodStorage = 10;
		curArea = areas [0];
        areasEntered = new List<string>();
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
}
