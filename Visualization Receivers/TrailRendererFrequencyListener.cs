using UnityEngine;
using System.Collections;

public class TrailRendererFrequencyListener : TrailRendererListener {
    public int frequencySpace = 0;
    public float yScale = 64;

    public override void OnAudioEvent(AudioVisualizer.AudioEvent audioEvent)
    {
        ProcessFrequencyDb(audioEvent.frequencySplitData[frequencySpace], audioEvent.averageDbSample);
    }

    // Override the regular process db sample to process the frequency db instead.
    public void ProcessFrequencyDb(float frequencyDb, float averageDbSample) {
        base.ProcessAverageDBSample(frequencyDb * averageDbSample * yScale);
    }

    // Don't do the regular processDBSample
    public override void ProcessAverageDBSample(float averageDbSample)
    {
        return;
    }
}
