using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //All the info that Player needed or can Have
    public PathFinding pathFinding = null;
    [SerializeField] private ObstacleInfoSO obstacleInfoSO;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private int endX, endY;
    [SerializeField] private int startX, startY;
    [SerializeField]private List<Vector3> pathVector;
    private List<PathNode> path;
    [SerializeField]private List<PathNode> obstacleNodes;
    [SerializeField] private List<Vector3> obstacleVector;
    [SerializeField] private MouseRayCast mouseRayCast;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float moveSpeed = 5f;
    private int currentPathIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //setting all the things that is needed
        transform.position = new Vector3(0, 1, 0);
        gridManager = FindAnyObjectByType<GridManager>();
        obstacleManager = FindAnyObjectByType<ObstacleManager>();
        pathVector = new List<Vector3>();
        obstacleNodes = new List<PathNode>();
        obstacleVector = new List<Vector3>();
        pathFinding = new PathFinding(gridManager.rows, gridManager.cols, gridManager.cubeSize, gridManager.cube, gridManager.parent);
    }

    // Update is called once per frame
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

            //setting obstacle nodes to avoid them from pathfinding
            if (obstacleNodes.Count == 0)
            {
                for (int y = 0; y < 10; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if (obstacleInfoSO.obstacleGrid[x, y])
                        {
                            Vector3 obstaclePosition = new Vector3(x, 1, y);
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
            
            //setting a pathVector on which player can travel
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
        //calulates which cube from grid is selected and using S we can set start position
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (mouseRayCast.GetSelectedX() != -1)
            {
                startX = mouseRayCast.GetSelectedX();
            }
            if (mouseRayCast.GetSelectedY() != -1)
            {
                startY = mouseRayCast.GetSelectedY();
            }
            Debug.Log(endX + " " + endY);
        }
        //using mouse click we can set end pos and also starts pathcalculations
        if (Input.GetMouseButtonDown(0))
        {
            if(mouseRayCast.GetSelectedX()!= -1)
            {
                endX = mouseRayCast.GetSelectedX();
            }
            if (mouseRayCast.GetSelectedY() != -1)
            {
                endY = mouseRayCast.GetSelectedY();
            }
            //pathfindings
            path = pathFinding.FindPath(startX, startY, endX, endY, obstacleNodes);

            if (path != null && path.Count > 0)
            {
                pathCalculations();
                currentPathIndex = 0;
                targetPosition = pathVector[currentPathIndex];
                isMoving = true;
            }
        }

        //After adding obstacles press space once
        path = pathFinding.FindPath(startX, startY, endX, endY, obstacleNodes);

        if (path != null && path.Count > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            pathCalculations();//pathCalcs
            currentPathIndex = 0;//currentPathIndex set to 0
            targetPosition = pathVector[currentPathIndex];//target is set to the currentPathIndex
            isMoving = true;//is moving set to true
        }
    }

    //moving Logic
    public void MoveOnPath()
    {
        //if there is a vector on which player can go
        if(currentPathIndex < pathVector.Count)
        {
            //target dir calc
            Vector3 direction = (targetPosition - transform.position).normalized;
            //this variable calculates the distance the object should move in the current frame.
            float distanceAtThisFrame = moveSpeed * Time.deltaTime;
            if (Vector3.Distance(transform.position, targetPosition) <= distanceAtThisFrame)
            {
                transform.position = targetPosition;
                currentPathIndex++;
                if (currentPathIndex < pathVector.Count)
                {
                    targetPosition = pathVector[currentPathIndex];//change target position to next index if once reached
                }
                else
                {
                    isMoving = false;//set is moving to false if distance is achieved
                }
            }
            else
            {
                transform.position += direction * distanceAtThisFrame;//move towards target postion
            }
        }
        
    }
}
