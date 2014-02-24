using UnityEngine;
using System.Collections;

public class WaveListener : AudioListener {

	public LineRenderer[] lineRenderers;

	// Scale of frequency-based motion on the x and y axis
	public float xScale = 1f;
	public float yScale = 1f;


	// Speed of the sine waves
	public float speedOffset = 8f;
	// Scale of the sine waves
	public float scaleOffset;
	// Number of points along lineRenderer it takes for the sine wave to fade out
	public float sineFade;

	// Target value for sine amplitude multiplier
	private float mMax;
	// Current value for sine amplitude multiplier
	private float mCur = 0f;
	// Minimum amplitude multiplier of sine waves
	public float minSin;

	// Target frequency value per vertex
	private float[] fMax;
	// Current frequency value per vertex
	private float[] fCur;

	void Start() {
		fMax = new float[AudioVisualizer.sSampleCount];
		fCur = new float[AudioVisualizer.sSampleCount];

		foreach(LineRenderer l in lineRenderers){
			l.SetVertexCount (AudioVisualizer.sSampleCount);
			l.useWorldSpace = false;
		}
	}

	public override void OnAudioEvent(AudioVisualizer.AudioEvent audioEvent) {
		/*
		 * Transform sine amplitude multiplier towards target
		 * Multiplier is based on the first frequency datapoint as well as the averageDbVolume
	 	 */

		// Compare the current target with the new data point
		mMax = Mathf.Max (mMax, audioEvent.averageDbSample * 0.5f + audioEvent.spectrumData [0] * 0.5f);
		// Get the direction of difference between the target and current
		float diff = Mathf.Sign (mMax - mCur);
		// Transform towards target
		mCur = Mathf.Clamp (mCur + diff * Time.deltaTime * 5, minSin, mMax);

		// If we're above the target, then lower the current
		if (mCur >= mMax) {
			mMax -= Time.deltaTime;
		}

		// Set the multiplier (a sine to add to the pulsing effect)
		float sc = Mathf.Sin (Time.time * speedOffset) * mCur * scaleOffset;

		// Iterate through each sample and linerenderer point
		for (int i = 0; i < AudioVisualizer.sSampleCount; i++) {
			// Set distance along x axis based on scale
			float x = xScale * i;

			// Calculate y based on scale, volume, and current frequency data
			float y = audioEvent.spectrumData[i] * audioEvent.averageDbSample * 2 * yScale;

			// Transform the y value towards the target. We want the highest point, based on current data and last data.
			fMax[i] = Mathf.Max(fMax[i], y);

			// Transform towards the target. This will clamp between fMax[i] and fCur[i]
			if(fCur[i] > fMax[i]) {
				fCur[i] = Mathf.Clamp(fCur[i] - Time.deltaTime * 60 * audioEvent.averageDbSample * 5, fMax[i], fCur[i]);
			} else {
				fCur[i] = Mathf.Clamp(fCur[i] + Time.deltaTime * 100 * audioEvent.averageDbSample * 5, fCur[i], fMax[i]);
			}

			// Lower the max over time since the last time fMax was set
			fMax[i] -= Time.deltaTime * 60;

			y = fCur[i];

			/*
			 * Apply the sine wave
			 */
			float segval = (sineFade - i) / sineFade;
			// Minimum value
			segval = Mathf.Max(segval, .15f);

			// Get sine based on multiplier
			float y2 = Mathf.Sin(x + Time.time * speedOffset) * (segval * sc);
			y += y2;

			// Apply the calculated position to the current point in LineRenderer
			foreach(LineRenderer l in lineRenderers) {
				l.SetPosition(i, new Vector3(x, y, 0));
			}
		}
		// Cycle material over time
		foreach (LineRenderer l in lineRenderers) {
			Vector2 mainTextureOffset = l.gameObject.renderer.material.mainTextureOffset;
			mainTextureOffset.x -= Time.deltaTime * 20 * mCur;
			l.gameObject.renderer.material.mainTextureOffset = mainTextureOffset;
		}
	}
}