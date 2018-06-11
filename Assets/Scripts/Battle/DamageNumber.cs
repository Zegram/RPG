using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour {

    public float floatTime = 2f;
    public float time = 0f;
	
	// Update is called once per frame
	void Update () 
    {
        time += Time.deltaTime;

        transform.Translate(Vector3.up * Time.deltaTime, Space.World);
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0f, 180f, 0f));

        if (time > floatTime)
            Destroy(gameObject);

	}
}
