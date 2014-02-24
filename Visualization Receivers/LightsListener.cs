using UnityEngine;
using System.Collections;

public class LightsListener : AudioEventListener {
	private Light[] lights;
	// Use this for initialization
	void Start () {
		lights = new Light[transform.childCount];
		int i = 0;
		foreach (Transform child in transform) {
			lights[i] = child.gameObject.GetComponent<Light> ();
			i++;
		}
	}
	
	public override void ProcessRepresentationalColor(Color color) {
		base.ProcessRepresentationalColor (color);
		foreach (Light light in lights) {
			light.color = color;
		}
	}

	public override void ProcessAverageDBSample(float averageSample) {
		foreach (Light light in lights) {
			light.intensity = Mathf.Lerp(0.01f, 0.1f, averageSample * 100);
		}
	}
}
