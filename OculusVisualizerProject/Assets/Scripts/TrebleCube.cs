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
	
	private const int cutoffAigues = 512;  // cutoff
	private float[] amplitudesAigues;
	
	private CubeInfo[,] ceilingCubes;
	
	
	// Use this for initialization
	void Start () {
		amplitudesAigues = new float[1024-cutoffAigues];
		ceilingCubes = wallComponent.cubeArray;
	}
	
	// Update is called once per frame
	void Update () {
		Array.Copy(audioProcessor.amplitudes, cutoffAigues, amplitudesAigues, 0, 1024-(cutoffAigues));
		//hauteurAigues = 1.5f * Tanh(1000f * amplitudesAigues.Average ());
		hauteurAigues = 100f * ( Tanh(2000f * amplitudesAigues.Average ()));
		//Debug.Log (hauteurAigues);
		if (hauteurAigues < 0.1f)
			hauteurAigues = 0.1f;

		ApplyFirstRow(hauteurAigues);
		ApplyScaleWave();
		ApplyColorWave ();
	}
	
	private float Tanh( float x ){
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
