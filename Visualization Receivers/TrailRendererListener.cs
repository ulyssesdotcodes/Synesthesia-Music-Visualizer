using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class TrailRendererListener : AudioEventListener {
	public float resetTime;
	public bool upsideDown;

	TrailRenderer trailRenderer;

	void Start() {
		trailRenderer = GetComponent<TrailRenderer> ();
	}
	
	public override void ProcessAverageDBSample (float averageDbSample) {
		//Trail stuff
		float newTrailX = (transform.position.x - 15 - Time.deltaTime * 2) % 30 + 15;
		
		if (trailRenderer.time < 0f) {
			trailRenderer.time = resetTime;
		}

		if (newTrailX < -14.5f || newTrailX > 14.5f) {
			resetTime = trailRenderer.time;
			trailRenderer.time = -1.0f;
		}

		
		float newTrailY = Mathf.Lerp (0, 3f, averageDbSample) * (upsideDown ? -1 : 1);
		transform.position = new Vector3 (newTrailX, newTrailY + 2.5f, transform.position.z);
	}

    protected override float GetVolumeIncreaseRate()
    {
        return 10.0f;
    }

    protected override float GetVolumeDecreaseRate()
    {
        return 8.0f;
    }

    protected override float GetMaxAverageDecrease()
    {
        return 0.8f;
    }
}
