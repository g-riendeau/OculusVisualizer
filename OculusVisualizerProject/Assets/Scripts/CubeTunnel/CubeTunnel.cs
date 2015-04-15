// Cube Tunnel genere le tunnel de blocs
using UnityEngine;
using System.Collections;

public class CubeTunnel : MonoBehaviour {


	public Material FloorMat;

	// parametres globaux
	private const float nCubes = 32f;  			 		// nb de cubes sur la circonference
	private const float cRadius = 4f;   				// rayon du cylindre
	private const float cHole = 0.05f;     		 		// fraction restante du rayon
	private const float height = 0.1f; 			 		// hauteur des cubes par defaut
	// note : les cubes sont de longueur 1

	// structures de cubes
	private const float nZCone = 32f;				    // nb de cubes de profondeur
	public CubeInfo[,] cubeCone1Array = new CubeInfo[(int)nCubes,(int)nZCone];
	public CubeInfo[,] cubeCone2Array = new CubeInfo[(int)nCubes,(int)nZCone];
	private const float nZCenter = 1f;
	public CubeInfo[,] cubeCenterArray = new CubeInfo[(int)nCubes, (int)nZCenter];
	//private const float nZCylinder = 16f;				// nb de cubes de profondeur
	//public CubeInfo[,] cubeCylinderArray = new CubeInfo[(int)nCubes,(int)nZCylinder];

	// variables publiques
	public static float dTheta = 2f * Mathf.PI/nCubes; 	// variation d'angle sur la circonference
	public static float dAlpha = -180f * Mathf.Atan((1f - cHole) * cRadius / nZCone)/Mathf.PI; // inclinaison du cone

	// variables locales                   		 		
	private float jRatio;						 		// facteur de reduction selon la profondeur dans le cone
	private float jRadius;								// rayon pour une profondeur donnee dans le cone
	private float jWidth;								// largeur des cubes pour une profondeur donnee
	
	// Use this for initialization
	void Awake() 
	{
		// CONSTRUCTION CONE 1 --------------------------------------------------------------------
		for (float j = 0f; j < nZCone; j++)
		{	
			// on assigne les valeurs pour la rangee
			jRatio  = (j/(nZCone-1f))*(cHole-1f) + 1f;
			jRadius = jRatio * cRadius;
			jWidth  =  2f * jRadius * Mathf.Tan (dTheta/2f);

			for (float i = 0f; i < nCubes; i++)
			{

				// on creer et place le cube
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = this.transform;
				cube.transform.localPosition = new Vector3( jRadius * Mathf.Cos (i*dTheta),
				                                            jRadius * Mathf.Sin (i*dTheta),
				                                            j + 1f );

				cube.transform.localScale = new Vector3(jWidth, 0.1f, 1f);
				cube.renderer.material = FloorMat;
				cube.renderer.material.SetColor("_Color", new Color(1f,1f,1f));


				// rotation du cube
				Quaternion target = Quaternion.Euler(-dAlpha*Mathf.Sin (i*dTheta), dAlpha*Mathf.Cos (i*dTheta), i*360f/nCubes + 90f);
				cube.transform.localRotation = target;

				// no collisions pour un plus faible temps de calcul
				cube.collider.enabled = false;

				// ajout de CubeInfo
				cube.AddComponent<CubeInfo>();
				cubeCone1Array[(int)i,(int)j] = cube.GetComponent<CubeInfo>();
				cubeCone1Array[(int)i,(int)j].jRatio = jRatio;
				cubeCone1Array[(int)i,(int)j].jWidth = jWidth;
				cubeCone1Array[(int)i,(int)j].posSansFlexion = cube.transform.localPosition;
				cubeCone1Array[(int)i,(int)j].lastScale = 0.1f;
				cubeCone1Array[(int)i,(int)j].lastColor = cube.renderer.material.color;

			}
		}

		// CONSTRUCTION CONE 2 --------------------------------------------------------------------
		for (float j = 0f; j < nZCone; j++)
		{	
			// on assigne les valeurs pour la rangee
			jRatio  = (j/(nZCone-1f))*(cHole-1f) + 1f;
			jRadius = jRatio * cRadius;
			jWidth  =  2f * jRadius * Mathf.Tan (dTheta/2f);
			
			for (float i = 0f; i < nCubes; i++)
			{
				
				// on creer et place le cube
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = this.transform;
				cube.transform.localPosition = new Vector3( jRadius * Mathf.Cos (i*dTheta),
				                                            jRadius * Mathf.Sin (i*dTheta),
				                                            -j - 1f );
				
				cube.transform.localScale = new Vector3(jWidth, 0.1f, 1f);
				cube.renderer.material = FloorMat;
				cube.renderer.material.SetColor("_Color", new Color(1f,1f,1f));
				
				
				// rotation du cube
				Quaternion target = Quaternion.Euler(dAlpha*Mathf.Sin (i*dTheta), -dAlpha*Mathf.Cos (i*dTheta), i*360f/nCubes + 90f);
				cube.transform.localRotation = target;
				
				// no collisions pour un plus faible temps de calcul
				cube.collider.enabled = false;
				
				// ajout de CubeInfo
				CubeInfo ci = cube.AddComponent<CubeInfo>();
				cubeCone2Array[(int)i,(int)j] = cube.GetComponent<CubeInfo>();
				cubeCone2Array[(int)i,(int)j].jRatio = jRatio;
				cubeCone2Array[(int)i,(int)j].jWidth = jWidth;
				cubeCone2Array[(int)i,(int)j].posSansFlexion = cube.transform.localPosition ;
				cubeCone2Array[(int)i,(int)j].lastScale = 0.1f;
				cubeCone2Array[(int)i,(int)j].lastColor = cube.renderer.material.color;
				
			}
		}

		// CONTRUCTION CENTRE ----------------------------------------------------------------
		for (float j = 0f; j < nZCenter; j++)
		{
			// on assigne les valeurs pour la rangee
			jRatio  = 1f;
			jRadius = jRatio * cRadius + 0.5f * Mathf.Sin ( -dAlpha*Mathf.PI/180f );
			jWidth  =  2f * jRadius * Mathf.Tan (dTheta/2f);
			
			for (float i = 0f; i < nCubes; i++)
			{
				
				// on creer et place le cube
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = this.transform;
				cube.transform.localPosition = new Vector3( jRadius * Mathf.Cos (i*dTheta),
				                                      jRadius * Mathf.Sin (i*dTheta),
				                                      j );
				
				cube.transform.localScale = new Vector3(jWidth, 0.1f, 1f);
				cube.renderer.material = FloorMat;
				cube.renderer.material.SetColor("_Color", new Color(1f,1f,1f));
				
				// rotation du cube
				Quaternion target = Quaternion.Euler(0f, 0f, i*360f/nCubes + 90f);
				cube.transform.localRotation = target;
				
				// no collisions pour un plus faible temps de calcul
				cube.collider.enabled = false;
				
				// ajout de CubeInfo
				CubeInfo ci = cube.AddComponent<CubeInfo>();
				cubeCenterArray[(int)i,(int)j] = cube.GetComponent<CubeInfo>();
				cubeCenterArray[(int)i,(int)j].jRatio = jRatio;
				cubeCenterArray[(int)i,(int)j].jWidth = jWidth;
				cubeCenterArray[(int)i,(int)j].posSansFlexion = cube.transform.localPosition;
				cubeCenterArray[(int)i,(int)j].lastScale = 0.1f;
				cubeCenterArray[(int)i,(int)j].lastColor = cube.renderer.material.color;
				
			}
		}

	}

}
