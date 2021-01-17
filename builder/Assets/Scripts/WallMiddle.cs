using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMiddle : MonoBehaviour
{
    public WallCorner corner1;
    public WallCorner corner2;

    public void setCorners(WallCorner corner1, WallCorner corner2)
    {
        this.corner1 = corner1;
        corner1.addConnector(this);
        this.corner2 = corner2;
        corner2.addConnector(this);
    }
}
