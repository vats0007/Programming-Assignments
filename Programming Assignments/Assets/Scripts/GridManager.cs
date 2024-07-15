using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script will be attached to GameObject named GridManager
public class GridManager : MonoBehaviour
{
    //vars to make a grid
    private PathFinding pathFinding;
    public GameObject parent;
    public GameObject cube;
    public int rows;
    public int cols;
    public float cubeSize;
}