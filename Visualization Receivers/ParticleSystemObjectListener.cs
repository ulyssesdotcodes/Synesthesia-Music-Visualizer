using UnityEngine;
using System.Collections;

public class ParticleSystemObjectListener : ParticleSystemListener {

	public override void ProcessAverageDBSample(float averageDbSample) {
		base.ProcessAverageDBSample (averageDbSample);

		float averageSampleScale = Mathf.Lerp (0.5f, 3, averageDbSample);
		
		transform.localScale = new Vector3 (averageSampleScale, averageSampleScale, averageSampleScale);
	}

	public override void ProcessRepresentationalColor (Color color) {
		base.ProcessRepresentationalColor (color);
		renderer.material.color = color;
	}

    protected override float GetMaxAverageDecrease()
    {
        return 0.8f;
    }
}
