using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAI : IAI
{
    private PathFinding pathFinding;
    private List<PathNode> currentPath;
    private int currentPathIndex;
    public int endX;
    public int endY;
    public int startX;
    public int startY;

    //enemyAI constructor
    public EnemyAI(PathFinding pathFinding)
    {
        this.pathFinding = pathFinding;
        this.currentPath = new List<PathNode>();
        this.currentPathIndex = 0;
    }

    public void MoveTowardsPlayer(PathNode playerPosition, out int endX,out int endY)
    {
        //Initial Position for enemy
        startX = pathFinding.GetGrid().GetGridObject(9, 9).x;
        startY = pathFinding.GetGrid().GetGridObject(9, 9).y;

        //getting up/down/left/right nodes
        List<PathNode> adjacentNodes = GetNeighboursListOfPlayer(playerPosition);
        //finding closest node for enemy to travel
        PathNode closestNode = GetClosestNode(startX, startY, adjacentNodes);

        //if there is closest node
        if (closestNode != null)
        {
            //setting ends as that node
            endX = closestNode.x;
            endY = closestNode.y;
        }
        else
        {
            //staying at the starting position(returning)
            endX = startX;
            endY = startY;
        }
        //default set
        this.endX = endX;
        this.endY = endY;

    }

    //same code as player finding neighbours
    private List<PathNode> GetNeighboursListOfPlayer(PathNode currentNode)
    {
        List<PathNode> neighboursList = new List<PathNode>();

        //only giving up/down/left/right nods
        if (currentNode.x - 1 >= 0)
        {
            //left Node
            neighboursList.Add(pathFinding.GetNode(currentNode.x - 1, currentNode.y));
        }
        if (currentNode.x + 1 < pathFinding.GetGrid().GetWidth())
        {
            //right Node
            neighboursList.Add(pathFinding.GetNode(currentNode.x + 1, currentNode.y));
        }

        //down Node
        if (currentNode.y - 1 >= 0)
        {
            neighboursList.Add(pathFinding.GetNode(currentNode.x, currentNode.y - 1));
        }
        //up Node
        if (currentNode.y + 1 < pathFinding.GetGrid().GetHeight())
        {
            neighboursList.Add(pathFinding.GetNode(currentNode.x, currentNode.y + 1));
        }

        return neighboursList;
    }

    //getting closestNode
    private PathNode GetClosestNode(int startX, int startY, List<PathNode> nodes)
    {
        if (nodes.Count == 0)
            return null;

        PathNode closestNode = nodes[0];//setting one node as closest
        float closestDistance = Vector3.Distance(new Vector3(startX, 1, startY), new Vector3(closestNode.x, 1, closestNode.y));
        foreach (PathNode node in nodes)
        {
            float distance = Vector3.Distance(new Vector3(startX, 1, startY), new Vector3(node.x, 1, node.y));
            if (distance < closestDistance)
            {
                closestNode = node;
                closestDistance = distance;
            }
        }
        //returning closest node
        return closestNode;
    }


}
