using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ModeUIScript : MonoBehaviour
{
    public ToggleGroup toggles;
    public GameObject can;
    public GameObject leftHand;
    public GameObject rightHand;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMode()
    {
        Debug.Log(toggles.ActiveToggles().FirstOrDefault().name);
        SceneLoader.SetGameMode(toggles.ActiveToggles().FirstOrDefault().name);
        can.SetActive(false);
        LayerMask someMask = ~0;
        leftHand.GetComponent<XRRayInteractor>().raycastMask = someMask;
        rightHand.GetComponent<XRRayInteractor>().raycastMask = someMask;
    }


}
