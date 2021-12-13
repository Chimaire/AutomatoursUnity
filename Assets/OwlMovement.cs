using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlMovement : MonoBehaviour
{
    public GameObject player;
    public float heightOffset;

    float hoverLowerBound = 0.95f;
    float hoverUpperBound = 1.05f;
    public bool goingUp = true; //True = up, false = down
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp)
        {
            if(transform.position.y < hoverUpperBound)
            {
                transform.position += new Vector3(0, Time.deltaTime * 0.05f, 0);
            }
            else
            {
                goingUp = false;
            }
        }
        else
        {
            if (transform.position.y > hoverLowerBound)
            {
                transform.position -= new Vector3(0, Time.deltaTime * 0.05f, 0);
            }
            else
            {
                goingUp = true;
            }
        }
        this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));
         
    }
}
