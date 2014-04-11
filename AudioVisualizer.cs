using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AudioVisualizer : MonoBehaviour
{
    //An AudioSource object so the music can be played
    private AudioSource aSource;

    //A float array that stores the audio samples
    public float[] dbSamples;
    public float[] spectrumSamples;

    // The sample count for spectrum data. Need to be accessible on start from other scripts.
    public static int sSampleCount = 1024;

    // averageDbSample will be multiplied by this.
    public float volume;

    // Remember the last averages so we can limit the averageDb decrease
    private float maxAverage;

    // All the listeners. These will be sent an AudioEvent once it is calculated.
    public AudioEventListener[] audioListeners;
    public List<AudioEventListener> expandableAudioListeners;

    // Minimum value to be recognized for color calculation of frequencies
    public float minimumVolume = 0.005f;

    void Awake()
    {
        //Get and store a reference to the following attached components:
        //AudioSource
        this.aSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Initialize the variables that will be used to store audio data
        dbSamples = new float[256];
        spectrumSamples = new float[sSampleCount];

        expandableAudioListeners = new List<AudioEventListener>();
        foreach (AudioEventListener al in audioListeners)
        {
            expandableAudioListeners.Add(al);
        }
    }

    void Update()
    {
        // Obtain samples of output data over dbSamples.Length samples
        aSource.GetOutputData(this.dbSamples, 0);

        // Obtain the samples from the frequency bands of the attached AudioSource
        aSource.GetSpectrumData(this.spectrumSamples, 0, FFTWindow.BlackmanHarris);

        // Create an AudioEvent to be passed into the receivers.
        AudioEvent frameAudioEvent = new AudioEvent();
        frameAudioEvent.dbData = this.dbSamples;
        frameAudioEvent.spectrumData = this.spectrumSamples;

        float averageDbSample = 0.0f;

        // Take the RMS of the db samples and set it on the frameAudioEvent
        for (int i = 0; i < dbSamples.Length; i++)
        {
            averageDbSample += dbSamples[i] * dbSamples[i];
        }

        averageDbSample = Mathf.Sqrt(averageDbSample / dbSamples.Length);
        frameAudioEvent.averageDbSample = Mathf.Exp(-2f * averageDbSample) * averageDbSample * volume;

        // Calculate the color based on the samples data. Most of the samples after a certain index
        // are 0 so we stop 
        float maxValue = -1f;
        int maxIndex = 0;
        int maxImportantIndex = spectrumSamples.Length;
        for (int i = 0; i < spectrumSamples.Length; i++)
        {
            if (spectrumSamples[i] > maxValue)
            {
                maxIndex = i;
                maxValue = spectrumSamples[i];
            }
            if (spectrumSamples[i] * volume > minimumVolume)
            {
                maxImportantIndex = i;
            }
        }

        int currentFrequencySplit = 0;
        float[] frequencySplits = new float[4];
        for (int i = 0; i < maxIndex; i++)
        {
            currentFrequencySplit = (int)(4 * i / maxIndex);
            frequencySplits[currentFrequencySplit] += spectrumSamples[i] * spectrumSamples[i];
        }

        for (int i = 0; i < frequencySplits.Length; i++)
        {
            frequencySplits[i] = Mathf.Sqrt(frequencySplits[i]);
        }
        frameAudioEvent.frequencySplitData = frequencySplits;

        // Turn the int from 0 to maxValue into a color based on an hsv algorithm
        Color calculatedColor = new Color(0, 0, 0, 0);
        if (maxImportantIndex >= 6)
        {
            calculatedColor.a = 1;
            calculatedColor = intToColor(maxIndex, maxImportantIndex);
        }
        frameAudioEvent.spectrumColorRepresetation = calculatedColor;


        // Send the audio event to all the receivers
        foreach (AudioEventListener audioListener in expandableAudioListeners)
        {
            audioListener.OnAudioEvent(frameAudioEvent);
        }
    }

    // Convert an integer in a range from 0 to maxVal into a color
    private Color intToColor(int color, int maxVal)
    {
        int valPrime = maxVal / 6;
        float r = 0;
        float g = 0;
        float b = 0;
        float max = 1;
        float min = 0;
        int colorPrime = color / valPrime;

        switch (colorPrime)
        {
            case 0:
                r = max;
                g = colorPrime;
                b = min;
                break;

            case 1:
                r = colorPrime;
                g = max;
                b = min;
                break;

            case 2:
                r = min;
                g = max;
                b = colorPrime;
                break;

            case 3:
                r = min;
                g = colorPrime;
                b = max;
                break;

            case 4:
                r = colorPrime;
                g = min;
                b = max;
                break;

            case 5:
                r = max;
                g = min;
                b = colorPrime;
                break;
        }

        return new Color(r, g, b);
    }

    public class AudioEvent
    {
        public float averageDbSample;
        public float[] spectrumData;
        public float[] dbData;
        public Color spectrumColorRepresetation;
        public float[] frequencySplitData;
    }
}