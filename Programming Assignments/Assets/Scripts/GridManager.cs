using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script will be attached to GameObject named GridManager
public class GridManager : MonoBehaviour
{
    //vars to make a grid
    public GameObject cube;
    public int rows;
    public int cols;
    public float cubeSize;
    private void Start()
    {
        //creating grid as object using Grid class
        Grid grid = new Grid(rows, cols, cubeSize, cube);
    }
}