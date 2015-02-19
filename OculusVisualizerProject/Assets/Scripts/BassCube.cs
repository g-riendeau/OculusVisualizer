using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class BassCube : MonoBehaviour {
	//private float _posX;
	//private float _posY;
	private float cumulGraves;
	private float moyGraves;
	private float hauteurGraves;

	public AudioProcessor audioProcessor ;
	public CubeWallComponent floor;

	private const int cutoffGraves = 8;
	private float[] amplitudesGraves;

	private GameObject[,] floorCubes;


	// Use this for initialization
	void Start () {
		amplitudesGraves = new float[ cutoffGraves ];
		floorCubes = floor.cubeArray;
	}
	
	// Update is called once per frame
	void Update () {
		Array.Copy(audioProcessor.amplitudes, 0, amplitudesGraves, 0, cutoffGraves);
		moyGraves = 100f*amplitudesGraves.Average();
		cumulGraves = 0.9f*cumulGraves + 0.5f*Tanh(moyGraves);
		hauteurGraves = 1f * cumulGraves + 0.2f * moyGraves;

		if (hauteurGraves < 0.1f)
			hauteurGraves = 0.1f;

		//On efect la premiere rangée des cubes du plancher (depth = 0)
		for(int i = 0; i<floorCubes.GetLength(0); i++){
			floorCubes[i,0].transform.localScale = new Vector3( 1f, hauteurGraves, 1f) ;
		}
	}

	private float Tanh( float x ){
		float exp2x = Mathf.Exp(x-5f);
		return 0.5f*(exp2x-1f)/(exp2x+1f)+0.5f;
	}

	void ApplyWave(){


	} 
}
 