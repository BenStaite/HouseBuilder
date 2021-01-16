using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWalls : MonoBehaviour
{
    public GameObject wallMiddle, wallCorner;
    public List<WallCorner> corners;
    public List<WallMiddle> middles;
    WallCorner nullCorner;


    public float wallThickness;
    public float wallHeight;
    // Start is called before the first frame update
    void Start()
    {
        corners = new List<WallCorner>();
        middles = new List<WallMiddle>();
    }

    public void addWall(Vector3 pos1, Vector3 pos2, Transform parent)
    {
        WallCorner corner1 = cornerExists(pos1);
        WallCorner corner2 = cornerExists(pos2);
        if (corner1 == nullCorner)
        {
            corner1 = new WallCorner(pos1, wallCorner, parent);
        }
        if (corner2 == nullCorner)
        {
            corner2 = new WallCorner(pos2, wallCorner, parent);
        }
        corners.Add(corner1);
        corners.Add(corner2);
        if (!areAdjacent(corner1, corner2)) return;
        WallMiddle middle = new WallMiddle(corner1, corner2, wallMiddle, parent);
        corner1.addConnector(middle);
        corner2.addConnector(middle);
    }

    WallCorner cornerExists(Vector3 pos)
    {
        foreach(WallCorner c in corners)
        {
            if(c.position == pos)
            {
                return c;
            }
        }
        return nullCorner; 
    }

    bool areAdjacent(WallCorner corner1, WallCorner corner2)
    {
        if (Mathf.Abs(corner1.position.x - corner2.position.x) == 1 ^ Mathf.Abs(corner1.position.z - corner2.position.z) == 1)
        {
            return true;
        }
        return false;
    }

    public class WallMiddle{
        public WallCorner corner1, corner2;
        public GameObject gameObject, prefab;

        public WallMiddle(WallCorner corner1, WallCorner corner2, GameObject prefab, Transform parent)
        {
            this.corner1 = corner1;
            this.corner2 = corner2;
            this.prefab = prefab;
            gameObject = Instantiate(prefab, Vector3.Lerp(corner1.position, corner2.position, 0.5f), Quaternion.identity);
            gameObject.transform.LookAt(corner1.position);
            gameObject.transform.parent = parent;
        }
    }

    public class WallCorner
    {
        public Vector3 position;
        public List<WallMiddle> connectors;
        public GameObject gameObject, prefab;

        public WallCorner(Vector3 position, GameObject prefab, Transform parent)
        {
            this.position = position;
            this.prefab = prefab;
            connectors = new List<WallMiddle>();
            gameObject = Instantiate(prefab, position, Quaternion.identity);
            gameObject.transform.parent = parent;
        }

        public void addConnector(WallMiddle connector)
        {
            connectors.Add(connector);
        }

        public void removeConnector(WallMiddle connector)
        {
            connectors.Remove(connector);
        }
    }
}
