using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {
    public float time = 5f;

    private float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - startTime > time)
        {
            Destroy(gameObject);
        }
	}
}
