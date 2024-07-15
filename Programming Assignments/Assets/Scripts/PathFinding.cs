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

    //used to create grid and setting each position as pathnode
    public PathFinding(int width, int height,float cubeSize,GameObject cube,GameObject parent)
    {
        //creating grid as object using Grid 
        grid = new Grid<PathNode>(width, height, cubeSize, cube, parent, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));

    }

    //Algorithm main function for finding the path
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
                pathNode.gCost = int.MaxValue;//setting g to infi.
                pathNode.CalculateFCost();
                //set cameFrom back to null
                pathNode.cameFromNode = null;
            }
        }

        //Calulation for startNode Cost
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);//b/w start end end node
        startNode.CalculateFCost();

        //start is already there
        while(openList.Count > 0)
        {
            //takes lowest to start with
            PathNode currentNode = GetLowestFCostNode(openList);
            if(currentNode == endNode)//setting current node to end node
            {
                //Reached at Final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);//remove a node from openList once it is visited
            closeList.Add(currentNode);// and add it to closeList

            foreach(PathNode neighbourNode in GetNeighboursList(currentNode))//traversing through neighbouring Nodes
            {
                if (closeList.Contains(neighbourNode)) continue;//if already visited then continue;
                
                if (!neighbourNode.isWalkable)//cheack if there any obstacle or not
                {
                    closeList.Add(neighbourNode);//if there is then add it to closeList and Continue
                    continue;
                }

                //This tentative g cost is a sum of the g cost of the current node and the cost to move from the current node to the neighboring node.
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if(tentativeGCost < neighbourNode.gCost)//if it is lesser then neighbourNode gCost
                {
                    neighbourNode.cameFromNode = currentNode;//then go to the neighbouring node(as it's cameFromNode is set to currentNode)
                    neighbourNode.gCost = tentativeGCost;//setting G as tent G
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);//calc hCost
                    neighbourNode.CalculateFCost();//calc fCost

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
        //list to return
        List<PathNode> neighboursList = new List<PathNode>();

        //Finding all 8 nodes that are near to player as neighbours
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

        return neighboursList;//returning neighboursList
    }

    //for other scripts to get Nodes
    public PathNode GetNode(int x,int y)
    {
        return grid.GetGridObject(x, y);
    }


    //adding paths in reverse order in list
    private List<PathNode> CalculatePath(PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();//creates list of pathnodes

        path.Add(endNode);//adding the node that is passed as arg
        PathNode currentNode = endNode;//temp currentNode set to endNode
        while(currentNode.cameFromNode != null)//if there is no came from node for current node that means it is starting node
        {
            //if there is cameFromNode then it is added to the list and currentNode is set to the cameFromNode
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        //making path reverse so it can go from startNode to endNode
        path.Reverse();
        return path;//returning Path array
    }

    
    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        //x and y dist calculations
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        //diff b/w them
        int remaining = Mathf.Abs(xDistance - yDistance);
        //returning total cost
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    //finding LowestFcost
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

    //for returning grid
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