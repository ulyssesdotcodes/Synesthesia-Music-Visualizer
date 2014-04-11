using UnityEngine;
using System.Collections;

public class ParticleSystemMoverListener : ParticleSystemListener {

    public Vector3 movementDirection;
    public Vector3 volumeDirection;
    public float loopDistance = 15f;
    public float movementMultiplier;

    private Vector3 basePosition;
    private Vector3 startPosition;

    protected override void Start()
    {
        base.Start();
        basePosition = transform.position;
        startPosition = transform.position;
    }

    public override void ProcessAverageDBSample(float averageDbSample)
    {
        base.ProcessAverageDBSample(averageDbSample);

        basePosition += movementDirection * Time.deltaTime;

        if (Vector3.Distance(basePosition, startPosition) > loopDistance)
        {
            Debug.Log(basePosition + ", " + startPosition);
            basePosition += 2 * (startPosition - basePosition);
        }

        transform.position = basePosition + volumeDirection * Mathf.Lerp(0f, 10f, averageDbSample * movementMultiplier);
    }

    public override int CalculateEmittedParticles(float averageSample)
    {
        return (int)((Mathf.Exp(averageSample * 4) - 1) * 2);
    }

    public override float CalculateParticleSpeed(float averageSample)
    {
        return averageSample * 10;
    }
}
