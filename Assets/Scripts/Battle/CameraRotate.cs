using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour {


public static bool mouseDown;
public float timeMouseDown;
public Camera mainCamera = null;

	// Update is called once per frame
void Update()
{
    if (mouseDown)
    {
        timeMouseDown += Time.deltaTime;
    }

    if(mouseDown)
        mainCamera.transform.RotateAround(Vector3.zero, Vector3.up, 30 * Time.deltaTime);
}
 
void OnPointerDown(){
      mouseDown = true;
}
void OnPointerUp(){
      mouseDown = false;
      timeMouseDown = 0;
}
}
