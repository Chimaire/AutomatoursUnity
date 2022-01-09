using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitInfo : MonoBehaviour
{
    public Transform owlPos1;
    public Transform owlPos2;
    public GameObject AudioPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform getOwlPos(Vector3 pos)
    {
        //if the player is closer to one of the positions, the owl should move to the other one. 
        if(Vector3.Distance(pos, owlPos1.position) < Vector3.Distance(pos, owlPos2.position))
        {
            return owlPos2;
        }
        else
        {
            return owlPos1;
        }
    }
}
