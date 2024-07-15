using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//SO
[CreateAssetMenu(fileName = "ObstacleInfSO", menuName = "SOs/ObstacleInfoSO", order = 1)]
public class ObstacleInfoSO : ScriptableObject
{
    public int rows = 10;
    public int cols = 10;
    //storing and modefying Grid
    public bool[,] obstacleGrid = new bool[10,10];
    private bool[,] prevGrid = new bool[10,10];

    //updating prevGrid
    public void UpdatePrevGrid()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                prevGrid[x, y] = obstacleGrid[x, y];
            }
        }
    }

    //Setting Info when Chnaged
    public bool IsObsInfoChanged()
    {
        for(int x = 0; x < 10; x++)
        {
            for(int y = 0; y < 10; y++)
            {
                if (obstacleGrid[x,y] != prevGrid[x, y])
                {
                    EditorUtility.SetDirty(this);
                    return true;
                }
            }
        }
        return false;
    }
}
