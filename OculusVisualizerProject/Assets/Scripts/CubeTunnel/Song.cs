using UnityEngine;
using System.Collections;

public class Song : MonoBehaviour {

	
	public AudioProcessor songProcessor;
	public bool songPlaying = false;

	public float startSong;
	public float colorItUp;

	public float debut2eTiers;
	public float length2eTiers;

	public float debut3eTiers;
	public float length3eTiers;

	public float descenteAuxEnfers;
	public float supernovae;
	
	public float songTime;

	// Truc relatif a la flexion du tunnel
	public float[] flexionTime;
	public float[] flexionLength;


	// Trucs rlatifs aux rotations
	public float zSpinTime1;
	public float zSpinLength1;
	public float zSpinTime2;
	public float zSpinLength2;

	public float[] skyboxTime;

	//Infos de chanson
	//Infos pour brave men:
	void Awake(){
		startSong = 10f;
		colorItUp = 12.8f;
		debut2eTiers = 115f;
		length2eTiers = 64f;
		debut3eTiers = 223.75f;
		length3eTiers = 102.25f;
		descenteAuxEnfers = 271f;
		supernovae = 290f;
		songTime = 326f;

		flexionTime = new float[2] {0f,115f};
		flexionLength = new float[2] {13f,64f};

		zSpinTime1 = 25.5f;
		zSpinLength1 = 24.5f;
		zSpinTime2 = 51f;
		zSpinLength2 = 39f;

		skyboxTime = new float[4] {debut3eTiers,248f,276f,289f};

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
