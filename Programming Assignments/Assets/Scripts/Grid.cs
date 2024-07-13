using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid<TGridObject> : MonoBehaviour
{
    public GameObject tile;//the base gameobject for a grid
    [SerializeField]
    private int rows, cols;//rows and cols 
    private float tileSize;//all 3 axis with same value h=w=l.
    private TGridObject[,] gridArray;//2D grid int array

    //Grid constructor for the class
    public Grid(int rows, int cols, float cubeSize, GameObject cube, GameObject parent, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)//class named Grid
    {
        //setting values to vars
        this.rows = rows;
        this.cols = cols;
        this.tileSize = cubeSize;
        //Initializing gridArray
        gridArray = new TGridObject[rows, cols];

        Debug.Log(rows + " " + cols);
        //cycle thorugh all pos and creating cube at certain position
        for(int x = 0; x < gridArray.GetLength(0); x++)//cycle through rows
        {
            for(int y = 0; y < gridArray.GetLength(1); y++)//cycle through cols
            {
                //adjusting scale for cube if needed
                cube.transform.localScale = Vector3.one * cubeSize;
                //Instantiating a cube
                Instantiate(cube, GetWorldPosition(x, y), Quaternion.identity, parent.transform);
            }
        }

        for (int x = 0; x < gridArray.GetLength(0); x++)//cycle through rows
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)//cycle through cols
            {
                gridArray[x, y] = createGridObject(this,x,y);
            }
        }
    }

    //function to calculate position if diff cubeSize
    private Vector3 GetWorldPosition(int x,int y)
    {
        return new Vector3(x, 0, y) * tileSize;
    }


    //Getters
    public void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPos.x / tileSize);
        y = Mathf.FloorToInt(worldPos.z / tileSize);
    }

    public TGridObject GetGridObject(int x,int y)
    {
        if(x>=0 && y>=0 && x<rows && y < cols)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetGridObject(x, y);
    }

    public int GetWidth()
    {
        return rows;
    }
    public int GetHeight()
    {
        return cols;
    }
}
