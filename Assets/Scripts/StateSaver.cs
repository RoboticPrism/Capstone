using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class StateSaver {

	public enum Area {Base=0, Jack=1, Overworld=2};
	public const string homeArea = "TestBase";
	public const string overworld = "OverworldExampleScene";
	public const string jack = "Jack's";
	public static AreaInfo[] areas = { new AreaInfo(homeArea, 0), new AreaInfo(jack, 3), new AreaInfo(overworld, 3)};
	public static GameState gameState;

    // Writes the contents of this game state to a file
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
		Debug.Log(Application.persistentDataPath + "/savedGames.gd");
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, gameState);
        file.Close();
    }

    public static bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            gameState = (GameState)bf.Deserialize(file);
            file.Close();
			return true;
        }
		return false;
    }

	public static void Reset(){
		gameState = new GameState ();
	}

    
}
