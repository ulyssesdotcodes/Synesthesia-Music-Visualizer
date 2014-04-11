using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class TrailRendererListener : AudioEventListener {
	public float resetTime;
	public bool upsideDown;

	TrailRenderer trailRenderer;

    private Vector3 basePosition;

	void Start() {
		trailRenderer = GetComponent<TrailRenderer> ();
        basePosition = transform.localPosition;
	}
	
	public override void ProcessAverageDBSample (float averageDbSample) {
		//Trail stuff
		float newTrailX = (basePosition.x - 30 - Time.deltaTime * 2) % 60 + 30;
		
		if (trailRenderer.time < 0f) {
			trailRenderer.time = resetTime;
		}

		if (newTrailX < -29.5f || newTrailX > 29.5f) {
			resetTime = trailRenderer.time;
			trailRenderer.time = -1.0f;
		}

        basePosition.x = newTrailX;
		
		float newTrailY = Mathf.Lerp (0, 3f, averageDbSample) * (upsideDown ? -1 : 1);

        Vector3 realWorldPosition = transform.TransformPoint(basePosition) + Vector3.up * newTrailY;

        transform.localPosition = transform.InverseTransformPoint(realWorldPosition);
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
