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

    public bool creatingWall;
    public GameObject WallBuildToolStart;
    public GameObject WallBuildToolEnd;

    public DynamicWalls dynamicWalls;
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
        ObjectTag = Object.tag;
        buildObject = obj.GetComponent<BuildObject>();
        buildType = obj.GetComponent<BuildObject>().objName;
    }

    // Update is called once per frame
    void Update()
    {
        if (buildmode)
        {
            if (buildType == "floor")
            {
                FloorBuild();
                FloorBuildTools(true);
            }
            else if(buildType == "none")
            {
                unsetPreview();
                unsetDelete();
                FloorBuildTools(false);
            }
            else if(buildType == "wall")
            {
                WallBuild();
                unsetPreview();
                FloorBuildTools(false);
            }
            else
            {
                ObjectBuild();
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

    Vector3 findClosestCorner(Vector3 origin, Transform trans)
    {
        Vector3 xOffset = new Vector3(.5f, 0, 0);
        Vector3 zOffset = new Vector3(0, 0, .5f);
        float dist1 = Vector3.Distance(origin, trans.position + xOffset + zOffset);
        float dist2 = Vector3.Distance(origin, trans.position + xOffset - zOffset);
        float dist3 = Vector3.Distance(origin, trans.position - xOffset + zOffset);
        float dist4 = Vector3.Distance(origin, trans.position - xOffset - zOffset);
        Debug.DrawLine(origin, trans.position + xOffset + zOffset, Color.blue);
        Debug.DrawLine(origin, trans.position + xOffset - zOffset, Color.blue);
        Debug.DrawLine(origin, trans.position - xOffset + zOffset, Color.blue);
        Debug.DrawLine(origin, trans.position - xOffset - zOffset, Color.blue);
        float smallest = Mathf.Min(dist1, dist2, dist3, dist4);
        Vector3 edge;
        if (smallest == dist1)
        {
            edge = trans.position + xOffset + zOffset;
        }
        else if (smallest == dist2)
        {
            edge = trans.position + xOffset - zOffset;
        }
        else if (smallest == dist3)
        {
            edge = trans.position - xOffset + zOffset;
        }
        else if (smallest == dist4)
        {
            edge = trans.position - xOffset - zOffset;
        }
        else
        {
            edge = trans.position;
        }
        return (edge);
    }

    void setPreviewObject(GameObject obj, Vector3 pos, Transform parent, bool rotateToFaceParent)
    {
        unsetDelete();

        Quaternion rot;
        if (rotateToFaceParent)
        {
            rot = Quaternion.FromToRotation(new Vector3(1, 0, 0), pos - parent.position);
        }
        else
        {
            rot = new Quaternion();
        }
        
        if (placePreview)
        {
            if (placePreview.GetComponent<BuildObject>().Anchor != pos)
            {
                Destroy(placePreview);
                placePreview = objHandler.buildObject(obj, pos, rot, parent, true);
            }
        }
        else
        {
            placePreview = objHandler.buildObject(obj, pos, rot, parent, true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            objHandler.buildObject(Object, pos, rot, parent, false);
        }
    }

    void unsetPreview()
    {
        if (placePreview)
        {
            Destroy(placePreview);
        }
    }

    void setDelete(GameObject obj)
    {
        unsetPreview();

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

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(deletepreview);
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

    RaycastHit detectWorldPoint()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 100f, layer);
        HitTag = hit.transform.tag;
        return hit;
    }

    void ObjectBuild()
    {
        RaycastHit hit = detectWorldPoint();
        if (hit.transform.tag == buildObject.requiredParent.tag)
        {
            Vector3 pos;
            if (buildObject.placePoints == 1)
            {
                pos = hit.transform.position;
            }
            else
            {
                pos = findClosestEdge(hit.point, hit.transform);
            }
            setPreviewObject(Object, pos, hit.transform, true);
        }
        else if (hit.transform.tag == Object.tag && hit.transform.gameObject != placePreview)
        {
            setDelete(hit.transform.gameObject);
        }
        else
        {
            unsetPreview();
            unsetDelete();
        }
    }

    void WallBuild()
    {
        RaycastHit hit = detectWorldPoint();

        if (hit.transform.tag == "Floor")
        {
            unsetDelete();
            if (Input.GetMouseButtonDown(0))
            {
                WallBuildToolStart.transform.position = findClosestCorner(hit.point, hit.transform);
                creatingWall = true;
            }
            else if (Input.GetMouseButtonUp(0) && creatingWall)
            {
                WallBuildToolEnd.transform.position = findClosestCorner(hit.point, hit.transform);
                dynamicWalls.addWall(WallBuildToolStart.transform.position, WallBuildToolEnd.transform.position, hit.transform);
                WallBuildToolEnd.transform.position = new Vector3(1000, 0, 0);
                creatingWall = false;
            }
            else
            {
                if (creatingWall)
                {
                    WallBuildToolEnd.transform.position = findClosestCorner(hit.point, hit.transform);
                }
                else
                {
                    WallBuildToolStart.transform.position = findClosestCorner(hit.point, hit.transform);
                }
            }
        }
        else if(hit.transform.tag == "Wall")
        {
            setDelete(hit.transform.gameObject);
            WallBuildToolEnd.transform.position = new Vector3(1000, 0, 0);
            WallBuildToolStart.transform.position = new Vector3(1000, 0, 0);
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(deletepreview);
            }
        }
        else
        {
            unsetDelete();

        }
    }

    void FloorBuild()
    {
        RaycastHit hit = detectWorldPoint();
        if (hit.transform.tag == "SnapPoint")
        {
            setPreviewObject(Object, hit.transform.position, hit.transform.root, false);
        }
        else if (hit.transform.tag == "Floor")
        {
            setDelete(hit.transform.gameObject);
        }
        else
        {
            unsetDelete();
            unsetPreview();
        }
    }
}
