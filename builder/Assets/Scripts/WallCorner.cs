using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCorner : MonoBehaviour
{
    public List<WallMiddle> connectors;
    // Start is called before the first frame update

    public void Initialise()
    {
        connectors = new List<WallMiddle>();
    }

    public void removeConnector(WallMiddle mid)
    {
        connectors.Remove(mid);
    }

    public void addConnector(WallMiddle mid)
    {
        connectors.Add(mid);
    }
}
