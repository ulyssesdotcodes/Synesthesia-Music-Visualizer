using UnityEngine;
using System.Collections;

public class MovingBasicObjectListener : BasicObjectListener {

    public float speed = 1f;
    public Vector3 direction = new Vector3(0, 1, 0);

    void FixedUpdate()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
	
}
