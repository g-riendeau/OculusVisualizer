using UnityEngine;
using System.Collections;
using System;

public class SongProcessor : AudioProcessor
{
	public AudioSource mainSong; 
	public AudioSource outsideSong;
	public AudioClip otherOutsideSong;
	private bool mainSongFadeOut = false;
	private bool mainSongFadeIn=false;
	private bool outsideSongFadeOut = false;
	private bool outsideSongFadeIn = false;

	// Use this for initialization
	void Start ()
	{
		mainSong.volume = 1f;
		outsideSong.volume = 0.0f;
	}
	
	// Update is called once per frame
	public override void FixedUpdate ()
	{
		mainSong.GetSpectrumData (amplitudes, 0, FFTWindow.BlackmanHarris);
		//Accelerer
		if (Input.GetKeyDown ("f")) {
			Time.timeScale = 8f;
			mainSong.pitch = 8f;
			outsideSong.pitch = 8f;
			//mainSong.mute = true;	
			//outsideSong.mute = true;
			Debug.Log ("x8");
		}
		// Pour Ralentir la toune
		if (Input.GetKeyDown ("b")) {
			Time.timeScale = 1f;
			mainSong.pitch = 1f;
			outsideSong.pitch = 1f;

			//mainSong.mute = false;	
			//outsideSong.mute = false;	
			Debug.Log ("Normal speed");
		}

		if (mainSongFadeOut){
			fadeOut(ref mainSong,ref mainSongFadeOut );
		}

		if (outsideSongFadeOut){
			fadeOut(ref outsideSong, ref outsideSongFadeOut);
		}

		if (mainSongFadeIn){
			fadeIn(ref mainSong, ref mainSongFadeIn);
		}

		if (outsideSongFadeIn){
			fadeIn(ref outsideSong, ref outsideSongFadeIn);
		}
	}
	public void startPlaying (){
		mainSong.Play ();
	}

	public void startOutsideSong(bool play){
		if (play)
			outsideSong.Play ();
		else
			outsideSong.Stop ();
	}

	public void leaveTunnel(){
		mainSongFadeOut = true;
		outsideSongFadeIn = true;
		//Debug.Log("Leaving tunnel!!");
	}
	public void enterTunnel(){
		mainSongFadeIn = true;
		outsideSongFadeOut = true;
	}

	public void setOutsideSongTime(float theTime)
	{
		outsideSong.time = theTime;
	}

	public void changeOutsideSong()
	{
		outsideSong.clip = otherOutsideSong;
		outsideSong.Play();
	}

	private void fadeOut(ref AudioSource audio, ref bool continu){
		if(audio.volume > 0.01f){
			audio.volume = Mathf.Max(0.01f, audio.volume -= 0.8f * Time.fixedDeltaTime);
			//audio.volume = 0.01f;
		}
		else{
			continu=false;
		}
	}

	private void fadeIn(ref AudioSource audio, ref  bool continu){
		if (audio.volume < 1.0f) {
			audio.volume = Mathf.Min (1.0f, audio.volume += 0.8f * Time.fixedDeltaTime);
			//audio.volume = 1.0f;
		}
		else{
			//Debug.Log ("Fade in done");
			continu=false;
		}
	}

}
