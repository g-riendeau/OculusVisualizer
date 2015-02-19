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

	private CubeInfo[,] floorCubes;


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

		//On effecte la premiere rangée des cubes du plancher (depth = 0)
		//TODO Éviter les valeurs 4, 5 pour le milieu.
		floorCubes[4,0].lastScale = floorCubes[4,0].transform.localScale.y;
		floorCubes[4,0].transform.localScale = new Vector3( 1f, hauteurGraves, 1f) ;
		floorCubes[5,0].lastScale = floorCubes[5,0].transform.localScale.y;
		floorCubes[5,0].transform.localScale = new Vector3( 1f, hauteurGraves, 1f) ;

		for (int i = 3 ; i >= 0 ; i--){
			floorCubes[i,0].lastScale = floorCubes[i,0].transform.localScale.y;
			floorCubes[i,0].transform.localScale = new Vector3( 1f, floorCubes[i+1,0].lastScale, 1f) ;
		}
		for (int i = 6 ; i < floorCubes.GetLength(0) ; i++){
			floorCubes[i,0].lastScale = floorCubes[i,0].transform.localScale.y;
			floorCubes[i,0].transform.localScale = new Vector3( 1f, floorCubes[i-1,0].lastScale, 1f) ;
		}
		//On affecte le reste des cubes
		ApplyScaleWave();

		//On colore
		ApplyColorWave ();
	}

	private float Tanh( float x ){
		float exp2x = Mathf.Exp(x-5f);
		return 0.5f*(exp2x-1f)/(exp2x+1f)+0.5f;
	}

	void ApplyScaleWave(){
		for(int i = 0; i<floorCubes.GetLength(0); i++){
			for(int j = 1; j<floorCubes.GetLength(1); j++){
				//Debug.Log (j);
				floorCubes[i,j].lastScale = floorCubes[i,j].transform.localScale.y;
				floorCubes[i,j].transform.localScale = new Vector3( 1f, floorCubes[i,j-1].lastScale, 1f) ;
			}
		}
	} 
	void ApplyColorWave(){
		for(int i = 0; i<floorCubes.GetLength(0); i++){
			for(int j = 0; j<floorCubes.GetLength(1); j++){
				//Debug.Log (j);
				floorCubes[i,j].renderer.material.SetColor("_Color", new Color(floorCubes[i,j].transform.localScale.y/4f, 
				                                                               Mathf.Sin(Time.realtimeSinceStartup/3)*0.9f,
				                                                               0.5f - (floorCubes[i,j].transform.localScale.y/4f)*0.5f));
			}
		}
		
	} 
}
 