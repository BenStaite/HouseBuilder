using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIHandler : MonoBehaviour
{
    public bool buildmode;

    public Canvas buildCanvas;
    public Button BuildButton;

    public ButtonHandler buttonHandler;
    public BuildHandler buildHandler;

    public ObjectHandler objectHandler;
    public bool resetTransparents;

    // Start is called before the first frame update
    void Start()
    {
    }

    void ToggleBuildMode()
    {
        buildmode = !buildmode;
        buildHandler.buildmode = buildmode;
        if (buildmode)
        {
            buttonHandler.BuildButtonsVisible(true);
            BuildButton.GetComponent<Image>().color = Color.cyan;
        }
        else
        {
            buttonHandler.BuildButtonsVisible(false);
            BuildButton.GetComponent<Image>().color = Color.white;
        }
    }

    private void Update()
    {
        if (buildmode && buildHandler.buildType != "wall")
        {
            makeWallsTransparent2();
            resetTransparents = false;
        }
        else
        {
            if (!resetTransparents)
            {
                removeWallTransparents();
                resetTransparents = true;
            }
        }
    }

    void removeWallTransparents()
    {
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            objectHandler.makeTransparent(wall, false);
        }
    }

    void makeWallsTransparent()
    {
        List<GameObject> wallsDealtwith = new List<GameObject>();
        foreach (GameObject floor in GameObject.FindGameObjectsWithTag("Floor"))
        {
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, floor.transform.position - Camera.main.transform.position);
            if (Physics.Raycast(ray, out hit, 20f))
            {
                if (hit.transform.tag == "Wall")
                {
                    wallsDealtwith.Add(hit.transform.gameObject);
                }
            }
            foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
            {
                if (wallsDealtwith.Contains(wall))
                {
                    objectHandler.makeTransparent(wall, true);
                }
                else
                {
                    objectHandler.makeTransparent(wall, false);
                }
            }
            Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.red);
        }
    }

    void makeWallsTransparent2()
    {

        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            objectHandler.makeTransparent(wall, false);
        }

        List<GameObject> wallsDealtwith = new List<GameObject>();
        foreach (GameObject floor in GameObject.FindGameObjectsWithTag("Floor"))
        {
            bool reachLimit = false;
            RaycastHit hit;
            Ray ray = new Ray(Camera.main.transform.position, floor.transform.position - Camera.main.transform.position);
            while (!reachLimit)
            {
                if (Physics.Raycast(ray, out hit, 20f,LayerMask.GetMask("Default")))
                {
                    if (hit.transform.tag == "Wall")
                    {
                        wallsDealtwith.Add(hit.transform.gameObject);
                        objectHandler.makeTransparent(hit.transform.gameObject, true);
                        Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.red);
                    }
                    else
                    {
                        reachLimit = true;
                        Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.blue);
                    }
                }
                else
                {
                    reachLimit = true;
                }
            }
        }
    }
}
