using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemListener : AudioEventListener {


	private ParticleSystem particleSystem;
	private ParticleSystem[] subemitters;

	void Start() {
		particleSystem = GetComponent<ParticleSystem> ();

		int i = 0;
		subemitters = new ParticleSystem[transform.childCount];
		foreach (Transform child in transform) 
		{
			subemitters[i] = child.GetComponent<ParticleSystem>();
			i++;
		}
	}

	public override void ProcessAverageDBSample (float averageSample) {
        particleSystem.startSpeed = CalculateParticleSpeed(averageSample);
		particleSystem.Emit (CalculateEmittedParticles(averageSample));
	}

	public override void ProcessRepresentationalColor (Color color) {
		particleSystem.startColor = color;
	}

    public virtual int CalculateEmittedParticles(float averageSample)
    {
        return (int)(averageSample * 8);
    }

    public virtual float CalculateParticleSpeed(float averageSample)
    {
        return averageSample * 32;
    }

}
