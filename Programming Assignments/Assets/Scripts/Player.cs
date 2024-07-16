using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private const string LEVEL_2 = "Level 2";
    public PathFinding pathFinding = null;
    //[SerializeField] private ObstacleInfoSO obstacleInfoSO;
    [SerializeField] private GridManager gridManager;
    //[SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private int endX, endY;
    [SerializeField] private int startX, startY;
    [SerializeField] private List<Vector3> pathVector;
    private List<PathNode> path;
    [SerializeField] private List<PathNode> obstacleNodes;
    [SerializeField] private List<Vector3> obstacleVector;
    [SerializeField] private MouseRayCast mouseRayCast;
    private GameManager gameManager;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float moveSpeed = 5f;
    private int currentPathIndex = 0;

    void Start()
    {
        transform.position = new Vector3(0, 1, 0);
        gridManager = FindAnyObjectByType<GridManager>();
        //obstacleManager = FindAnyObjectByType<ObstacleManager>();
        gameManager = FindAnyObjectByType<GameManager>();
        pathVector = new List<Vector3>();
        obstacleNodes = new List<PathNode>();
        obstacleVector = new List<Vector3>();
        pathFinding = new PathFinding(gridManager.rows, gridManager.cols, gridManager.cubeSize, gridManager.cube, gridManager.parent);
        if(SceneManager.GetActiveScene().name == LEVEL_2)
        {
            SetUpObstacles(1, 1);
            SetUpObstacles(2, 2);
            SetUpObstacles(3, 3);
            SetUpObstacles(4, 4);
            SetUpObstacles(5, 5);
            SetUpObstacles(6, 6);
            SetUpObstacles(7, 7);
            SetUpObstacles(8, 8);
            SetUpObstacles(9, 9);
            SetUpObstacles(1, 2);
            SetUpObstacles(1, 3);
            SetUpObstacles(1, 4);
            SetUpObstacles(2, 1);
            SetUpObstacles(3, 1);
            SetUpObstacles(4, 1);
            SetUpObstacles(3, 4);
            SetUpObstacles(3, 5);
            SetUpObstacles(5, 3);
            SetUpObstacles(5, 3);
        }
    }

    void SetUpObstacles(int a, int b)
    {
        //for (int y = 0; y < gridManager.rows; y++)
        //{
        //    for (int x = 0; x < gridManager.cols; x++)
        //    {
        //        if (obstacleInfoSO.obstacleGrid[x, y])
        //        {
        //            PathNode obstacleNode = pathFinding.GetGrid().GetGridObject(a, b);
        //            if (obstacleNode.isWalkable)
        //            {
        //                obstacleNode.isWalkable = false;
        //            }
        //            obstacleVector.Add(new Vector3(a, 1, b));
        //        }
        //    }
        //}
    }

    void Update()
    {
        playerMovementCalc();
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

    public void pathCalculations()
    {
        pathVector.Clear();
        obstacleNodes.Clear();
        obstacleVector.Clear();

        //for (int y = 0; y < gridManager.rows; y++)
        //{
        //    for (int x = 0; x < gridManager.cols; x++)
        //    {
        //        if (obstacleInfoSO.obstacleGrid[x, y])
        //        {
        //            Vector3 obstaclePosition = new Vector3(x, 1, y);
        //            PathNode obstacleNode = pathFinding.GetGrid().GetGridObject(x, y);
        //            if (obstacleNode.isWalkable)
        //            {
        //                obstacleNode.isWalkable = false;
        //            }
        //            obstacleVector.Add(new Vector3(x, 1, y));
        //        }
        //        else
        //        {
        //            PathNode obstacleNode = pathFinding.GetGrid().GetGridObject(x, y);
        //            if (!obstacleNode.isWalkable)
        //            {
        //                obstacleNode.isWalkable = true;
        //            }
        //            obstacleVector.Remove(new Vector3(x, 1, y));
        //        }
        //    }
        //}

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

    void playerMovementCalc()
    {
        if (isMoving) return;

        if (mouseRayCast.GetSelectedX() != -1)
        {
            endX = mouseRayCast.GetSelectedX();
        }
        if (mouseRayCast.GetSelectedY() != -1)
        {
            endY = mouseRayCast.GetSelectedY();
        }
        if (Input.GetMouseButtonDown(0))
        {
            path = pathFinding.FindPath(startX, startY, endX, endY, obstacleNodes);

            if (path != null && path.Count > 0)
            {
                pathCalculations();
                currentPathIndex = 0;
                targetPosition = pathVector[currentPathIndex];
                isMoving = true;
            }
        }

        path = pathFinding.FindPath(startX, startY, endX, endY, obstacleNodes);

        if (path != null && path.Count > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            pathCalculations();
            currentPathIndex = 0;
            targetPosition = pathVector[currentPathIndex];
            isMoving = true;
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

                // Change the color of the cube where the player lands
                ChangeColor(transform.position - Vector3.up, Color.red);
            }
            else
            {
                transform.position += direction * distanceAtThisFrame;
            }
        }
    }

    void ChangeColor(Vector3 position, Color color)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.3f);
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
