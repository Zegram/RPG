using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCamera : MonoBehaviour {

    public void PanCameraToRight()
    {

        Camera.main.transform.RotateAround(Vector3.zero, Vector3.down, 90);
    }

    public void PanCameraToLeft()
    {
        Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 90);
    }

    public void PanCameraBack()
    {
        Camera.main.fieldOfView--;
    }

    public void PanCameraForward()
    {
        Camera.main.fieldOfView++;
    }
}
