using UnityEngine;
using System;  // Needed for Math

public class FrequencyChange : MonoBehaviour
{
	// un-optimized version
	public double frequency = 440;
	public double frequency_base = 440;
	public double gain = 0.05;
	public double range = 200.0;

	private double increment;
	private double phase;
	private double sampling_frequency = 44100;
	private int direction = +1;
	private double freqChange;

    private double volt_freq=0;

	void OnAudioFilterRead(float[] data, int channels)
	{
        /*
		//update frequency (linear increase and then decrease, etc)
		freqChange = direction*10;
		frequency = frequency + freqChange;
		if (frequency > frequency_base + range || frequency  < frequency_base - range)
			direction = -direction; 

        */

        frequency = volt_freq*frequency_base;

		// update increment in case frequency has changed
		increment = frequency * 2 * Math.PI / sampling_frequency;
		for (var i = 0; i < data.Length; i = i + channels)
		{
			phase = phase + increment;
			// this is where we copy audio data to make them “available” to Unity
			data[i] = (float)(gain*Math.Sin(phase));
			// if we have stereo, we copy the mono data to each channel
			if (channels == 2) data[i + 1] = data[i];
			if (phase > 2 * Math.PI) phase = 0;
		}
	}

    public void PassData(double freqVal)
    {
        volt_freq = freqVal;
    }
} 

