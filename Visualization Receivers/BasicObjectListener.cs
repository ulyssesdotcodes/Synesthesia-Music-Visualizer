using UnityEngine;
using System.Collections;

public class BasicObjectListener : AudioEventListener {

    public float minSize = 3f;
    public float maxSize = 10f;

	public override void ProcessAverageDBSample (float averageSample) {
		float averageSampleCeiling = Mathf.Lerp (minSize, maxSize, averageSample);
		transform.localScale = new Vector3 (averageSampleCeiling, averageSampleCeiling, averageSampleCeiling);
	}
}
