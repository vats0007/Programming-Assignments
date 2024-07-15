using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using OpenCover.Framework.Model;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
public class PathNode
{
    private Grid<PathNode> grid;
    public int x;
    public int y;

    /*The cost from the start node to the current node.
    This is the cumulative cost of traveling from the
    starting point to the current node, taking into account the specific path taken.*/
    public int gCost;


    /*The heuristic cost estimate from the current node to the goal node.
    This is an estimated cost, often calculated using methods such as
    the Manhattan distance, Euclidean distance, or other heuristics depending on the problem space.*/
    public int hCost;

    // The total cost of the node, which is the sum of the g and h cost
    public int fCost;
    public PathNode cameFromNode;
    public bool isWalkable;//for obstacle avoidence
    public PathNode(Grid<PathNode> grid, int x, int y)//constructor without bool
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
    }
    public PathNode(Grid<PathNode> grid, int x, int y,bool isWalkable)//constructor with bool
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
