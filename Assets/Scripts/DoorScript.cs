using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorScript : MonoBehaviour
{
    public GameObject doorCanvas;
    public GameObject leftHand;
    public GameObject rightHand;
    // Start is called before the first frame update
    void Start()
    {
        doorCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            doorCanvas.SetActive(true);
            LayerMask mask = LayerMask.GetMask("UI");
            leftHand.GetComponent<XRRayInteractor>().raycastMask = mask;
            rightHand.GetComponent<XRRayInteractor>().raycastMask = mask;
            other.GetComponent<PlayerController>().inDoorway = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorCanvas.SetActive(false);
            other.GetComponent<PlayerController>().inDoorway = false;
        }
    }
    public void HideCanvas()
    {
        LayerMask mask = ~0;
        leftHand.GetComponent<XRRayInteractor>().raycastMask = mask;
        rightHand.GetComponent<XRRayInteractor>().raycastMask = mask;
        doorCanvas.SetActive(false);

    }

}
