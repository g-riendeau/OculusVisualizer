using UnityEngine;
using System.Collections;

public class CameraForce : MonoBehaviour {

	public Song song;
	public Material black;
	//public Material[] sky = new Material[song.skyboxTime.Length];
	
	private bool switchFirst;
	private bool switchSecond;
	private bool switchThird;

	// Use this for initialization
	void Start () {

		RenderSettings.skybox = black;
		RenderSettings.skybox.SetColor("_Color", Color.black);


//		t1 = song.debutLastStretch + (song.finLastStretch - song.debutLastStretch)*0.25f;
//		t2 = song.debutLastStretch + (song.finLastStretch - song.debutLastStretch)*0.5f;
//		t3 = song.debutLastStretch + (song.finLastStretch - song.debutLastStretch)*0.75f;

		switchFirst = false;
		switchSecond = false;
		switchThird = false;

	}
	
	// Update is called once per frame
	void Update () {
//		if( !posAtteinte )
//			posAtteinte = GoToPosition (new Vector3 (0f, 0f, 10f), 1f);

//		if(Time.time>song.debutLastStretch){
//			float dist = 0;
//			if(Time.time > song.finLastStretch)
//			{
//				dist = -transform.position.z;
//				rigidbody.AddForce(0,0,dist);
//			}
//			else{
//				dist = -transform.position.z - 5f;
//				rigidbody.AddForce(0,0,dist);
//			}
//		}

//		if(Time.time>song.debutLastStretch)
//		{
//			RenderSettings.skybox = sky1;
//		}
//
//		if(!switchFirst && Time.time>t1)
//		{
//			RenderSettings.skybox = sky2;
//		}
//		if(!switchSecond && Time.time>t2)
//		{
//			RenderSettings.skybox = sky3;
//		}
//		if(!switchThird && Time.time>t3)
//		{
//			RenderSettings.skybox = sky4;
//		}
	}
	
//	private bool StopAtPosition( Vector3 target, float time ){
//		Vector3 dist = target - transform.position;
//		vitesse += acceleration * Time.deltaTime;
//		Vector3 dPos = dist / dist.magnitude * vitesse * Time.deltaTime;
//		if (dPos.magnitude >= dist.magnitude) {
//			transform.position = target;
//			return true;
//		} else {
//			transform.position += dPos;
//			return false;
//		}
//	}
}
