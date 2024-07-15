using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//script will be attached to UI object of a tile
public class CubeInfoUI : MonoBehaviour
{
    //Reference for Text and Tile
    [SerializeField] private TextMeshProUGUI posText;
    [SerializeField] private Tile cube;

    private void Start()
    {
        //setting the position as text
        posText.text = "X : " + cube.GetCubeXPos() + "\n,Y : " + cube.GetCubeYPos();
        //Initial hiding of UI
        HideCubeInfoUI();
    }
    //Activate UI
    public void ShowCubeInfoUI()
    {
        gameObject.SetActive(true);
    }
    //Deactivate UI
    public void HideCubeInfoUI()
    {
        gameObject.SetActive(false);
    }
}
