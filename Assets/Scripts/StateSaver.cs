using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//This class functions as the container for the universal constants of our game
//as well as handling the loading and saving of the game state
public static class StateSaver {

	public enum Minigames { PatternSquares = 0, ShivToTheBeat, num_opts };

	public enum Area {Base=0, Jack, Overworld, SR3, SR5, BLD2, SR1, RM1, RM2, RM3};
	public static string[] areaNames = {"TestBase", "Jack's", "OverworldExampleScene", "StoryRom3", "SR5", "Building2", "StoryRoom1", "Room1", "Room2", "Room3"};
	public static AreaInfo[] areas = { new AreaInfo(Area.Base, 0), new AreaInfo(Area.Jack, 3), new AreaInfo(Area.Overworld, 3), new AreaInfo(Area.SR3, 1), new AreaInfo(Area.SR5, 0), new AreaInfo(Area.BLD2, 0), new AreaInfo(Area.SR1, 0), new AreaInfo(Area.RM1, 0), new AreaInfo(Area.RM2, 0), new AreaInfo(Area.RM3, 0)};
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
