using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWalls : MonoBehaviour
{
    public GameObject wallMiddle, wallCorner;
    public List<WallCorner> corners;
    public List<WallMiddle> middles;
    WallCorner nullCorner;
    GameObject nullObject;
    WallMiddle nullMiddle;

    public float wallHeight;
    // Start is called before the first frame update
    void Start()
    {
        corners = new List<WallCorner>();
        middles = new List<WallMiddle>();
    }

    public bool addWall(Vector3 pos1, Vector3 pos2, Transform parent)
    {
        if (!areAdjacent(pos1, pos2)) return false;
        WallCorner corner1 = cornerExists(pos1);
        WallCorner corner2 = cornerExists(pos2);

        if (corner1 == nullCorner)
        {
            corner1 = Instantiate(wallCorner, pos1, Quaternion.identity).GetComponent<WallCorner>();
            corner1.gameObject.transform.parent = parent;
            corner1.GetComponent<BuildObject>().Anchor = pos1;
            corner1.gameObject.transform.localPosition = new Vector3(0,wallHeight,0) + corner1.gameObject.transform.localPosition;
            corner1.Initialise();
            corners.Add(corner1);
        }
        if (corner2 == nullCorner)
        {
            corner2 = Instantiate(wallCorner, pos2, Quaternion.identity).GetComponent<WallCorner>();
            corner2.gameObject.transform.parent = parent;
            corner2.GetComponent<BuildObject>().Anchor = pos2;
            corner2.gameObject.transform.localPosition = new Vector3(0, wallHeight, 0) + corner2.gameObject.transform.localPosition;
            corner2.Initialise();
            corners.Add(corner2);
        }

        WallMiddle middle = Instantiate(wallMiddle, Vector3.Lerp(pos1,pos2,0.5f), Quaternion.identity).GetComponent<WallMiddle>();
        middle.gameObject.transform.parent = parent;
        middle.GetComponent<BuildObject>().Anchor = Vector3.Lerp(pos1, pos2, 0.5f);
        middle.gameObject.transform.LookAt(pos1);
        middle.gameObject.transform.localPosition = new Vector3(0, wallHeight, 0) + middle.gameObject.transform.localPosition;
        middle.setCorners(corner1, corner2);
        middles.Add(middle);
        return true;
    }

    public void removeWall(GameObject wall)
    {
        WallMiddle temp = nullMiddle;
        foreach(WallMiddle mid in middles)
        {
            if(mid.gameObject == wall)
            {
                mid.corner1.removeConnector(mid);
                mid.corner2.removeConnector(mid);
                if(mid.corner1.connectors.Count == 0)
                {
                    removeCorner(mid.corner1);
                }
                if(mid.corner2.connectors.Count == 0)
                {
                    removeCorner(mid.corner2);
                }
                temp = mid;
            }
        }
        if(temp != nullObject)
        {
            middles.Remove(temp);
            Destroy(temp.gameObject);
        }
    }

    public void removeCorner(WallCorner corner)
    {
        corners.Remove(corner);
        Destroy(corner.gameObject);
    }

    WallCorner cornerExists(Vector3 pos)
    {
        foreach(WallCorner c in corners)
        {
            if(c.gameObject.GetComponent<BuildObject>().Anchor == pos)
            {
                return c;
            }
        }
        return nullCorner; 
    }

    bool areAdjacent(Vector3 corner1, Vector3 corner2)
    {
        if (Mathf.Abs(corner1.x - corner2.x) == 1 ^ Mathf.Abs(corner1.z - corner2.z) == 1)
        {
            return true;
        }
        return false;
    }
}
