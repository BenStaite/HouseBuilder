using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : MonoBehaviour
{
    public GameObject FloorPrefab, WallPrefab;
    public float FloorPrice, WallPrice;
    public Material DeleteMaterial, PreviewMaterial, transparentMaterial;
    public List<GameObject> objects;

    public GameObject buildObject(GameObject obj, Vector3 pos, Quaternion rot, Transform parent, bool preview)
    {
        Vector3 newpos = pos;
        GameObject newObj = Instantiate(obj, newpos, rot);
        newObj.GetComponent<BuildObject>().setAnchor(pos);
        newObj.transform.localPosition += (obj.GetComponent<BuildObject>().Offset.x * newObj.transform.forward) + (obj.GetComponent<BuildObject>().Offset.y * newObj.transform.up) + (obj.GetComponent<BuildObject>().Offset.z * newObj.transform.right);
        newObj.transform.parent = parent;
        placePreview(newObj, preview);
        return newObj;
    }

    public void deletePreview(GameObject obj, bool isTrue)
    {
        if (isTrue)
        {
            obj.GetComponent<BuildObject>().addTempMaterial(DeleteMaterial);
        }
        else
        {
            obj.GetComponent<BuildObject>().removeTempMaterial(DeleteMaterial);
        }
    }

    public void placePreview(GameObject obj, bool isTrue)
    {
        if (isTrue)
        {
            obj.GetComponent<BuildObject>().addTempMaterial(PreviewMaterial);
            obj.layer = LayerMask.NameToLayer("UI");
        }
        else
        {
            obj.GetComponent<BuildObject>().removeTempMaterial(PreviewMaterial);
            obj.layer = LayerMask.NameToLayer("Default");
        }
    }

    public void makeTransparent(GameObject obj, bool isTrue)
    {
        if (isTrue)
        {
            obj.GetComponent<BuildObject>().addTempMaterial(transparentMaterial);
            obj.layer = LayerMask.NameToLayer("UI");
        }
        else
        {
            obj.GetComponent<BuildObject>().removeTempMaterial(transparentMaterial);
            obj.layer = obj.GetComponent<BuildObject>().layer;
        }
    }

}
