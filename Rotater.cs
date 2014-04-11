using UnityEngine;
using System.Collections;

public class Rotater : MonoBehaviour {
    Quaternion rotation = new Quaternion();
    float currentRotation = 0.0f;
	
	// Update is called once per frame
	void Update () {
        currentRotation += Mathf.PI;
        rotation.eulerAngles = new Vector3(currentRotation, 0, 0);
        transform.rotation = rotation;
	}
}
