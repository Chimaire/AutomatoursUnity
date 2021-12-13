using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLogic : MonoBehaviour
{
    public bool ChangeScene = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ChangeScene)
        {
            ChangeScene = false;
            SceneLoader.Load(SceneLoader.Scene.TutorialScene);
        }
    }

    public void StartTour()
    {
        SceneLoader.Load(SceneLoader.Scene.BilliardRoomScene);
    }
}
