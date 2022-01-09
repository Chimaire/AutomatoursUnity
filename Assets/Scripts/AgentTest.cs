using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{

    public GameObject player;
    NavMeshAgent agent;
    Vector3 goalPosition;
    public Transform owlHome;
    public float lookSpeed;
    public MeshRenderer Owlhead;

    public OwlState currentState = OwlState.Greeting;

    GameObject CurrentAudioInstance;

    public enum OwlState
    {
        Greeting, Moving, WaitingForPlayerInput, Explaining, ExplanationFinished
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        this.transform.position = owlHome.position;
        goalPosition = owlHome.position;
    }
    

    void Update()
    {
        agent.destination = goalPosition;
        if (currentState == OwlState.Explaining)
        {
            if (CurrentAudioInstance.GetComponent<AudioSource>().isPlaying == false)
            {
                Debug.Log("Explanation finished");
                currentState = OwlState.ExplanationFinished;
                Destroy(CurrentAudioInstance);
            }
        }
        

        if(currentState == OwlState.Explaining)
        {
            Owlhead.material.color = new Color(0, 1, 0);
        }else if(currentState == OwlState.Greeting)
        {
            Owlhead.material.color = new Color(1, 1, 1);
        }else if (currentState == OwlState.Moving)
        {
            Owlhead.material.color = new Color(1, 0, 0);
        }else if (currentState == OwlState.ExplanationFinished)
        {
            Owlhead.material.color = new Color(0, 0, 0);
        }else if (currentState == OwlState.WaitingForPlayerInput)
        {
            Owlhead.material.color = new Color(0, 0, 1);
        }


        if (agent.remainingDistance < agent.stoppingDistance)
        {
            if(currentState == OwlState.Moving)
            {
                if(SceneLoader.currentMode != SceneLoader.GameMode.NoControl)
                {
                   currentState = OwlState.WaitingForPlayerInput;
                }
                else
                {
                    StartExplanation(player.GetComponent<PlayerController>().currentExhibit);
                }
            }
            agent.updateRotation = false;
            //transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
            
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, lookSpeed * Time.deltaTime);
            //this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

        }
        else
        {
            currentState = OwlState.Moving;
            agent.updateRotation = true;
        }
    }

    public void SetGoal(Vector3 position)
    {
        goalPosition = position;
    }

    public void SendHome()
    {
        goalPosition = owlHome.position;
    }

    public void StartExplanation(GameObject Exhibit)
    {
        if (CurrentAudioInstance != null)
        {
            Destroy(CurrentAudioInstance);
        }
        Debug.Log("Owl Explanation Started for Exhibit: " + Exhibit.name);
        currentState = OwlState.Explaining;
        ExhibitInfo info = Exhibit.GetComponent<ExhibitInfo>();
        CurrentAudioInstance = Instantiate(info.AudioPrefab, transform);
        CurrentAudioInstance.GetComponent<AudioSource>().Play();
    }

    public void StopExplanation()
    {
        Debug.Log("Explanation Stopped");
        currentState = OwlState.ExplanationFinished;
        Destroy(CurrentAudioInstance);
    }
}
