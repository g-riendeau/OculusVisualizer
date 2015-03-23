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
	
	public float debut2eTiers;
	public float fin2eTiers;
	public float bassDrop;
	public float debutLastStretch;
	public float finLastStretch;
	public float songTime;

	// Truc relatif a la flexion du tunnel
	public float[] flexion_t;
	public float[] flexion_length;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
