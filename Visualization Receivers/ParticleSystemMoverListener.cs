using UnityEngine;
using System.Collections;

public class ParticleSystemMoverListener : ParticleSystemListener {

    public override void ProcessAverageDBSample(float averageDbSample)
    {
        base.ProcessAverageDBSample(averageDbSample);
        //moving stuff
        float newTrailX = (transform.position.x + 15 + Time.deltaTime * 2) % 30 - 15;

        float newTrailY = Mathf.Lerp(0.5f, 5f, averageDbSample);
        transform.position = new Vector3(newTrailX, newTrailY, transform.position.z);
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
