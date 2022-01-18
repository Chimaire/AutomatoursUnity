using UnityEngine;

public class EnableOutline : MonoBehaviour
{
    public GameObject cube;

    public Outline outline;
    
    // Start is called before the first frame update
    void Start()
    {
        outline = cube.AddComponent<Outline>();
        
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.cyan;
        outline.OutlineWidth = 5.6f;
        outline.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
    }
}
