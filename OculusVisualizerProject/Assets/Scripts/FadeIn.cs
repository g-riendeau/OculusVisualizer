using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {
	private float t;
	private float alphaValue;

	// Use this for initialization
	void Start () {
		t = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		t += Time.deltaTime;
		if (t < 3f) {
			alphaValue = 1f - t / 3f;
			Color color = gameObject.renderer.material.color;
			color.a = alphaValue;
			gameObject.renderer.material.color = color;
		}
		else{
			Destroy(gameObject);
		}

	}
}
