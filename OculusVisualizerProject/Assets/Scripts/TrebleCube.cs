using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class TrebleCube : MonoBehaviour {
	//private float _posX;
	//private float _posY;
	private float hauteurAigues;
	
	public AudioProcessor audioProcessor ;
	public CubeWallComponent wallComponent;
	private CubeInfo[,] ceilingCubes;
	
	
	// Use this for initialization
	void Start () {
		ceilingCubes = wallComponent.cubeArray;
	}
	
	// Update is called once per frame
	void Update () {

		hauteurAigues = HauteurCube( 2 );

		ApplyFirstRow(hauteurAigues);
		ApplyScaleWave();
		ApplyColorWave ();
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
			amplitudes = new float[8];
			break;
		case 1 :
			cuton = 32;
			cutoff = 128;
			scale = 50f;
			amplitudes = new float[96];
			break;
		case 2 :
			cuton = 512;
			cutoff = 1024;
			scale = 10000f;
			amplitudes = new float[512];
			break;
		default :
			amplitudes = new float[1];
			Debug.LogError("range doit etre entre 0 et 2");
			break;
		}

		// moyenne du range de la FFT
		Array.Copy(audioProcessor.amplitudes, cuton, amplitudes, 0, cutoff-cuton);
		moy = scale * amplitudes.Average ();

		// application de la transformation mathematique
		cumul = 0.9f * cumul + 0.5f * Tanh (moy);
		hauteur = 1f * cumul + 0.2f * moy;
		if (hauteur < 0.1f)
			hauteur = 0.1f;

		return hauteur;
	}	

	float Tanh( float x ){
		float exp2x = Mathf.Exp(x-5f);
		return 0.5f*(exp2x-1f)/(exp2x+1f)+0.5f;
	}

	void ApplyFirstRow(float hauteurAigues)
	{
		//On effecte la premiere rangée des cubes du plancher (depth = 0)
		//TODO Éviter les valeurs 4, 5 pour le milieu.
		ceilingCubes[4,0].lastScale = ceilingCubes[4,0].transform.localScale.y;
		ceilingCubes[4,0].transform.localScale = new Vector3( 1f, hauteurAigues, 1f) ;
		ceilingCubes[5,0].lastScale = ceilingCubes[5,0].transform.localScale.y;
		ceilingCubes[5,0].transform.localScale = new Vector3( 1f, hauteurAigues, 1f) ;
		
		for (int i = 3 ; i >= 0 ; i--){
			ceilingCubes[i,0].lastScale = ceilingCubes[i,0].transform.localScale.y;
			ceilingCubes[i,0].transform.localScale = new Vector3( 1f, ceilingCubes[i+1,0].lastScale, 1f) ;
		}
		for (int i = 6 ; i < ceilingCubes.GetLength (0) ; i++){
			ceilingCubes[i,0].lastScale = ceilingCubes[i,0].transform.localScale.y;
			ceilingCubes[i,0].transform.localScale = new Vector3( 1f, ceilingCubes[i-1,0].lastScale, 1f) ;
		}

		for (int i = 0 ; i < ceilingCubes.GetLength (0) ; i++){
			ceilingCubes[i,0].lastColor = ceilingCubes[i,0].renderer.material.GetColor("_Color");

			float r = 0f;
			float g = Mathf.Sin(Time.realtimeSinceStartup/1f)*ceilingCubes[i,0].transform.localScale.y;
			float b = 1f-g;

			ceilingCubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));

		}
	}


	void ApplyScaleWave(){
		for(int i = 0; i<ceilingCubes.GetLength(0); i++){
			for(int j = 1; j<ceilingCubes.GetLength(1); j++){
				//Debug.Log (j);
				ceilingCubes[i,j].lastScale = ceilingCubes[i,j].transform.localScale.y;
				ceilingCubes[i,j].transform.localScale = new Vector3( 1f, ceilingCubes[i,j-1].lastScale, 1f) ;
			}
		}
	} 

	void ApplyColorWave(){
		for(int i = 0; i<ceilingCubes.GetLength(0); i++){
			for(int j = 1; j<ceilingCubes.GetLength(1); j++){
				ceilingCubes[i,j].lastColor = ceilingCubes[i,j].renderer.material.GetColor("_Color");
				ceilingCubes[i,j].renderer.material.SetColor("_Color", ceilingCubes[i,j-1].lastColor);
			}
		}
	}
}
