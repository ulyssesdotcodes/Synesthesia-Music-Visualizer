using UnityEngine;
using System.Collections;

public class AudioEventListener :MonoBehaviour {

    protected float currentDbAverage;
    protected float maxAverage;

	public virtual void OnAudioEvent(AudioVisualizer.AudioEvent audioEvent) {
        // Allow the listener to be disabled without being removed from the visualizer controller
        if (!this.enabled)
        {
            Debug.Log(this.GetType().Name + " is disabled.");
            return;
        }

        // Calulate the current db sample. This will be different for different listeners because they may have
        // different rates of increase/decrease
        CalculateAverageDb(audioEvent.averageDbSample);

        // This is the default order of processing. Override OnAudioEvent to override this order
        // If there's no spectrum color, don't change color from the last thing.
        if (audioEvent.spectrumColorRepresetation.a != 0)
        {
            ProcessRepresentationalColor(audioEvent.spectrumColorRepresetation);
        }
		ProcessAverageDBSample (currentDbAverage);
		ProcessSpectrumSamples (audioEvent.spectrumData);
		ProcessDBSamples (audioEvent.dbData);
	}

	public virtual void ProcessAverageDBSample (float averageSample){ }

	public virtual void ProcessSpectrumSamples (float[] samples){ }

	public virtual void ProcessDBSamples (float[] samples){}
	
	public virtual void ProcessRepresentationalColor (Color color){}

    // The maximum rate of volume increase per frame
    protected virtual float GetVolumeIncreaseRate()
    {
        return 1.2f;
    }

    // The maximum rate of volume decrease per frame
    protected virtual float GetVolumeDecreaseRate()
    {
        return 0.4f;
    }

    // How much maxaverage decreases per frame if the max volume isn't achieved.
    protected virtual float GetMaxAverageDecrease()
    {
        return 0.5f;
    }

    protected void CalculateAverageDb(float averageDbSample)
    {
        // The max average db is the higher of the previous max and the current measurement
        maxAverage = Mathf.Max(maxAverage, averageDbSample);

        // Move the current average closer to max average
        if (currentDbAverage < maxAverage)
        {
            // If the current average is below, allow a large change per second
            currentDbAverage = Mathf.Clamp(currentDbAverage + GetVolumeIncreaseRate() * Time.deltaTime, currentDbAverage, maxAverage);
        }
        else
        {
            // If the current average is above, allow a small change per second
            currentDbAverage = Mathf.Clamp(currentDbAverage - GetVolumeDecreaseRate() * Time.deltaTime, maxAverage, currentDbAverage);
        }

        // Move the max allowed average down
        maxAverage -= Time.deltaTime * GetMaxAverageDecrease();
    }

    void OnDestroy()
    {
        AudioVisualizer av = GameObject.FindObjectOfType<AudioVisualizer>();
        if (av != null)
        {
            av.expandableAudioListeners.Remove(this);
        }
    }
}
