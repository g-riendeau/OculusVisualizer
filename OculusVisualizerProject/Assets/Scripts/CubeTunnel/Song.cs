using UnityEngine;
using System.Collections;

public class Song : MonoBehaviour {

	/*
	Leave1 : 3:47 = 227
	Enter1 : 4:06 = 246
	Leave2 : 4:08 = 248
	Enter2 : 4:35 = 275
	Leave3 : 4:42 = 282
	Enter3 : 4:48 = 288
	Leave4 : 4:52 = 292
	Enter4 : 5:26 = 326
	*/
	
	public SongProcessor songProcessor;
	//Bools for playback controls
	public bool songPlaying = false;
	public bool outsideSongPlaying = false;
	public bool firstExitDone = false;
	public bool secondExitDone = false;
	public bool thirdExitDone = false;
	public bool lastExitDone = false;
	public bool firstEnterDone = false;
	public bool secondEnterDone = false;
	public bool thirdEnterDone = false;
	public bool lastEnterDone = false;

	//Main parts of song
	public float startSong;
	public float colorItUp;

	public float debut2eTiers;
	public float length2eTiers;

	public float debut3eTiers;
	public float length3eTiers;

	public float descenteAuxEnfers;
	public float supernovae;

	//Sub-parts du dernier tier
	public float leave1;
	public float enter1;
	public float leave2;
	public float enter2;
	public float leave3;
	public float enter3;
	public float leave4;
	public float enter4;

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

		leave1 = 227.5f;
		enter1 = 246.0f;
		leave2 = 247.5f;
		enter2 = 275.0f;
		leave3 = 282.5f;
		enter3 = 288.0f;
		leave4 = 292.0f;
		enter4 = 326.0f;


		flexionTime = new float[2] {0f,115f};
		flexionLength = new float[2] {13f,64f};

		zSpinTime1 = 25.5f;
		zSpinLength1 = 24.5f;
		zSpinTime2 = 51f;
		zSpinLength2 = 39f;

		skyboxTime = new float[4] {debut3eTiers,247f,276f,289f};

	}

	void Update() {
		if (time () >= 0 && !songPlaying) {
			songProcessor.startPlaying();
			songPlaying = true;
		}

		handleOutsideSong();

	}


	// Temps depuis le debut de la chanson
	public float time(){
		return Time.time - startSong;
	}

	private void handleOutsideSong(){
		//Start outside song 
		if(time () >= debut3eTiers && !outsideSongPlaying){
			songProcessor.startOutsideSong();
			outsideSongPlaying = true;
		}

		//Handle song transitions when leaving/entering the tunnel :
		if(time () >= leave1 && !firstExitDone){
			songProcessor.leaveTunnel();
			firstExitDone = true;
			Debug.Log("Leaving...1");
		}

		if(time () >= enter1 && !firstEnterDone){
			songProcessor.enterTunnel();
			firstEnterDone = true;
			Debug.Log("Entering...1");
		}

		if(time () >= leave2 && !secondExitDone){
			songProcessor.leaveTunnel();
			secondExitDone = true;
			Debug.Log("Leaving...2");
		}

		if(time () >= enter2 && !secondEnterDone){
			songProcessor.enterTunnel();
			secondEnterDone = true;
			Debug.Log("Entering...2");
		}

		if(time () >= leave3 && !thirdExitDone){
			songProcessor.setOutsideSongTime(103.0f);
			songProcessor.leaveTunnel();
			thirdExitDone = true;
			Debug.Log("Leaving...3");
		}

		if(time () >= enter3 && !thirdEnterDone){
			songProcessor.enterTunnel();
			thirdEnterDone = true;
			Debug.Log("Entering...3");
		}

		if(time () >= leave4 && !lastExitDone){
			songProcessor.changeOutsideSong();
			songProcessor.setOutsideSongTime(59.0f);
			songProcessor.leaveTunnel();
			lastExitDone = true;
			Debug.Log("Leaving...4");
		}

		if(time () >= enter4 && !lastEnterDone){
			songProcessor.enterTunnel();
			lastEnterDone = true;
			Debug.Log("Entering...4");
		}
		

	}
}
