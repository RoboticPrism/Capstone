using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//This class functions as the container for the universal constants of our game
//as well as handling the loading and saving of the game state
public static class StateSaver {

	public enum Minigames { PatternSquares };

	public enum Area {Base=0, Jack=1, Overworld=2, SR3=3, SR5=4, BLD2=5};
	public const string homeArea = "TestBase";
	public const string overworld = "OverworldExampleScene";
	public const string jack = "Jack's";
	public const string sr3 = "StoryRom3";
	public const string bld2 = "Building2";
	public const string sr5 = "SR5";
	public static AreaInfo[] areas = { new AreaInfo(homeArea, 0), new AreaInfo(jack, 3), new AreaInfo(overworld, 3), new AreaInfo(sr3, 1), new AreaInfo(sr5, 0), new AreaInfo(bld2, 0)};
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
			gameState.startup ();
            file.Close();
			return true;
        }
		return false;
    }

	public static void Reset(){
		gameState = new GameState ();
	}
}
