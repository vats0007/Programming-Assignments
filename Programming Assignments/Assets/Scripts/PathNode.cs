using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int y;

    public int gCost;
    //distance
    public int hCost;
    public int fCost;

    public PathNode cameFromNode;

    public bool isWalkable;

    public PathNode(Grid<PathNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }
    public PathNode(Grid<PathNode> grid, int x, int y,bool isWalkable)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        this.isWalkable = isWalkable;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }

    public int GetPX()
    {
        return x;
    }
    public int GetPY()
    {
        return y;
    }
}
