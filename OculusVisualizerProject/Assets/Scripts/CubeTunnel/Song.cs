using UnityEngine;
using System.Collections;

public class Song : MonoBehaviour {

	//Infos de chanson
	//Infos pour brave men:
	/*
	public float debut2eTiers = 115f;
	public float fin2eTiers = 179f;
	public float bassDrop = 224f;
	public float debutLastStretch = 275;
	public float finLastStretch = 326f;
	public float songTime = 355f;
*/
	//Essai Simon
	/*
		debut2eTiers = 115f;
		fin2eTiers = 179f;
		bassDrop = 12.5f;
		debutLastStretch = 224f;
		finLastStretch = 326.5f;
		songTime = 355f;
		*/ 
	public AudioProcessor songProcessor;
	public bool songPlaying = false;

	public float startSong;
	public float colorItUp;

	public float debut2eTiers = 115f;
	public float fin2eTiers;
	public float bassDrop;
	public float debutLastStretch;
	public float finLastStretch;
	public float songTime;

	// Truc relatif a la flexion du tunnel
	public float[] flexionTime = new float[2];
	public float[] flexionLength = new float[2];


	// Trucs rlatifs aux rotations
	public float zSpinTime1;
	public float zSpinLength1;
	public float zSpinTime2;
	public float zSpinLength2;

	public float[] skyboxTime;

	void Start(){
		startSong = 10f;
		colorItUp = 12.8f;

		flexionTime[0] = 0f;
		flexionTime[1] = 179f;
		flexionLength[0] = 13f;
		flexionLength[1] = 44f;

		zSpinTime1 = 25.5f;
		zSpinLength1 = 24.5f;
		zSpinTime2 = 51f;
		zSpinLength2 = 39f;
	}

	void Update() {
		if (time () >= 0 && !songPlaying) {
			songProcessor.startPlaying();
			songPlaying = true;
		}
	}


	// Temps depuis le debut de la chanson
	public float time(){
		return Time.time - startSong;
	}

	
}
