using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI LookAtCamera script can be used with all UI with worldPosition type
public class LookAtCamera : MonoBehaviour
{
    //Looking towards cam Modes
    private enum Mode
    {
        //Direct Look at camera
        LookAt,
        //Inverted Look at camera
        LookAtInverted,
        //Looking towards same forward as camera
        CameraForward,
        //Inverted and towards same forward as camera
        CameraForwardInverted
    }
    //show in inspector
    [SerializeField] private Mode mode;


    private void LateUpdate()
    {
        //Options
        switch (mode)
        {
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform.position);
                break;
            case Mode.LookAtInverted:
                //dir from camera
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}