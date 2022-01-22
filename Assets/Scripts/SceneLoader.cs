using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader 
{
    public enum Scene
    {
        TutorialScene, BilliardRoomScene, GreatDrawingRoomScene
    }
    public enum GameMode
    {
        NoControl, PartialControl, FullControl
    }

    public static GameMode currentMode = GameMode.NoControl;
    public static List<ExportItem> entries = new List<ExportItem>();

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static void SetGameMode(string mode)
    {
        currentMode = (SceneLoader.GameMode)System.Enum.Parse(typeof(SceneLoader.GameMode), mode);
        Debug.Log(currentMode);
        //currentMode = mode;
    }
    
    public static void AddEntryToList(ExhibitInfo info, float listenedTime, float cliplength)
    {
        bool valid = true;
        if(cliplength > listenedTime + 3)
        {
            valid = false;
        }
        entries.Add(new ExportItem(info.gameObject.name, valid, cliplength, listenedTime));

        FileHandler.SaveToJSON<ExportItem>(entries, "FinalTestFullControl.json");

    }


}
