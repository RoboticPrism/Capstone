using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameState {

    public int foodStorage;
    public List<string> areasEntered;

    public GameState()
    {
        foodStorage = 10;
        areasEntered = new List<string>();
    }

    public void AddFood(int food)
    {
        foodStorage += food;
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
