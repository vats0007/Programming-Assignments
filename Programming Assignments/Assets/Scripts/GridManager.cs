using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script will be attached to GameObject named GridManager
public class GridManager : MonoBehaviour
{
    private PathFinding pathFinding;
    //vars to make a grid
    public GameObject parent;
    public GameObject cube;
    public int rows;
    public int cols;
    public float cubeSize;
    private void Start()
    {
        //creating grid as object using Grid class
        pathFinding = new PathFinding(rows, cols,cubeSize,cube,parent);
        List<PathNode> path = pathFinding.FindPath(0, 0, 6, 6);
        
        foreach (PathNode node in path)
        {
            Debug.Log(node);
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            List<PathNode> path = pathFinding.FindPath(0, 0, 3, 4);
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(
                        new Vector3(path[i].x, path[i].y, 0f) * 10f + Vector3.one * 0.5f,
                        new Vector3(path[i + 1].x, path[i + 1].y, 0f) * 10f + Vector3.one * 0.5f,
                        Color.green
                    );
                }
            }
        }
    }


}