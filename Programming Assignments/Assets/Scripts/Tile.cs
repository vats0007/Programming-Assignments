using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script will be attached to tile
public class Tile : MonoBehaviour
{
    //vars for tileInfo/cubeInfo
    [SerializeField] private float x, y;//position
    [SerializeField] private CubeInfoUI cubeInfoUI;

    private void Start()
    {
        //assigning x and y pos at start
        x = transform.position.x;
        y = transform.position.z;
    }
    //defining a function that returns the pos for the UI element
    public Vector2 GetCubePos()
    {
        x = transform.position.x;
        y = transform.position.z;
        return new Vector2(x, y);
    }
    //function returning x value
    public float GetCubeXPos()
    {
        return x;
    }
    //function returning y value
    public float GetCubeYPos()
    {
        return y;
    }
    //function returns CubeInfoUI Component
    public CubeInfoUI GetCubeInfoUI()
    {
        return cubeInfoUI;
    }
}
