using UnityEngine;
using System.Collections;

public class SpeakerListener : AudioEventListener {

    public Vector3 movementDirection = Vector3.up;

    private Vector3 startLocation;

    void Start()
    {
        startLocation = transform.position;
    }

    public override void ProcessAverageDBSample(float averageSample)
    {
        transform.position = startLocation + movementDirection * averageSample * 10;
    }
	
}
