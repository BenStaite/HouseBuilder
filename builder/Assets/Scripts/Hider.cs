using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour
{
    public bool hidden;
    // Start is called before the first frame update
    void Start()
    {
        hidden = false;
    }

    void HideBuildTools()
    {
        hidden = !hidden;
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("SnapPoint"))
        {
            if (hidden)
            {
                obj.GetComponent<Renderer>().enabled = false;
            }
            else
            {
                obj.GetComponent<Renderer>().enabled = true;
            }
        }
    }
}
