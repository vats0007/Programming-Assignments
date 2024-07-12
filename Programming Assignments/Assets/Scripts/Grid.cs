using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject tile;//the base gameobject for a grid
    [SerializeField]
    private int rows, cols;//rows and cols 
    private float tileSize;//all 3 axis with same value h=w=l.
    private int[,] gridArray;//2D grid int array

    //Grid constructor for the class
    public Grid(int rows, int cols,float cubeSize,GameObject cube)//class named Grid
    {
        //setting values to vars
        this.rows = rows;
        this.cols = cols;
        this.tileSize = cubeSize;
        //Initializing gridArray
        gridArray = new int[rows, cols];

        Debug.Log(rows + " " + cols);
        //cycle thorugh all pos and creating cube at certain position
        for(int x = 0; x < gridArray.GetLength(0); x++)//cycle through rows
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)//cycle through cols
            {
                //adjusting scale for cube if needed
                cube.transform.localScale = Vector3.one * cubeSize;
                //Instantiating a cube
                Instantiate(cube, GetWorldPosition(x, y),Quaternion.identity);
            }
        }
    }

    //function to calculate position if diff cubeSize
    private Vector3 GetWorldPosition(int x,int y)
    {
        return new Vector3(x, 0, y) * tileSize;
    }
}
