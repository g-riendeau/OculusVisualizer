using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class TrebleCube : MonoBehaviour {
	//private float _posX;
	//private float _posY;
	private float hauteurAigues;
	
	public AudioProcessor audioProcessor ;
	public CubeWallComponent floor;
	
	private const int cutoffAigues = 512;  // cutoff
	private float[] amplitudesAigues;
	
	private GameObject[,] floorCubes;
	
	
	// Use this for initialization
	void Start () {
		amplitudesAigues = new float[1024-cutoffAigues];
		floorCubes = floor.cubeArray;
	}
	
	// Update is called once per frame
	void Update () {
		Array.Copy(audioProcessor.amplitudes, cutoffAigues, amplitudesAigues, cutoffAigues, 1024-cutoffAigues);
		hauteurAigues = 1.5f * Tanh(100f * amplitudesAigues.Average ());
		if (hauteurAigues < 0.1f)
			hauteurAigues = 0.1f;

		floorCubes[0,0].transform.localScale = new Vector3(1f, hauteurAigues, 1f);
	}
	
	private float Tanh( float x ){
		float exp2x = Mathf.Exp(x-5f);
		return 0.5f*(exp2x-1f)/(exp2x+1f)+0.5f;
	}
}
