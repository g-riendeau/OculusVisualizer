using UnityEngine;
using System.Collections;

public class CubeInfo: MonoBehaviour {
	//Position 0 = caméra
	private int _idxDepth;
	public float lastScale;
	// Use this for initialization
	void Start () {
		lastScale = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialize(int depth){
		_idxDepth = depth;
	}
}
