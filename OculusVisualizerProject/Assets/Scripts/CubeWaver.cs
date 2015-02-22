using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class CubeWaver : MonoBehaviour {
	//private float _posX;
	//private float _posY;
	private float hauteurAigues;
	private float hauteurBasses;
	private float hauteurMoyennes;
	
	public AudioProcessor audioProcessor ;
	public CubeWallComponent bassWallComponent;
	public CubeWallComponent trebleWallComponent;
	public CubeWallComponent leftMiddleComponent;
	public CubeWallComponent rightMiddleComponent;
	private CubeInfo[,] _ceilingCubes;
	private CubeInfo[,] _floorCubes;
	private CubeInfo[,] _leftWallCubes;
	private CubeInfo[,] _rightWallCubes;
	
	
	// Use this for initialization
	void Start () {
		_ceilingCubes = trebleWallComponent.cubeArray;
		_floorCubes = bassWallComponent.cubeArray;
		_leftWallCubes = leftMiddleComponent.cubeArray;
		_rightWallCubes = rightMiddleComponent.cubeArray;
	}
	
	// Update is called once per frame
	void Update () {

		hauteurBasses = HauteurCube( 0 );
		hauteurMoyennes = HauteurCube( 1 );
		hauteurAigues = HauteurCube( 2 );

		ApplyFirstRow(_ceilingCubes, hauteurAigues, 2);
		ApplyFirstRow(_floorCubes, hauteurBasses, 0);
		ApplyFirstRow(_leftWallCubes, hauteurMoyennes, 1);
		ApplyFirstRow(_rightWallCubes, hauteurMoyennes, 1);

		ApplyScaleWave(_ceilingCubes);
		ApplyScaleWave(_floorCubes);
		ApplyScaleWave(_leftWallCubes);
		ApplyScaleWave(_rightWallCubes);

		ApplyColorWave (_ceilingCubes);
		ApplyColorWave (_floorCubes);
		ApplyColorWave (_leftWallCubes);
		ApplyColorWave (_rightWallCubes);
	}
	


	float HauteurCube( int range ){
		// range :
		// Basses -> 0
		// Mids   -> 1
		// Aigues -> 2

		int cuton = 0;
		int cutoff = 0;
		float scale = 0f;
		float[] amplitudes;
		float cumul = 0f;
		float moy;
		float hauteur;

		// Selection des parametres
		switch ( range )
		{
		case 0 :
			cuton = 0;
			cutoff = 8;
			scale = 100f;

			break;
		case 1 :
			cuton = 16;
			cutoff = 128;
			scale = 700f;
			break;
		case 2 :
			cuton = 512;
			cutoff = 1024;
			scale = 10000f;
			break;
		default :
			amplitudes = new float[1];
			Debug.LogError("range doit etre entre 0 et 2");
			break;
		}

		// moyenne du range de la FFT
		amplitudes = new float[cutoff-cuton];
		Array.Copy(audioProcessor.amplitudes, cuton, amplitudes, 0, cutoff-cuton);
		moy = scale * amplitudes.Average ();

		// application de la transformation mathematique
		cumul = 0.95f * cumul + 0.5f * Tanh (moy);
		hauteur = 1f * cumul + 0.2f * moy;
		if (hauteur < 0.1f)
			hauteur = 0.1f;

		return hauteur;
	}	

	float Tanh( float x ){
		float exp2x = Mathf.Exp(x-5f);
		return 0.5f*(exp2x-1f)/(exp2x+1f)+0.5f;
	}

	void ApplyFirstRow(CubeInfo[,] cubes, float hauteurAigues, int range)
	{
		//On effecte la premiere rangée des cubes du plancher (depth = 0)
		//TODO Éviter les valeurs 4, 5 pour le milieu.
		cubes[4,0].lastScale = cubes[4,0].transform.localScale.y;
		cubes[4,0].transform.localScale = new Vector3( 1f, hauteurAigues, 1f) ;
		cubes[5,0].lastScale = cubes[5,0].transform.localScale.y;
		cubes[5,0].transform.localScale = new Vector3( 1f, hauteurAigues, 1f) ;
		
		for (int i = 3 ; i >= 0 ; i--){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;
			cubes[i,0].transform.localScale = new Vector3( 1f, cubes[i+1,0].lastScale, 1f) ;
		}
		for (int i = 6 ; i < cubes.GetLength (0) ; i++){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;
			cubes[i,0].transform.localScale = new Vector3( 1f, cubes[i-1,0].lastScale, 1f) ;
		}

		//for (int i = 0 ; i < cubes.GetLength (0) ; i++){
		//	cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
		//	float r = 0f;
		//	float g = Mathf.Sin(Time.realtimeSinceStartup/1f)*cubes[i,0].transform.localScale.y;
		//	float b = 1f-g;
		//	cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));
		//}

		switch ( range )
		{
			/*
		case 0 :
			for (int i = 0 ; i < cubes.GetLength (0) ; i++){
				cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
				
				float r = cubes[i,0].transform.localScale.y/4f;
				float g = Mathf.Sin(Time.realtimeSinceStartup/3)*0.9f;
				float b = 0.5f - (cubes[i,0].transform.localScale.y/4f)*0.5f;				
				cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));		
			}
			break;
		case 1 :
			for (int i = 0 ; i < cubes.GetLength (0) ; i++){
				cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
				float r = (1f-Mathf.Sin(Time.realtimeSinceStartup)/3f) * (0.02f + cubes[i,0].transform.localScale.y/3f);
				float g =0.2f - cubes[i,0].transform.localScale.y/2f; 
				float b = (Mathf.Sin(Time.realtimeSinceStartup)/3f) * (0.01f + cubes[i,0].transform.localScale.y/2f);
				cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));
			}
			break;
		case 2 :
			for (int i = 0 ; i < cubes.GetLength (0) ; i++){
				cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
				float r = 0f;
				float g = Mathf.Sin(Time.realtimeSinceStartup)*cubes[i,0].transform.localScale.y;
				float b = 1f-g;
				cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));
			}
			break;
			*/
		default :
			//Debug.LogError("range doit etre entre 0 et 2");
			for (int i = 0 ; i < cubes.GetLength (0) ; i++){
				cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");

				float ampli = cubes[i,0].transform.localScale.y;
				float g = (Mathf.Sin(Time.realtimeSinceStartup/7f)*0.7f) * (ampli/2f);
				float r = (ampli/7f) - (0.8f*g);
				float b = 0.5f - (ampli/3f)*0.5f;				

				cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));		
			}
			break;
		}
	}


	void ApplyScaleWave(CubeInfo[,] cubes){
		for(int i = 0; i<cubes.GetLength(0); i++){
			for(int j = 1; j<cubes.GetLength(1); j++){
				//Debug.Log (j);
				cubes[i,j].lastScale = cubes[i,j].transform.localScale.y;
				cubes[i,j].transform.localScale = new Vector3( 1f, cubes[i,j-1].lastScale, 1f) ;
			}
		}
	} 

	void ApplyColorWave(CubeInfo[,] cubes){
		for(int i = 0; i<cubes.GetLength(0); i++){
			for(int j = 1; j<cubes.GetLength(1); j++){
				cubes[i,j].lastColor = cubes[i,j].renderer.material.GetColor("_Color");
				cubes[i,j].renderer.material.SetColor("_Color", cubes[i,j-1].lastColor);
			}
		}
	}
}
