using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject doorCanvas;
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
}
