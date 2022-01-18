using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool atExhibit = false;
    public GameObject currentExhibit;
    public AgentTest agentScript;
    public bool inDoorway = false;
    public InputActionReference callRobotReference = null;
    public InputActionReference startRobotReference = null;
    public InputActionReference stopRobotReference = null;
    public Animator RedButtonAnim;
    public Animator GreenButtonAnim;
    // Start is called before the first frame update
    private void Awake()
    {
        callRobotReference.action.started += CallOver;
        startRobotReference.action.started += StartRobot;
        stopRobotReference.action.started += StopRobot;

        callRobotReference.action.canceled += GreenButtonUp;
        startRobotReference.action.canceled += GreenButtonUp;
        stopRobotReference.action.canceled += RedButtonUp;

    }

    private void OnDestroy()
    {

        callRobotReference.action.started -= CallOver;
        startRobotReference.action.started -= StartRobot;
        stopRobotReference.action.started -= StopRobot;

        callRobotReference.action.canceled -= GreenButtonUp;
        startRobotReference.action.canceled -= GreenButtonUp;
        stopRobotReference.action.canceled -= RedButtonUp;
    }

    private void GreenButtonUp(InputAction.CallbackContext context)
    {
        GreenButtonAnim.SetTrigger("Lift");

    }
    private void RedButtonUp(InputAction.CallbackContext context)
    {

        RedButtonAnim.SetTrigger("Lift");
    }

    private void CallOver(InputAction.CallbackContext context)
    {
        GreenButtonAnim.SetTrigger("Press");
        if (atExhibit && (SceneLoader.currentMode == SceneLoader.GameMode.FullControl) && agentScript.currentState != AgentTest.OwlState.WaitingForPlayerInput)
        {
            agentScript.SetOwlState(AgentTest.OwlState.Moving);
            agentScript.SetGoal(currentExhibit.GetComponent<ExhibitInfo>().getOwlPos(this.transform.position));
        }

    }

    private void StartRobot(InputAction.CallbackContext context)
    {
        GreenButtonAnim.SetTrigger("Press");
        if ((SceneLoader.currentMode == SceneLoader.GameMode.FullControl) || (SceneLoader.currentMode == SceneLoader.GameMode.PartialControl))
        {
            if(agentScript.currentState == AgentTest.OwlState.WaitingForPlayerInput)
            {
                agentScript.StartExplanation(currentExhibit);
            }
        }
    }

    private void StopRobot(InputAction.CallbackContext context)
    {
        RedButtonAnim.SetTrigger("Press");
        if ((SceneLoader.currentMode == SceneLoader.GameMode.FullControl))
        {
            if (agentScript.currentState == AgentTest.OwlState.Explaining)
            {
                agentScript.StopExplanation();
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Exhibit"))
        {
            atExhibit = true;
            if(currentExhibit != null)
            {
                currentExhibit.GetComponent<ExhibitInfo>().DisableOutline();
            }
            currentExhibit = other.gameObject;
            currentExhibit.GetComponent<ExhibitInfo>().EnableOutline();
            if (SceneLoader.currentMode == SceneLoader.GameMode.NoControl)
            {
                //get the correct owl position from the exhibit
                if(agentScript.currentState != AgentTest.OwlState.Explaining)
                {
                    agentScript.SetOwlState(AgentTest.OwlState.Moving);
                    agentScript.SetGoal(other.GetComponent<ExhibitInfo>().getOwlPos(this.transform.position));
                }
            }
            else if (SceneLoader.currentMode == SceneLoader.GameMode.PartialControl)
            {
                if (agentScript.currentState != AgentTest.OwlState.Explaining)
                {
                    agentScript.SetOwlState(AgentTest.OwlState.Moving);
                    agentScript.SetGoal(other.GetComponent<ExhibitInfo>().getOwlPos(this.transform.position));
                }
            }
            //  owlScript.SetGoal(this.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Exhibit"))
        {
            atExhibit = false;
            if (currentExhibit != null)
            {
                currentExhibit.GetComponent<ExhibitInfo>().DisableOutline();
            }
            currentExhibit = null;
            if(agentScript.currentState == AgentTest.OwlState.WaitingForPlayerInput)
            {
                agentScript.SetOwlState(AgentTest.OwlState.Greeting);
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    


    public void StartTour()
    {
        SceneLoader.Load(SceneLoader.Scene.BilliardRoomScene);
    }

    public void GoToRoom(string room)
    {
        SceneLoader.Load((SceneLoader.Scene)System.Enum.Parse(typeof(SceneLoader.Scene), room));
    }

    public void EndGame()
    {
        Application.Quit();
        Debug.Break();
    }
}
