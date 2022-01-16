using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
    private bool debug = false;

    public GameObject player;
    NavMeshAgent agent;
    Vector3 goalPosition;
    public Transform owlHome;
    public float lookSpeed;
    public Animator animator;

    public OwlState currentState = OwlState.Greeting;

    GameObject CurrentAudioInstance;
    private float lookTimer = 0.0f;
    private char currentPointDir = 'L';

    //Texture animation Variables
    public Material Owlhead;
    public Texture2D OwlNeutral;
    public Texture2D OwlBlink;
    private float EyesOpenedTime = 2;
    private float EyesClosedTime = 0.15f;
    private float RandomBlinkFactor = 1.5f;
    private bool IsBlinking = false;
    private float CurrentEyeTime = 0.0f;

    public Texture2D OwlHappy;
    private bool Ishappy = false;
    private float MaxHappyTime = 3;
    private float CurrentHappyTimer = 3;

    /*
    public Texture2D OwlSad;
    private bool IsSad = false;
    private float MaxSadTime = 4;
    private float CurrentSadTimer = 4;*/
    
    public enum OwlState
    {
        Greeting, Moving, WaitingForPlayerInput, Explaining, ExplanationFinished
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        this.transform.position = owlHome.position;
        goalPosition = owlHome.position;

        CurrentEyeTime = EyesOpenedTime + Random.value * RandomBlinkFactor;
        Owlhead.mainTexture = OwlNeutral;
        IsBlinking = false;

        CurrentHappyTimer = MaxHappyTime;
        Ishappy = false;
    }
    
    public void MakeHappy()
    {
        if (!Ishappy)
        {
            Ishappy = true;
            CurrentHappyTimer = 0.0f + Random.value * 2;
            Owlhead.mainTexture = OwlHappy;
            
        }
    }

    void Update()
    {

        if (Ishappy)
        {
            CurrentHappyTimer += Time.deltaTime;
            if(CurrentHappyTimer >= MaxHappyTime)
            {
                Ishappy = false;
                Owlhead.mainTexture = OwlNeutral;
            }
        }
        else
        {
            //Blinking Logic
            if (CurrentEyeTime > 0.0f)
            {
                CurrentEyeTime -= Time.deltaTime;
            }
            else if (IsBlinking)
            {
                Owlhead.mainTexture = OwlNeutral;
                CurrentEyeTime = EyesOpenedTime + Random.value * RandomBlinkFactor;
                IsBlinking = false;
            }
            else
            {
                Owlhead.mainTexture = OwlBlink;
                CurrentEyeTime = EyesClosedTime;
                IsBlinking = true;
            }
        }

        

        agent.destination = goalPosition;
        //Debug.Log(agent.remainingDistance);
        if (currentState == OwlState.Explaining)
        {
            if (CurrentAudioInstance.GetComponent<AudioSource>().isPlaying == false)
            {
                Debug.Log("Explanation finished");
                SetOwlState(OwlState.ExplanationFinished);
                Destroy(CurrentAudioInstance);
            }
        }

        if(currentState == OwlState.Greeting || currentState == OwlState.ExplanationFinished)
        {
            lookTimer += Time.deltaTime;
            if(lookTimer >= 8)
            {
                animator.SetTrigger("Look");
                lookTimer = 0.0f;
            }

        }



        if (!agent.pathPending)
        {
            if (agent.remainingDistance < agent.stoppingDistance)
            {


                if (currentState == OwlState.Moving)
                {
                    //animator.SetTrigger("FlyStop");
                    if (SceneLoader.currentMode != SceneLoader.GameMode.NoControl)
                    {
                        SetOwlState(OwlState.WaitingForPlayerInput);
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
                SetOwlState(OwlState.Moving);
                agent.updateRotation = true;
            }

        }
        else
        {
            Debug.Log("pending");
        }

        if (debug)
        {
            if (currentState == OwlState.Explaining)
            {
                Owlhead.color = new Color(0, 1, 0);
            }
            else if (currentState == OwlState.Greeting)
            {
                Owlhead.color = new Color(1, 1, 1);
            }
            else if (currentState == OwlState.Moving)
            {
                Owlhead.color = new Color(1, 0, 0);
            }
            else if (currentState == OwlState.ExplanationFinished)
            {
                Owlhead.color = new Color(0, 0, 0);
            }
            else if (currentState == OwlState.WaitingForPlayerInput)
            {
                Owlhead.color = new Color(0, 0, 1);
            }
        }
    }

    public void SetGoal(GameObject position)
    {
        goalPosition = position.transform.position;
        currentPointDir = position.GetComponent<ExhibitOwlPos>().pointDirection;
    }

    private void PointAtExhibit()
    {
        if(currentPointDir == 'L')
        {
            animator.SetTrigger("PointL");
        }
        else
        {
            animator.SetTrigger("PointR");
        }
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
        SetOwlState(OwlState.Explaining);
        ExhibitInfo info = Exhibit.GetComponent<ExhibitInfo>();
        CurrentAudioInstance = Instantiate(info.AudioPrefab, transform);
        CurrentAudioInstance.GetComponent<AudioSource>().Play();
        PointAtExhibit();
    }

    public void StopExplanation()
    {
        Debug.Log("Explanation Stopped");
        SetOwlState(OwlState.ExplanationFinished);
        Destroy(CurrentAudioInstance);
    }

    public void SetOwlState(AgentTest.OwlState state)
    {
        if(state == OwlState.Moving && currentState != OwlState.Moving)
        {
            MakeHappy();
            animator.SetTrigger("FlyStart");
            Debug.Log("FlyStart");
        }
        else if (currentState == OwlState.Moving && state == OwlState.WaitingForPlayerInput)
        {
            animator.SetTrigger("FlyStop");
            Debug.Log("FlyStop");
        }
        else if (currentState == OwlState.Moving && state == OwlState.Explaining)
        {
            MakeHappy();
            animator.SetTrigger("FlyStop");
            animator.SetTrigger("Talk");

            Debug.Log("FlyStopTalk");
        }else if(currentState == OwlState.WaitingForPlayerInput && state == OwlState.Explaining)
        {
            MakeHappy();
            animator.SetTrigger("Talk");
        }

        
        if (state == OwlState.ExplanationFinished) {
            animator.SetTrigger("TalkStop");

            Debug.Log("TalkStop");
        }

        currentState = state;

    }
}
