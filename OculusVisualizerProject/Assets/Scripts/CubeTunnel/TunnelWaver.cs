using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class TunnelWaver : MonoBehaviour {
	
	private float hauteurAigues;
	private float hauteurBasses;
	private float hauteurMoyennes;
	private float cumul = 0f;
	public Song song;

	public AudioProcessor audioProcessor ;
	public AudioProcessor micProcessor ;
	public CubeTunnel tunnel;
	private CubeInfo[,] _cubeCone1Array;
	private CubeInfo[,] _cubeCone2Array;
	//private CubeInfo[,] _cubeCylinderArray;
	private CubeInfo[,] _cubeCenterArray;

	private float[] flexion_t;
	private bool[] flexionDone;
	private bool[] firstFlexFrame;
	private float[] ampFlexion;
	private float[] freqFactorY;

	public TunnelSpinner TunnelSpinner;
	
	// Use this for initialization
	void Start () {
		_cubeCone1Array = tunnel.cubeCone1Array;
		_cubeCone2Array = tunnel.cubeCone2Array;
		//_cubeCylinderArray = tunnel.cubeCylinderArray;
		_cubeCenterArray = tunnel.cubeCenterArray;
		micProcessor.enabled = false;

		flexionDone = new bool[song.flexion_t.Length];
		firstFlexFrame= new bool[song.flexion_t.Length];
		ampFlexion = new float[song.flexion_t.Length];
		freqFactorY = new float[song.flexion_t.Length];

		for (int i = 0; i<song.flexion_t.Length; i++){
			flexionDone[i] = false;
			firstFlexFrame[i] = true;
			freqFactorY[i] = UnityEngine.Random.Range(0.2f, 2f);
		}


	}
	
	// Update is called once per frame
	void Update () {

		hauteurBasses = HauteurCube( 0 );
		hauteurMoyennes = HauteurCube( 1 );
		hauteurAigues = HauteurCube( 2 );


		// On applique les trucs a la premiere rangée
		ApplyFirstRow(_cubeCone1Array, hauteurBasses, hauteurMoyennes, hauteurAigues);
		CopyFirstRow (_cubeCone1Array, _cubeCone2Array, 0, 1);
		CopyFirstRow (_cubeCone1Array, _cubeCenterArray, 0, _cubeCenterArray.GetLength(1));


		// --------------------------- S C A L E !  ----------------------------------
		// On transmet le scale de rangée en rangée
		ApplyScaleWave(_cubeCone1Array);
		ApplyScaleWave(_cubeCone2Array);
		
		// --------------------------------***************---------------------------------
		// --------------------------------------------------------------------------------


		// --------------------------- C O U L E U R ----------------------------------
		// On transmet la couleur de rangée en rangée
		ApplyColorWave (_cubeCone1Array);
		ApplyColorWave (_cubeCone2Array);
		
		// --------------------------------***************---------------------------------
		// --------------------------------------------------------------------------------


		// ------------------------F L E X I O N   T U N N E L ------------------------------
		// Make the tunnel bend after the specified time
		for (int i=0; i<flexionDone.Length;i++){
			if (Time.time >song.flexion_t[i] && !flexionDone[i]) {

				if (song.flexion_length[i] < 4) {
					Debug.Log("A song.flexion_length is too short. It is ignored");
				}
				else {

				
				flexionDone[i] = flexTunnel(_cubeCone1Array,_cubeCone2Array,Mathf.Sin ((Time.time-song.flexion_t[i])*1.0f)/100,	
				                            Mathf.Sin ((Time.time-song.flexion_t[i])*freqFactorY[i])/100, i);
				}
			}
		}

		// --------------------------------***************---------------------------------
		// --------------------------------------------------------------------------------
		// 

		//On overwrite audioProcessor et remplace par le micro
		if(Time.time > song.songTime)
		{
			micProcessor.enabled = true;
			audioProcessor = micProcessor;
		}

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
			scale = 120f;
			break;
		case 1 :
			cuton = 16;
			cutoff = 128;
			scale = 720f;
			break;
		case 2 :
			cuton = 512;
			cutoff = 1024;
			scale = 12000f;
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
		int idxGraves = 3*largeur/4;
		int idxMidLeft = largeur/2;
		int idxAigues = largeur/4;
		float ampli = 0.1f;
		float g = 0f;
		float r = 0f;
		float b = 0f;	
		float timeSin=Mathf.Sin(Time.time/7f);
		float fastTime=Mathf.Sin(Time.time*2);

		///Valeurs connues:
		/// MidsR:
		cubes[idxMidRight,0].lastScale = cubes[idxMidRight,0].transform.localScale.y;
		cubes[idxMidRight,0].transform.localScale = new Vector3( cubes[idxMidRight,0].jWidth, hauteurMoyennes, 1f) ;
		cubes[idxMidRight,0].lastColor = cubes[idxMidRight,0].renderer.material.GetColor("_Color");
		ampli = cubes[idxMidRight,0].transform.localScale.y;
		g =  ((timeSin*0.7f) * (ampli/6f))-0.4f;
		r = (ampli/6f) - (g);
		b = 0.8f - (ampli/3);	
		cubes[idxMidRight,0].renderer.material.SetColor("_Color", new Color(r,g,b));


		//MidsL
		cubes[idxMidLeft,0].lastScale = cubes[idxMidLeft,0].transform.localScale.y;
		cubes[idxMidLeft,0].transform.localScale = new Vector3( cubes[idxMidLeft,0].jWidth, hauteurMoyennes, 1f) ;
		cubes[idxMidLeft,0].lastColor = cubes[idxMidLeft,0].renderer.material.GetColor("_Color");
		ampli = cubes[idxMidLeft,0].transform.localScale.y;
		g = ((timeSin*0.7f) * (ampli/6f))-0.4f;
		r = (ampli/6f) - (0.8f*g);
		b = 0.8f - (ampli/3);	
		cubes[idxMidLeft,0].renderer.material.SetColor("_Color", new Color(r,g,b));

		// Graves:
		cubes[idxGraves,0].lastScale = cubes[idxGraves,0].transform.localScale.y;
		cubes[idxGraves,0].transform.localScale = new Vector3( cubes[idxGraves,0].jWidth, hauteurBasses, 1f) ;
		cubes[idxGraves,0].lastColor = cubes[idxGraves,0].renderer.material.GetColor("_Color");
		ampli = cubes[idxGraves,0].transform.localScale.y;
		g = ((0.8f - ((timeSin*0.7f) * (ampli/6)))-0.2f);
		r = (ampli/4f) - (0.8f*g);
		b = 0.8f - (ampli/3);	
		cubes[idxGraves,0].renderer.material.SetColor("_Color", new Color(r,g,b));

		// Aigues:
		cubes[idxAigues,0].lastScale = cubes[idxAigues,0].transform.localScale.y;
		cubes[idxAigues,0].transform.localScale = new Vector3( cubes[idxAigues,0].jWidth, hauteurAigues, 1f) ;
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
		int dist = idxAigues - idxMidRight;
		for(int i = idxMidRight+1; i<idxAigues; i++){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;

			prop1 = (1f - (float)(i-idxMidRight)/dist);
			prop2 = 1f-prop1;
  			scale = (prop1*hauteurMoyennes) + (prop2*hauteurAigues);
			cubes[i,0].transform.localScale = new Vector3( cubes[i,0].jWidth, scale, 1f) ;

			r = prop1*cubes[idxMidRight,0].renderer.material.GetColor("_Color").r + prop2*cubes[idxAigues,0].renderer.material.GetColor("_Color").r;
			g = prop1*cubes[idxMidRight,0].renderer.material.GetColor("_Color").g + prop2*cubes[idxAigues,0].renderer.material.GetColor("_Color").g;
			b = prop1*cubes[idxMidRight,0].renderer.material.GetColor("_Color").b + prop2*cubes[idxAigues,0].renderer.material.GetColor("_Color").b;
			cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
			cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));

		}

		dist = idxMidLeft - idxAigues;
		for(int i = idxAigues+1; i<idxMidLeft; i++){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;

			prop1 = (1f - (float)(i-idxAigues)/dist);
			prop2 = 1f-prop1;
			scale = (prop1*hauteurAigues) + (prop2*hauteurMoyennes);
			cubes[i,0].transform.localScale = new Vector3( cubes[i,0].jWidth, scale, 1f) ;


			r = prop1*cubes[idxAigues,0].renderer.material.GetColor("_Color").r + prop2*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").r;
			g = prop1*cubes[idxAigues,0].renderer.material.GetColor("_Color").g + prop2*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").g;
			b = prop1*cubes[idxAigues,0].renderer.material.GetColor("_Color").b + prop2*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").b;
			cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
			cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));
		}

		dist = idxGraves - idxMidLeft;
		for(int i = idxMidLeft+1; i<idxGraves; i++){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;

			prop1 = (1f - (float)(i-idxMidLeft)/dist);
			prop2 = 1f-prop1;
			scale = (prop1*hauteurMoyennes) + (prop2*hauteurBasses);
			cubes[i,0].transform.localScale = new Vector3( cubes[i,0].jWidth, scale, 1f) ;

			r = prop1*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").r + prop2*cubes[idxGraves,0].renderer.material.GetColor("_Color").r;
			g = prop1*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").g + prop2*cubes[idxGraves,0].renderer.material.GetColor("_Color").g;
			b = prop1*cubes[idxMidLeft,0].renderer.material.GetColor("_Color").b + prop2*cubes[idxGraves,0].renderer.material.GetColor("_Color").b;
			cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
			cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));
		}

		dist = largeur - idxGraves;
		for(int i = idxGraves+1; i<largeur; i++){
			cubes[i,0].lastScale = cubes[i,0].transform.localScale.y;

			prop1 = (1f - (float)(i-idxGraves)/dist);
			prop2 = 1f-prop1;
			scale = (prop1*hauteurBasses) + (prop2*hauteurMoyennes);
			cubes[i,0].transform.localScale = new Vector3( cubes[i,0].jWidth, scale, 1f) ;

			r = prop1*cubes[idxGraves,0].renderer.material.GetColor("_Color").r + prop2*cubes[idxMidRight,0].renderer.material.GetColor("_Color").r;
			g = prop1*cubes[idxGraves,0].renderer.material.GetColor("_Color").g + prop2*cubes[idxMidRight,0].renderer.material.GetColor("_Color").g;
			b = prop1*cubes[idxGraves,0].renderer.material.GetColor("_Color").b + prop2*cubes[idxMidRight,0].renderer.material.GetColor("_Color").b;
			cubes[i,0].lastColor = cubes[i,0].renderer.material.GetColor("_Color");
			cubes[i,0].renderer.material.SetColor("_Color", new Color(r,g,b));
		}

	}

	void CopyFirstRow(CubeInfo[,] original, CubeInfo[,] copy, int z0, int nZ){
		for (int i = 0; i<original.GetLength(0); i++) {
			for (int j = z0; j<(z0+nZ); j++) {
				copy[i,j].lastScale = original[i,0].lastScale;
				copy[i,j].transform.localScale = original[i,0].transform.localScale;
				copy[i,j].lastColor = original[i,0].lastColor;
				copy[i,j].renderer.material.SetColor("_Color",original[i,0].renderer.material.GetColor("_Color"));

			}
		}
	}

	void ApplyScaleWave(CubeInfo[,] cubes){

		for(int i = 0; i<cubes.GetLength(0); i++){
			for(int j = 1; j<cubes.GetLength(1); j++)	{
				//Le jratio sert a prendre en compte l'effet conique. Si on ne divisait pas par jratio, 
				//on aurait l'impression que les scale augmente avec la profondeur
				cubes[i,j].lastScale = cubes[i,j].transform.localScale.y / cubes[i,j].jRatio;
				cubes[i,j].transform.localScale = new Vector3( cubes[i,j].jWidth, cubes[i,j].jRatio * cubes[i,j-1].lastScale, 1f) ;
			}
		}
	} 

	private bool flexTunnel(CubeInfo[,] cubes1,CubeInfo[,] cubes2, float sinOffsetX, float sinOffsetY, int flexionID)  {	

		bool flexionDone = false;
		float amp =1000;

		// Si c'est la premiere frame de cette flexion, avertir TunnelSpinner pour qu'il cesse d'enregistrer la position
		if (firstFlexFrame[flexionID]==true) {
			firstFlexFrame[flexionID] = false;
			TunnelSpinner.startFlexion();
		}

		if ((Time.time - song.flexion_t[flexionID])<Mathf.Min (2,song.flexion_length[flexionID]/3)) {
			// La force d'amplication du sinus commence a 0 et augmente jusqu'a 4 
			amp = (Time.time-song.flexion_t[flexionID])/2;

		}
		else if ((Time.time - song.flexion_t[flexionID])<(song.flexion_length[flexionID]-4))  {
			
			amp =1;
		}
		else {
			// La force d'amplication du sinus diminue jusqu'a 0 (parabole inversée)
			amp = Mathf.Max ((song.flexion_length[flexionID]-(Time.time-song.flexion_t[flexionID]))/4,0);
		}

		for(int i = 0; i<cubes1.GetLength(0); i++){
			for(int j = 1; j<cubes1.GetLength(1); j++){
				cubes1[i,j].transform.position = new Vector3 ((cubes1[i,j].posSansFlexion.x+ amp*Mathf.Pow(j,2)*sinOffsetX),cubes1[i,j].posSansFlexion.y + amp*Mathf.Pow(j,2)*sinOffsetY,cubes1[i,j].transform.position.z);
			}
		}
		for(int i = 0; i<cubes2.GetLength(0); i++){
			for(int j = 1; j<cubes2.GetLength(1); j++){
				cubes2[i,j].transform.position = new Vector3 ((cubes2[i,j].posSansFlexion.x+ amp*Mathf.Pow(j,2)*sinOffsetX),cubes2[i,j].posSansFlexion.y+ amp*Mathf.Pow(j,2)*sinOffsetY,cubes2[i,j].transform.position.z);
			}
		}
		// Stop the flexion script when amp and the offset are small
		if (amp < 0.0001f){	
			flexionDone = true;
			ApplyDefaultPosition(_cubeCone1Array);
			ApplyDefaultPosition(_cubeCone2Array);
		}
		return flexionDone;
	}
	

	void ApplyDefaultPosition(CubeInfo[,] cubes)  {	
		TunnelSpinner.endFlexion();
		for(int i = 0; i<cubes.GetLength(0); i++){
			for(int j = 1; j<cubes.GetLength(1); j++){
				cubes[i,j].transform.position = cubes[i,j].posSansFlexion;

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
