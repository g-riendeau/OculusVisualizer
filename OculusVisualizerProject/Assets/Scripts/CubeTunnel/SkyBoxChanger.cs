using UnityEngine;
using System.Collections;

public class SkyBoxChanger : MonoBehaviour {

	public Song song;
	public Material black;
	public Material sky0;
	public Material sky1;
	public Material sky2;
	public Material sky3;
	private bool[] switchSky;

	// Use this for initialization
	void Start () {
		Debug.Log("Changing to black!!");
		RenderSettings.skybox = black;
		RenderSettings.skybox.SetColor("_Color", Color.black);

		switchSky = new bool[song.skyboxTime.Length];
		for (int i=0; i<song.skyboxTime.Length; i++) {
			switchSky [i] = false;
		}

		GetComponent<Camera>().backgroundColor = new Color(1.0f,1.0f,0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		if (!switchSky [0] && song.time () > song.skyboxTime [0]) {
			RenderSettings.skybox = sky0;
			switchSky [0] = true;
		} else if (!switchSky [1] && song.time () > song.skyboxTime [1]) {
			RenderSettings.skybox = sky1;
			switchSky [1] = true;
		} else if (!switchSky [2] && song.time () > song.skyboxTime [2]) {
			RenderSettings.skybox = sky2;
			switchSky [2] = true;
		} else if (!switchSky [3] && song.time () > song.skyboxTime [3]) {
			RenderSettings.skybox = sky3;
			switchSky [3] = true;
		}
	}
	

}
