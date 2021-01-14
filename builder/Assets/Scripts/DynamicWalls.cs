using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWalls : MonoBehaviour
{
    List<Chain> allChains;
    public float wallThickness;
    public float wallHeight;
    // Start is called before the first frame update
    void Start()
    {
        allChains = new List<Chain>();
    }

    public void addChain(List<Vector3> positions)
    {
        List<WallNode> nodes = new List<WallNode>();
        foreach(Vector3 pos in positions)
        {
            nodes.Add(new WallNode(pos));
        }
        Chain newChain = new Chain(nodes[0]);
        newChain.nodes = nodes;
        newChain.updateVerts();
    }

    public void addWall(Vector3 position)
    {
        WallNode wall = new WallNode(position);
        if(allChains.Count == 0)
        {
            allChains.Add(new Chain(wall));
        }
        else
        {
            if (!findNeighbour(wall))
            {
                allChains.Add(new Chain(wall));
            }
        }
    }

    bool findNeighbour(WallNode wall)
    {
        foreach(Chain chain in allChains)
        {
            foreach(WallNode w in chain.nodes)
            {
                if (areAdjacent(wall, w)){
                    w.addNeighbour(wall);
                    wall.addNeighbour(w);
                    chain.addNode(wall);
                    return true;
                }
            }
        }
        return false;
    }

    bool areAdjacent(WallNode wall1, WallNode wall2)
    {
        if(Mathf.Abs(wall1.position.x - wall2.position.x) == 1)
        {
            return true;
        }
        else if (Mathf.Abs(wall1.position.z - wall2.position.z) == 1f)
        {
            return true;
        }
        return false;
    }

    class Chain
    {
        public List<WallNode> nodes;
        public WallNode head;
        public GameObject wallObject;

        public Chain(WallNode head)
        {
            this.head = head;
            nodes = new List<WallNode>();
            wallObject = new GameObject();
        }

        public void addNode(WallNode wall)
        {
            nodes.Add(wall);
            updateVerts();
        }

        public void updateVerts()
        {
            Destroy(wallObject);
            wallObject = new GameObject("Wall");
            MeshFilter meshFilter = (MeshFilter)wallObject.AddComponent(typeof(MeshFilter));
            Mesh mesh = new Mesh();
            List<Vector3> verts = new List<Vector3>();
            foreach (WallNode node in nodes)
            {
                
            }
        }
    }


    class WallNode
    {
        public Vector3 position;
        public List<WallNode> neighbours;

        public WallNode(Vector3 position)
        {
            this.position = position;
            neighbours = new List<WallNode>();
        }

        public void addNeighbour(WallNode neighbour)
        {
            neighbours.Add(neighbour);
        }

        public void removeNeighbour(WallNode neighbour)
        {
            neighbours.Remove(neighbour);
        }
    }
}
