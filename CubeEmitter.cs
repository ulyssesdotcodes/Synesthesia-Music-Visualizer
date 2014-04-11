using UnityEngine;
using System.Collections;

public class CubeEmitter : MonoBehaviour {
    public GameObject cubePrefab;

    public float emissionChancePerSecond;
    public float threshold = 1f;

    public AudioVisualizer av;

    private Bounds bounds;

    void Start()
    {
        bounds = transform.collider.bounds;
    }

	// Update is called once per frame
	void Update () {
        if (emissionChancePerSecond * Random.value * Time.deltaTime > threshold)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y), Random.Range(bounds.min.z, bounds.max.z));
            GameObject cube = (GameObject) Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            av.expandableAudioListeners.Add(cube.GetComponent<AudioEventListener>());
        }
	}
}
