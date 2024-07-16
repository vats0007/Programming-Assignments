using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // All the info that Enemy needed or can have
    private PathFinding pathFinding = null;
    //[SerializeField] private ObstacleInfoSO obstacleInfoSO;
    [SerializeField] private GridManager gridManager;
    //[SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private int endX, endY;
    [SerializeField] private int startX, startY;
    [SerializeField] private List<Vector3> pathVector;
    private List<PathNode> path;
    [SerializeField] private List<PathNode> obstacleNodes;
    [SerializeField] private List<Vector3> obstacleVector;
    public EnemyAI enemyAI;
    private Vector3 targetPosition;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private float moveSpeed = 4f;
    private int currentPathIndex = 0;
    private PathNode playerNode;

    private Vector3 lastPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Setting all the things that are needed
        transform.position = new Vector3(9, 0.9f, 9);
        startX = 9;
        startY = 9;
        gridManager = FindAnyObjectByType<GridManager>();
        //obstacleManager = FindAnyObjectByType<ObstacleManager>();
        pathVector = new List<Vector3>();
        obstacleNodes = new List<PathNode>();
        obstacleVector = new List<Vector3>();
        pathFinding = new PathFinding(gridManager.rows, gridManager.cols, gridManager.cubeSize, gridManager.cube, gridManager.parent);
        enemyAI = new EnemyAI(pathFinding);
        lastPlayerPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Finding player for playerPos
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        int playerGridX = (int)playerPosition.x;
        int playerGridY = (int)playerPosition.z;

        // If player has moved to a new position, recalculate the path
        if (playerPosition != lastPlayerPosition)
        {
            lastPlayerPosition = playerPosition;

            // Setting PlayerNode based on position and using Grid from pathFinding
            playerNode = new PathNode(pathFinding.GetGrid(), playerGridX, playerGridY);

            // MoveTowardsFunction call from enemyAI
            enemyAI.MoveTowardsPlayer(playerNode, out endX, out endY);

            // Calculate the path towards the new player position
            EnemyMovementCalculation();
        }

        // Start movements
        if (isMoving)
        {
            MoveOnPath();
        }
    }

    void EnemyMovementCalculation()
    {
        // Pathfinding code
        path = pathFinding.FindPath(startX, startY, endX, endY, obstacleNodes);
        if (path != null && path.Count > 0)
        {
            EnemyPathCalculation();
            currentPathIndex = 0;
            targetPosition = pathVector[currentPathIndex];
            isMoving = true;
            Debug.Log("Path found. Moving to first target: " + targetPosition);
        }
        else
        {
            Debug.Log("No valid path found.");
        }
    }

    void EnemyPathCalculation()
    {
        // Calculations same as player
        pathVector.Clear();
        obstacleNodes.Clear();
        obstacleVector.Clear();

        //if (obstacleNodes.Count == 0)
        //{
        //    for (int y = 0; y < 10; y++)
        //    {
        //        for (int x = 0; x < 10; x++)
        //        {
        //            if (obstacleInfoSO.obstacleGrid[x, y])
        //            {
        //                // Vector3 obstaclePosition = new Vector3(x, 1, y);
        //                PathNode obstacleNode = pathFinding.GetGrid().GetGridObject(x, y);
        //                if (obstacleNode.isWalkable)
        //                {
        //                    obstacleNode.isWalkable = false;
        //                }
        //                obstacleVector.Add(new Vector3(x, 0.9f, y));
        //            }
        //            else
        //            {
        //                PathNode obstacleNode = pathFinding.GetGrid().GetGridObject(x, y);
        //                if (!obstacleNode.isWalkable)
        //                {
        //                    obstacleNode.isWalkable = true;
        //                }
        //                obstacleVector.Remove(new Vector3(x, 0.9f, y));
        //            }
        //        }
        //    }
        //}

        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Vector3 newVector = new Vector3(path[i].x, 0.9f, path[i].y);
                if (!pathVector.Contains(newVector))
                {
                    pathVector.Add(newVector);
                }
            }
            foreach (PathNode node in path)
            {
                Debug.Log(node);
            }
        }
    }

    // Same as player MoveOnPath
    public void MoveOnPath()
    {
        if (currentPathIndex < pathVector.Count)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            float distanceAtThisFrame = moveSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, targetPosition) <= distanceAtThisFrame)
            {
                transform.position = targetPosition;
                currentPathIndex++;
                if (currentPathIndex < pathVector.Count)
                {
                    targetPosition = pathVector[currentPathIndex];
                    Debug.Log("Moving to next target: " + targetPosition);
                }
                else
                {
                    isMoving = false;
                    Debug.Log("Reached the end of path.");
                    // Update start position to the current position
                    startX = (int)transform.position.x;
                    startY = (int)transform.position.z;
                }
                // Change the color of the cube where the enemy lands
                ChangeColor(transform.position - Vector3.up, Color.black);
            }
            else
            {
                transform.position += direction * distanceAtThisFrame;
                Debug.Log("Moving towards target: " + targetPosition);
            }
        }
    }

    // Method to change the color of the cube at the given position
    void ChangeColor(Vector3 position, Color color)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f);
        foreach (Collider collider in colliders)
        {
            Renderer renderer = collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}
