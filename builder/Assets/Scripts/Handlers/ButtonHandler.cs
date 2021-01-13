using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Canvas buildCanvas;
    public List<GameObject> buttons;
    public UIHandler handler;
    public BuildHandler buildHandler;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("BuildButton"))
        {
            buttons.Add(b);
        }
        foreach (GameObject b in buttons)
        {
            b.GetComponent<Button>().onClick.AddListener(delegate { buildHandler.updateObject(b.GetComponent<ButtonPrefab>().Prefab); highlightButton(b); });
        }
    }

    public void highlightButton(GameObject button)
    {
        foreach (GameObject b in buttons)
        {
            if(b == button)
            {
                b.GetComponent<Image>().color = Color.cyan;
            }
            else{
                b.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void BuildButtonsVisible(bool show)
    {
        foreach (GameObject b in buttons)
        {
            if (show)
            {
                b.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                b.transform.localScale = new Vector3(0, 0, 0);
            }
        }
    }

}
