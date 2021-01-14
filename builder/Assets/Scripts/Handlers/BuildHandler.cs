using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHandler : MonoBehaviour
{
    public GameObject placePreview;
    public GameObject deletepreview;

    public ObjectHandler objHandler;

    public LayerMask layer;
    public string HitTag;
    public string ObjectTag;
    public GameObject Object;
    public BuildObject buildObject;
    public string buildType;
    public bool buildmode;



    // Start is called before the first frame update
    void Start()
    {
        buildmode = false;
        FloorBuildTools(false);
        buildType = "none";
    }


    public void updateObject(GameObject obj)
    {
        Object = obj;
        buildObject = obj.GetComponent<BuildObject>();
        buildType = obj.GetComponent<BuildObject>().objName;
    }
    // Update is called once per frame
    void Update()
    {
        if (buildmode)
        {
            if (Object)
            {
                ObjectTag = Object.tag;
            }
            if (buildType == "floor")
            {
                if (!detectObjectBuild())
                {
                    Destroy(placePreview);
                    unsetDelete();
                }
                FloorBuildTools(true);
            }
            else if(buildType == "none")
            {
                Destroy(placePreview);
                unsetDelete();
                FloorBuildTools(false);
            }
            else
            {
                if (!detectObjectBuild())
                {
                    Destroy(placePreview);
                    unsetDelete();
                }
                FloorBuildTools(false);
            }
        }
        else
        {
            Destroy(placePreview);
            unsetDelete();
        }
    }

    void FloorBuildTools(bool mode)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("SnapPoint"))
        {
            if (mode)
            {
                obj.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                obj.GetComponent<Renderer>().enabled = false;
            }
        }
    }


    Vector3 findClosestEdge(Vector3 origin, Transform trans)
    {
        Vector3 xOffset = new Vector3(.5f, 0, 0);
        Vector3 zOffset = new Vector3(0, 0, .5f);
        float dist1 = Vector3.Distance(origin, trans.position + xOffset);
        float dist2 = Vector3.Distance(origin, trans.position - xOffset);
        float dist3 = Vector3.Distance(origin, trans.position + zOffset);
        float dist4 = Vector3.Distance(origin, trans.position - zOffset);
        Debug.DrawLine(origin, trans.position + xOffset, Color.blue);
        Debug.DrawLine(origin, trans.position - xOffset, Color.blue);
        Debug.DrawLine(origin, trans.position + zOffset, Color.blue);
        Debug.DrawLine(origin, trans.position - zOffset, Color.blue);
        float smallest = Mathf.Min(dist1, dist2, dist3, dist4);
        Vector3 edge;
        if (smallest == dist1)
        {
            edge = trans.position + xOffset;
        }
        else if (smallest == dist2)
        {
            edge = trans.position - xOffset;
        }
        else if (smallest == dist3)
        {
            edge = trans.position + zOffset;
        }
        else if (smallest == dist4)
        {
            edge = trans.position - zOffset;
        }
        else
        {
            edge = trans.position;
        }
        return (edge);
    }

    void setPreviewObject(GameObject obj, Vector3 pos, Quaternion rot, Transform parent)
    {
        if (placePreview)
        {
            if (placePreview.GetComponent<BuildObject>().Anchor != pos)
            {
                Debug.Log(pos.ToString()+ " " +  placePreview.GetComponent<BuildObject>().Anchor.ToString());
                Destroy(placePreview);
                placePreview = objHandler.buildObject(obj, pos, rot, parent, true);
            }
        }
        else
        {
            placePreview = objHandler.buildObject(obj, pos, rot, parent, true);
        }
    }

    void setDelete(GameObject obj)
    {
        if (deletepreview)
        {
            if (deletepreview.GetComponent<BuildObject>().Anchor != obj.GetComponent<BuildObject>().Anchor)
            {
                unsetDelete();
                deletepreview = obj;
                objHandler.deletePreview(deletepreview, true);
            }
        }
        else
        {
            deletepreview = obj;
            objHandler.deletePreview(deletepreview, true);
        }
    }

    void unsetDelete()
    {
        if (deletepreview)
        {
            objHandler.deletePreview(deletepreview, false);
            deletepreview = null;
        }
    }

    bool detectObjectBuild()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f, layer))
        {
            Debug.DrawLine(new Vector3(), hit.point);
            HitTag = hit.transform.tag;
            if (hit.transform.tag == buildObject.requiredParent.tag)
            {
                unsetDelete();
                Vector3 newpos;
                if (buildObject.placePoints == 1)
                {
                    newpos = hit.transform.position;
                }
                else
                {
                    newpos = findClosestEdge(hit.point, hit.transform);
                }
                Quaternion rotation = Quaternion.FromToRotation(new Vector3(1, 0, 0), newpos - hit.transform.position);
                setPreviewObject(Object, newpos, rotation, hit.transform);
                if (Input.GetMouseButtonDown(0))
                {
                    objHandler.buildObject(Object, newpos, rotation, hit.transform, false);
                    return false;
                }
                return true;
            }
            else if (hit.transform.tag == Object.tag && hit.transform.gameObject != placePreview)
            {
                Destroy(placePreview);
                setDelete(hit.transform.gameObject);
                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(deletepreview);
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
