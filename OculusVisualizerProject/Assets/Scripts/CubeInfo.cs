using UnityEngine;
using System.Collections;

public class CubeInfo: MonoBehaviour {
	//Position 0 = caméra
	private int _idxDepth;
	public float scale;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Initialize(int depth){
		_idxDepth = depth;
	}
}
