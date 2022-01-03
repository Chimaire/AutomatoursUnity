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
}
