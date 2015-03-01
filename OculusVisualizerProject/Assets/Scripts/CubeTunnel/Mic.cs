using UnityEngine;
using System.Collections;

public class Mic : MonoBehaviour {
	
	public string selectedDevice { get; private set; }	
	private bool micSelected = false;
	private int minFreq, maxFreq; 
	public float loudness { get; private set; } //dont touch
	public float sourceVolume = 100;//Between 0 and 100
	public float sensitivity = 100;
	public float ramFlushSpeed = 5;//The smaller the number the faster it flush's the ram, but there might be performance issues...
	private int amountSamples = 256; //increase to get better average, but will decrease performance. Best to leave it

	// Use this for initialization
	void Start () {
		audio.loop = true; // Set the AudioClip to loop
		audio.mute = true; // Mute the sound, we don't want the player to hear it
		selectedDevice = Microphone.devices[0].ToString();
		micSelected = true;

		GetMicCaps();
	}
	
	// Update is called once per frame
	void Update () {
		audio.volume = (sourceVolume/100);
		loudness = 1000 * GetAveragedVolume() * sensitivity * (sourceVolume/10);

		if (!Microphone.IsRecording(selectedDevice)) 
			StartMicrophone();
	}
	
	public void StartMicrophone () {
		audio.clip = Microphone.Start(selectedDevice, true, 10, maxFreq);//Starts recording
		while (!(Microphone.GetPosition(selectedDevice) > 0)){} // Wait until the recording has started
		audio.Play(); // Play the audio source!
	}

	public void GetMicCaps () {
		Microphone.GetDeviceCaps(selectedDevice, out minFreq, out maxFreq);//Gets the frequency of the device
		if ((minFreq + maxFreq) == 0)//These 2 lines of code are mainly for windows computers
			maxFreq = 44100;
	}

	float GetAveragedVolume() {
		float[] data = new float[amountSamples];
		float a = 0;
		audio.GetOutputData(data,0);
		foreach(float s in data) {
			a += Mathf.Abs(s);
		}
		return a/amountSamples;
	}
}
