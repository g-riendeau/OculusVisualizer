using UnityEngine;
using System.Collections;
using System;

public class AudioProcessor : MonoBehaviour {
	public float[] amplitudes;
	public Song song;

	void Awake(){
		amplitudes = new float[1024];
		Array.Clear(amplitudes, 0, amplitudes.Length);
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

			audio.GetSpectrumData (amplitudes, 0, FFTWindow.BlackmanHarris);

		// Pour fastForward la toune
		if (Input.GetKeyDown("f") && Time.timeScale < 30)		    {
			Time.timeScale = Time.timeScale* 2f;
			audio.pitch =Time.timeScale;
			Debug.Log(Time.timeScale);

			if (Time.timeScale > 2) {
				audio.mute=true;
			}
			else if (Time.timeScale <=2) {
				audio.mute=false;		
				
			}
	}
		// Pour Ralentir la toune
		if (Input.GetKeyDown("b") )		    {
			Time.timeScale =  Time.timeScale/ 2f;
			audio.pitch =  Time.timeScale;
			Debug.Log(Time.timeScale);
			if (Time.timeScale > 1) {
				audio.mute=true;
			}
			else if (Time.timeScale <=1) {
				audio.mute=false;						
			}

		}
	}

	public void startPlaying(){
		audio.Play ();
	}

}
