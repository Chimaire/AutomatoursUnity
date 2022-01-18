using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExhibitInfo : MonoBehaviour
{
    public GameObject owlPos1;
    public GameObject owlPos2;
    public GameObject AudioPrefab;
    public GameObject Indicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getOwlPos(Vector3 pos)
    {
        //if the player is closer to one of the positions, the owl should move to the other one. 
        if(Vector3.Distance(pos, owlPos1.transform.position) < Vector3.Distance(pos, owlPos2.transform.position))
        {
            return owlPos2;
        }
        else
        {
            return owlPos1;
        }
    }

    public void EnableOutline()
    {
        Indicator.GetComponent<Outline>().enabled = true;
    }
    public void DisableOutline()
    {
        Indicator.GetComponent<Outline>().enabled = false;
    }
}
