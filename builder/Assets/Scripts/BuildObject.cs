using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour
{
    public GameObject thisObject;
    public LayerMask layer;
    public float price;
    public Material previousMaterial;
    public Vector3 Offset;
    public string objName;
    public bool tempMat;
    public GameObject requiredParent;
    public int placePoints;
    public bool isValid;
    public Vector3 Anchor;

    public void Start()
    {
        isValid = true;
        thisObject = transform.gameObject;
        previousMaterial = GetComponent<Renderer>().material;
        layer = gameObject.layer;
    }

    public void changeMaterial(Material mat, bool rememberMat)
    {
        if (rememberMat)
        {
            previousMaterial = thisObject.GetComponent<Renderer>().material;
        }
        thisObject.GetComponent<Renderer>().material = mat;
    }

    public void addTempMaterial(Material mat)
    {
        if (tempMat)
        {
            GetComponent<Renderer>().material = mat;
        }
        else
        {
            previousMaterial = GetComponent<Renderer>().material;
            GetComponent<Renderer>().material = mat;
            tempMat = true;
        }
        
    }

    public void setAnchor(Vector3 anchor)
    {
        Anchor = anchor;
    }

    public void UILayer(bool isTrue)
    {

    }

    public void removeTempMaterial(Material mat)
    {
        if (tempMat)
        {
            GetComponent<Renderer>().material = previousMaterial;
            tempMat = false;
        }
    }


    public void revertMaterial()
    {
        thisObject.GetComponent<Renderer>().material = previousMaterial;
    }

    public void remove()
    {
        Destroy(thisObject);
    }
}
