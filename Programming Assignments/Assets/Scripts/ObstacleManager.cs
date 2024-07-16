//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ObstacleManager : MonoBehaviour
//{
//    [SerializeField]
//    private ObstacleInfoSO obstacleInfoSO;
//    [SerializeField]
//    private GameObject obstaclePrefab;
//    public GameObject parent;
//    private GameObject[,] obstacles = new GameObject[10, 10];

//    private void Start()
//    {
//        GenerateObstacles();
//        //init store
//        obstacleInfoSO.UpdatePrevGrid();
//    }
//    private void Update()
//    {
//        if (obstacleInfoSO.IsObsInfoChanged())
//        {
//            GenerateObstacles();
//            //update prevGrid
//            obstacleInfoSO.UpdatePrevGrid();
//        }
//    }

//    private void GenerateObstacles()
//    {
//        DestroyExistingObstacles();//destroyExisting so duplications can not be occur
//        for (int y = 0; y < 10; y++)
//        {
//            for (int x = 0; x < 10; x++)
//            {
//                if (obstacleInfoSO.obstacleGrid[x, y])//if there is a SO with Info
//                {
//                    //Debug.Log(obstacleInfoSO.obstacleGrid[x, y]);
//                    if (obstacles[x, y] == null)
//                    {
//                        Vector3 obsPos = new Vector3(x, 1, y);//setting vecs
//                        //instantiating obs
//                        obstacles[x, y] = Instantiate(obstaclePrefab, obsPos, Quaternion.identity,parent.transform);
//                    }
//                }
//                else
//                {
//                    if (obstacles[x, y] != null)
//                    {
//                        Destroy(obstacles[x, y]);
//                        obstacles[x, y] = null;
//                    }
//                }
//            }
//        }
//    }

//    //destroy if there is anything on that position
//    private void DestroyExistingObstacles()
//    {
//        for (int y = 0; y < 10; y++)
//        {
//            for (int x = 0; x < 10; x++)
//            {
//                if (obstacles[x, y] != null)
//                {
//                    Destroy(obstacles[x, y]);
//                    obstacles[x, y] = null;
//                }
//            }
//        }
//    }

//    //VectorToPathNode Getter
//    public PathNode VectorToPathNode(Vector3 vector,Grid<PathNode> grid)
//    {
//        PathNode pathnode = new PathNode(grid, ((int)vector.x), ((int)vector.z), false);
//        return pathnode;
//    }
//}