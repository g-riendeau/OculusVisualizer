using UnityEngine;
using System.Collections;

public class CubeInfo: MonoBehaviour {
	public float lastScale;
	public Color lastColor;
	public float jRatio;
	public float jWidth;
	// Use this for initialization
	void Start () {
		lastScale = 0.1f;
		lastColor = new Color(1.0f, 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
