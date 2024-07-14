using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PathFinding pathFinding = null;
    [SerializeField] private ObstacleInfoSO obstacleInfoSO;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private int endX, endY;
    [SerializeField] private int startX, startY;
    [SerializeField] private List<Vector3> pathVector;
    private List<PathNode> path;
    [SerializeField] private List<PathNode> obstacleNodes;
    [SerializeField] private List<Vector3> obstacleVector;
    public EnemyAI enemyAI;

    private Vector3 targetPosition;
    [SerializeField]
    private bool isMoving = false;
    private float moveSpeed = 4f;
    private int currentPathIndex = 0;
    private PathNode playerNode;
    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(9, 1, 9);
        startX = 9; 
        startY = 9; 
        gridManager = FindAnyObjectByType<GridManager>();
        obstacleManager = FindAnyObjectByType<ObstacleManager>();
        pathVector = new List<Vector3>();
        obstacleNodes = new List<PathNode>();
        obstacleVector = new List<Vector3>();
        pathFinding = new PathFinding(gridManager.rows, gridManager.cols, gridManager.cubeSize, gridManager.cube, gridManager.parent);
        enemyAI = new EnemyAI(pathFinding);
        

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        int playerGridX =(int)playerPosition.x;
        int playerGridY = (int)playerPosition.z;
        playerNode = new PathNode(pathFinding.GetGrid(), playerGridX, playerGridY);

        enemyAI.MoveTowardsPlayer(playerNode, out endX, out endY);

        if (Input.GetKeyDown(KeyCode.A))
        {
            EnemyMovementCalculation();
        }
        

        if (isMoving)
        {
            MoveOnPath();
            if (!isMoving)
            {
                startX = endX;
                startY = endY;
            }
        }
    }

    void EnemyMovementCalculation()
    {

        //if (Input.GetKeyDown(KeyCode.A))
        
            path = pathFinding.FindPath(startX, startY, endX, endY, obstacleNodes);
            if (path != null && path.Count > 0)
            {
                EnemyPathCalculation();
                currentPathIndex = 0;
                targetPosition = pathVector[currentPathIndex];
                isMoving = true;
            }
        
        //else
        //{
        //    isMoving = false;
        //}

    }

    void EnemyPathCalculation()
    {

        pathVector.Clear();
        obstacleNodes.Clear();
        obstacleVector.Clear();

        if (obstacleNodes.Count == 0)
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (obstacleInfoSO.obstacleGrid[x, y])
                    {
                        //Vector3 obstaclePosition = new Vector3(x, 1, y);
                        PathNode obstacleNode = pathFinding.GetGrid().GetGridObject(x, y);
                        if (obstacleNode.isWalkable)
                        {
                            obstacleNode.isWalkable = false;
                        }
                        obstacleVector.Add(new Vector3(x, 1, y));
                    }
                    else
                    {
                        PathNode obstacleNode = pathFinding.GetGrid().GetGridObject(x, y);
                        if (!obstacleNode.isWalkable)
                        {
                            obstacleNode.isWalkable = true;
                        }
                        obstacleVector.Remove(new Vector3(x, 1, y));
                    }
                }
            }
        }


        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Vector3 newVector = new Vector3(path[i].x, 1, path[i].y);
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
                }
                else
                {

                    isMoving = false;
                }
            }
            else
            {
                transform.position += direction * distanceAtThisFrame;
            }
        }

    }

    //void UpdateObstacleNodes()
    //{
    //    for (int y = 0; y < gridManager.rows; y++) 
    //    {
    //        for (int x = 0; x < gridManager.cols; x++)
    //        {
    //            PathNode node = pathFinding.GetGrid().GetGridObject(x, y);
    //            if (obstacleInfoSO.obstacleGrid[x, y])
    //            {
    //                node.isWalkable = false;
    //                obstacleNodes.Add(node);
    //            }
    //            else
    //            {
    //                node.isWalkable = true;
    //            }
    //        }
    //    }
    //}
}
