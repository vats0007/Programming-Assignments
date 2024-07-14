using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseRayCast : MonoBehaviour
{
    //private PathFinding pathFinding;
    //Tag String
    private const string SELECTABLE_TAG = "Tile";
    //Tracking of currentHitObject
    [SerializeField]
    private Transform currentHitObject;
    private Tile selectedTile;

    private int selectedX = -1;
    private int selectedY = -1;

    // Start is called before the first frame update
    void Start()
    {
        //pathFinding = new PathFinding(10, 10, cube.gameObject, parent.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        //Taking Mouse Input
        Vector3 mousePos = Input.mousePosition;
        //Defining z for mousePoz
        mousePos.z = 100f;
        //mousePos thorugh main cam
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //Ray casted from main cam to the point mouse is pointing towards
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //hit stors the info about raycast
        RaycastHit hit;

        //setting to default if ray is not hitting
        if (currentHitObject != null)
        {
            //material color set to black and Ui is hidden if ray is not hitting current object
            var selectedObject = currentHitObject.transform;
            Tile selectedTile = selectedObject.GetComponent<Tile>();
            selectedTile.GetComponent<Renderer>().material.color = Color.black;
            selectedTile.GetCubeInfoUI().HideCubeInfoUI();
            //setting last object selected to null
            currentHitObject = null;
        }

        //ray casting and hit saves the info
        if (Physics.Raycast(ray, out hit, 100))
        {
            //ray casting for debug
            Debug.DrawRay(transform.position, mousePos - transform.position * hit.distance, Color.red);
            

            //selectedObject is object that is hit by ray
            var selectedObject = hit.transform;
            //checks if the hit has tag "Tile"
            if (selectedObject.CompareTag(SELECTABLE_TAG))
            {
                //gets tile component
                Tile selectedTile = selectedObject.GetComponent<Tile>();
                //if it is not null
                if (selectedTile != null)
                {
                    //set diff material and show UI element
                    selectedTile.GetComponent<Renderer>().material.color = Color.red;
                    selectedTile.GetCubeInfoUI().ShowCubeInfoUI();
                    //set current object to this object
                    currentHitObject = hit.transform;

                    selectedX = (int)selectedTile.GetCubeXPos();
                    selectedY = (int)selectedTile.GetCubeYPos();
                }
            }

        }
    }

    public int GetSelectedX()
    {
        return selectedX;
    }

    public int GetSelectedY()
    {
        return selectedY;
    }

}
