using UnityEngine;
using System.Collections;
using System;

public class AudioProcessor : MonoBehaviour
{
	public float[] amplitudes;
	public Song song;

	void Awake ()
	{
		GetComponent<AudioSource>().pitch = 1f;
		amplitudes = new float[1024];
		Array.Clear (amplitudes, 0, amplitudes.Length);
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	public virtual void FixedUpdate ()
	{
		//ON ne rempli plus les amplitudes ici. On doit utiliser les classes enfant
		//MicProcessor ou SongProcessor
		// Pour fastForward la toune
		if (Input.GetKeyDown ("f")) {
			Time.timeScale = 8f;
			GetComponent<AudioSource>().pitch = 8f;
			Debug.Log ("x8");
			GetComponent<AudioSource>().mute = true;				
		}
		// Pour Ralentir la toune
		if (Input.GetKeyDown ("b")) {
			Time.timeScale = 1f;
			GetComponent<AudioSource>().pitch = 1f;
			Debug.Log ("Normal speed");
			GetComponent<AudioSource>().mute = false;					
		}
	}
}
