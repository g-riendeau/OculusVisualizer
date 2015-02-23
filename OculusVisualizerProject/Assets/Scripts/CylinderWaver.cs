using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class CylinderWaver : MonoBehaviour {
	//private float _posX;
	//private float _posY;
	private float hauteurAigues;
	private float hauteurBasses;
	private float hauteurMoyennes;
	private float w = 2f * 10f * Mathf.Sin (180f / 60f);
	private float cumul = 0f;

	public AudioProcessor audioProcessor ;
	public CubeCylinder cylinder;
	private CubeInfo[,] _cubeArray;
	
	
	// Use this for initialization
	void Start () {
		_cubeArray = cylinder.cubeArray;
	}
	
	// Update is called once per frame
	void Update () {

		hauteurBasses = HauteurCube( 0 );
		hauteurMoyennes = HauteurCube( 1 );
		hauteurAigues = HauteurCube( 2 );

		ApplyFirstRow(_cubeArray, hauteurBasses, hauteurMoyennes, hauteurAigues);

		ApplyScaleWave(_cubeArray);

		ApplyColorWave (_cubeArray);

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
		//float cumul = 0f;
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
		cumul = 0.90f * cumul + 0.3f * Tanh (moy);
		hauteur = (0.4f * cumul +  0.3f*(moy) );
		if (hauteur < 0.1f)
			hauteur = 0.1f;

		return hauteur;
	}	

	float Tanh( float x ){
		float exp2x = Mathf.Exp(x-5f);
		return 0.5f*(exp2x-1f)/(exp2x+1f)+0.5f;
	}

	void ApplyFirstRow(CubeInfo[,] cubes, float hauteurBasses, float hauteurMoyennes, float hauteurAigues)
	{
		//On effecte la premiere rangée des cubes du plancher (depth = 0)
		int largeur = cubes.GetLength(0);
		int idxMidRight = 0;
		int idxGraves = largeur/4;
		int idxMidLeft = largeur/2 -1;
		int idxAigues = 3*largeur/4 -1;
		float ampli = 0.1f;
		float g = 0f;
		float r = 0f;
		float b = 0f;	
		float timeSin=Mathf.Sin(Time.realtimeSinceStartup/7f);
		float fastTime=Mathf.Sin(Time.realtimeSinceStartup*2);

		///Valeurs connues:
		/// MidsR:
		cubes[idxMidRight,0].lastScale = cubes[idxMidRight,0].transform.localScale.y;
		cubes[idxMidRight,0].transform.localScale = new Vector3( w, hauteurMoyennes, w) ;
		cubes[idxMidRight,0].lastColor = cubes[idxMidRight,0].renderer.material.GetColor("_Color");
		ampli = cubes[idxMidRight,0].transform.localScale.y;
		g =  ((timeSin*0.7f) * (ampli/6f))-0.4f;
		r = (ampli/6f) - (g);
		b = 0.8f - (ampli/3);	
		cubes[idxMidRight,0].renderer.material.SetColor("_Color", new Color(r,g,b));


		//MidsL
		cubes[idxMidLeft,0].lastScale = cubes[idxMidLeft,0].transform.localScale.y;
		cubes[idxMidLeft,0].transform.localScale = new Vector3( w, hauteurMoyennes, w) ;
		cubes[idxMidLeft,0].lastColor = cubes[idxMidLeft,0].renderer.material.GetColor("_Color");
		ampli = cubes[idxMidLeft,0].transform.localScale.y;
		g = ((timeSin*0.7f) * (ampli/6f))-0.4f;
		r = (ampli/6f) - (0.8f*g);
		b = 0.8f - (ampli/3);	
		cubes[idxMidLeft,0].renderer.material.SetColor("_Color", new Color(r,g,b));

		// Graves:
		cubes[idxGraves,0].lastScale = cubes[idxGraves,0].transform.localScale.y;
		cubes[idxGraves,0].transform.localScale = new Vector3( w, hauteurBasses, w) ;
		cubes[idxGraves,0].lastColor = cubes[idxGraves,0].renderer.material.GetColor("_Color");
		ampli = cubes[idxGraves,0].transform.localScale.y;
		g = ((0.8f - ((timeSin*0.7f) * (ampli/6)))-0.2f);
		r = (ampli/4f) - (0.8f*g);
		b = 0.8f - (ampli/3);	
		cubes[idxGraves,0].renderer.material.SetColor("_Color", new Color(r,g,b));

		// Aigues:
		cubes[idxAigues,0].lastScale = cubes[idxAigues,0].transform.localScale.y;
		cubes[idxAigues,0].transform.localScale = new Vector3( w, hauteurAigues, w) ;
		cubes[idxAigues,0].lastColor = cubes[idxAigues,0].renderer.material.GetColor("_Color");
		ampli = cubes[idxAigues,0].transform.localScale.y;
		g = (ampli/4f);
		r = 0.4f*fastTime - (ampli/6);
		b = 0.5f - (ampli/6);	
		cubes[idxAigues,0].renderer.material.SetColor("_Color", new Color(r,g,b));

		float prop1 = 0f;
		float prop2 = 0f;


		//interpoller:
		float scale = 0f;
		int dist = idxGraves - idxMidRight;
		for(int i = idxMidRight+1; i<idxGraves; i++){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;

			prop1 = (1f - (float)(i-idxMidRight)/dist);
			prop2 = 1f-prop1;
			scale = (prop1*hauteurMoyennes) + (prop2*hauteurBasses);
			cubes[i,0].transform.localScale = new Vector3( w, scale, w) ;

			r = prop1*cubes[idxMidRight,0].renderer.material.GetColor("_Color").r + prop2*cubes[idxGraves,0].renderer.material.GetColor("_Color").r;
			g = prop1*cubes[idxMidRight,0].renderer.material.GetColor("_Color").g + prop2*cubes[idxGraves,0].renderer.material.GetColor("_Color").g;
			b = prop1*cubes[idxMidRight,0].renderer.material.GetColor("_Color").b + prop2*cubes[idxGraves,0].renderer.material.GetColor("_Color").b;
			cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
			cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));

		}

		dist = idxMidLeft - idxGraves;
		for(int i = idxGraves+1; i<idxMidLeft; i++){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;
			prop1 = (1f - (float)(i-idxGraves)/dist);
			prop2 = 1f-prop1;
			scale = (prop1*hauteurBasses) + (prop2*hauteurMoyennes);
			cubes[i,0].transform.localScale = new Vector3( w, scale, w) ;

			r = prop1*cubes[idxGraves,0].renderer.material.GetColor("_Color").r + prop2*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").r;
			g = prop1*cubes[idxGraves,0].renderer.material.GetColor("_Color").g + prop2*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").g;
			b = prop1*cubes[idxGraves,0].renderer.material.GetColor("_Color").b + prop2*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").b;
			cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
			cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));
		}

		dist = idxAigues - idxMidLeft;
		for(int i = idxMidLeft+1; i<idxAigues; i++){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;
			prop1 = (1f - (float)(i-idxMidLeft)/dist);
			prop2 = 1f-prop1;
			scale = (prop1*hauteurMoyennes) + (prop2*hauteurAigues);
			cubes[i,0].transform.localScale = new Vector3( w, scale, w) ;

			r = prop1*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").r + prop2*cubes[idxAigues,0].renderer.material.GetColor("_Color").r;
			g = prop1*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").g + prop2*cubes[idxAigues,0].renderer.material.GetColor("_Color").g;
			b = prop1*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").b + prop2*cubes[idxAigues,0].renderer.material.GetColor("_Color").b;
			cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
			cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));
		}

		dist = largeur - idxAigues;
		for(int i = idxAigues+1; i<largeur; i++){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;
			prop1 = (1f - (float)(i-idxAigues)/dist);
			prop2 = 1f-prop1;
			scale = (prop1*hauteurAigues) + (prop2*hauteurMoyennes);
			cubes[i,0].transform.localScale = new Vector3( w, scale, w) ;

			r = prop1*cubes[idxAigues,0].renderer.material.GetColor("_Color").r + prop2*cubes[idxMidRight,0].renderer.material.GetColor("_Color").r;
			g = prop1*cubes[idxAigues,0].renderer.material.GetColor("_Color").g + prop2*cubes[idxMidRight,0].renderer.material.GetColor("_Color").g;
			b = prop1*cubes[idxAigues,0].renderer.material.GetColor("_Color").b + prop2*cubes[idxMidRight,0].renderer.material.GetColor("_Color").b;
			cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
			cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));
		}

	}


	void ApplyScaleWave(CubeInfo[,] cubes){
		for(int i = 0; i<cubes.GetLength(0); i++){
			for(int j = 1; j<cubes.GetLength(1); j++){
				//Debug.Log (j);
				cubes[i,j].lastScale = cubes[i,j].transform.localScale.y;
				cubes[i,j].transform.localScale = new Vector3( w, cubes[i,j-1].lastScale, w) ;
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
