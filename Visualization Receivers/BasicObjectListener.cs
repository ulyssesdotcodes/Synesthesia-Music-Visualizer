using UnityEngine;
using System.Collections;

public class BasicObjectListener : AudioEventListener {

	public override void ProcessAverageDBSample (float averageSample) {
		float averageSampleCeiling = Mathf.Lerp (3, 10, averageSample);
		transform.localScale = new Vector3 (averageSampleCeiling, averageSampleCeiling, averageSampleCeiling);
	}
}
