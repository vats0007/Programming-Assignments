using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    //basic costs for moving
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Grid<PathNode> grid;
    private List<PathNode> openList;
    private List<PathNode> closeList;

    public PathFinding(int width, int height,float cubeSize,GameObject cube,GameObject parent)
    {
        //creating grid as object using Grid class
        grid = new Grid<PathNode>(width, height, cubeSize, cube, parent, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));

    }

    public List<PathNode> FindPath(int startX,int startY, int endX,int endY,List<PathNode> obstacleNodes)
    {
        PathNode startNode =  grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);
        //Queuing for searching
        openList = new List<PathNode> { startNode };
        //Already Searched
        closeList = new List<PathNode>();

        //We have to set all nodes gCost = infi. and calculate their fCost
        for(int x = 0; x < grid.GetWidth(); x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                //set cameFrom back to null
                pathNode.cameFromNode = null;
            }
        }

        //Calulation for startNode Cost
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)
            {
                //Reached at Final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighboursList(currentNode))
            {
                if (closeList.Contains(neighbourNode)) continue;
                
                if (!neighbourNode.isWalkable)
                {
                    closeList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of Nodes of the openList
        return null;
    }

    private List<PathNode> GetNeighboursList(PathNode currentNode)
    {
        List<PathNode> neighboursList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) 
        {
            //left Node
            neighboursList.Add(GetNode(currentNode.x - 1, currentNode.y));

            //left Down
            if (currentNode.y - 1 >= 0)
            {
                neighboursList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            }

            //left Up
            if (currentNode.y + 1 < grid.GetHeight())
            {
                neighboursList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
            }
            
        }
        if(currentNode.x+1 < grid.GetWidth())
        {
            //right Node
            neighboursList.Add(GetNode(currentNode.x + 1, currentNode.y));

            //right Down
            if (currentNode.y - 1 >= 0)
            {
                neighboursList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            }

            //right Up
            if (currentNode.y + 1 < grid.GetHeight())
            {
                neighboursList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
            }
        }

        //down Node
        if (currentNode.y - 1 >= 0) 
        {
            neighboursList.Add(GetNode(currentNode.x, currentNode.y - 1));
        }
        if (currentNode.y + 1 < grid.GetHeight()) 
        {
            neighboursList.Add(GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighboursList;
    }

    public PathNode GetNode(int x,int y)
    {
        return grid.GetGridObject(x, y);
    }


    //adding paths in reverse order in list
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();

        path.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        //making path reverse so it can go from startNode to endNode
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for(int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    public Grid<PathNode> GetGrid()
    {
        return grid;
    }

    //public List<Vector3> FindPath(Vector3 startPos,Vector3 endPos)
    //{
    //    grid.GetXY(startPos, out int startX, out int startY);
    //    grid.GetXY(startPos, out int endX, out int endY);
    //}
}