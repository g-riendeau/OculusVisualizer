using UnityEngine;
using System.Collections;
using System;

public class SongProcessor : AudioProcessor
{
	public AudioSource mainSong; 
	public AudioSource outsideSong;
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
		base.FixedUpdate();

		if (mainSongFadeOut){
			fadeOut(mainSong,mainSongFadeOut );
		}
		if (outsideSongFadeOut){
			fadeOut(outsideSong, outsideSongFadeOut);
		}
		if (mainSongFadeIn){
			fadeIn(mainSong, mainSongFadeIn);
		}
		if (outsideSongFadeIn){
			fadeIn(outsideSong, outsideSongFadeIn);
		}
	}
	public void startPlaying (){
		mainSong.Play ();
		outsideSong.Play ();
	}

	public void leaveTunnel(){
		mainSongFadeOut = true;
		outsideSongFadeIn = true;
		Debug.Log("LEaving tunnel!!");
	}
	public void enterTunnel(){
		mainSongFadeIn = true;
		outsideSongFadeOut = true;
	}

	private void fadeOut(AudioSource audio, bool continu){
		if(audio.volume > 0.01f){
			audio.volume -= 0.2f * Time.fixedDeltaTime;
		}
		else
			continu=false;
	}

	private void fadeIn(AudioSource audio, bool continu){
		if (audio.volume < 1f) {
			audio.volume += 0.2f * Time.fixedDeltaTime;
		}
		else
			continu=false;
	}

}
