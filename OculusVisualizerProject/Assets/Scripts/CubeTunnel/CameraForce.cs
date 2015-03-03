using UnityEngine;
using System.Collections;

public class CameraForce : MonoBehaviour {

	public Song song;
	public Material black;
	public Material sky1;
	public Material sky2;
	public Material sky3;
	public Material sky4;

	private float t1;
	private float t2;
	private float t3;
	private bool switchFirst;
	private bool switchSecond;
	private bool switchThird;
	// Use this for initialization
	void Start () {

		RenderSettings.skybox = black;
		RenderSettings.skybox.SetColor("_Color", Color.black);


		t1 = song.debutLastStretch + (song.finLastStretch - song.debutLastStretch)*0.25f;
		t2 = song.debutLastStretch + (song.finLastStretch - song.debutLastStretch)*0.5f;
		t3 = song.debutLastStretch + (song.finLastStretch - song.debutLastStretch)*0.75f;

		switchFirst = false;
		switchSecond = false;
		switchThird = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.realtimeSinceStartup>song.debutLastStretch){
			float dist = 0;
			if(Time.realtimeSinceStartup > song.finLastStretch)
			{
				dist = -transform.position.z;
				rigidbody.AddForce(0,0,dist);
			}
			else{
				dist = -transform.position.z - 5f;
				rigidbody.AddForce(0,0,dist);
			}
		}

		if(Time.realtimeSinceStartup>song.debutLastStretch)
		{
			RenderSettings.skybox = sky1;
		}

		if(!switchFirst && Time.realtimeSinceStartup>t1)
		{
			RenderSettings.skybox = sky2;
		}
		if(!switchSecond && Time.realtimeSinceStartup>t2)
		{
			RenderSettings.skybox = sky3;
		}
		if(!switchThird && Time.realtimeSinceStartup>t3)
		{
			RenderSettings.skybox = sky4;
		}


	}
}
