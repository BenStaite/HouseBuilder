using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    public Canvas buildCanvas;
    public List<Button> buttons;
    public UIHandler handler;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Button b in buildCanvas.GetComponents<Button>())
        {
            buttons.Add(b);
        }
        foreach(Button b in buttons)
        {
            if(b.name != "FloorButton" && b.name != "WallButton")
            {
               // b.onClick.AddListener(delegate { handler.updateObject(b.GetComponent<ButtonPrefab>().Prefab, b); });
            }
        }
    }

    public void updateListeners()
    {
        foreach (Button b in buildCanvas.GetComponents<Button>())
        {
            buttons.Add(b);
        }
        foreach (Button b in buttons)
        {
            if (b.name != "FloorButton" && b.name != "WallButton")
            {
                //b.onClick.AddListener(delegate { handler.updateObject(b.GetComponent<ButtonPrefab>().Prefab, b); });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
