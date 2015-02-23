using UnityEngine;
using System.Collections;

public class CubeCylinder : MonoBehaviour {


	public Material FloorMat;

	private const float nCubes = 24f;   // nb de cubes sur la circonference
	private const float nZ = 31f;       // nb de cubes de profondeur
	private const float cRadius = 4f;   // rayon du cylindre
	private const float cHole = 0.1f;   // fraction restante du rayon
	public CubeInfo[,] cubeArray = new CubeInfo[(int)nCubes,(int)nZ];
	
	private float height = 0.1f;
	private float dTheta = 2f * Mathf.PI/nCubes;
	private float distZ = 0f;
	private float dAlpha = -180f*Mathf.Atan((1f - cHole) * cRadius / nZ)/Mathf.PI;
	
	private float jRatio = 1f;
	private float jRadius = 1f;
	private float jWidth = 1f;
	
	// Use this for initialization
	void Start() 
	{

		for (float j = 0f; j < nZ; j++)
		{
			jRatio = (j/(nZ-1f))*(cHole-1f) + 1f;
			jRadius = jRatio * cRadius;
			jWidth =  2f * jRadius * Mathf.Tan (dTheta/2f);
			distZ += jWidth;

			for (float i = 0f; i < nCubes; i++)//Sans espacement
			{

				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = this.transform;
				cube.transform.localPosition = new Vector3( jRadius * Mathf.Cos (i*dTheta),
				                                            jRadius * Mathf.Sin (i*dTheta),
				                                            j );


				cube.transform.localScale = new Vector3(jWidth, 0.1f, 1f);
				cube.renderer.material = FloorMat;


				Quaternion target = Quaternion.Euler(-dAlpha*Mathf.Sin (i*dTheta), dAlpha*Mathf.Cos (i*dTheta), i*360f/nCubes + 90f);
				cube.transform.localRotation = target;

				//Pour se retrouver
				if(i==0f)
				{
					cube.renderer.material.SetColor("_Color", new Color(1f, 1f, 1f));
				}


				
				CubeInfo ci = cube.AddComponent<CubeInfo>();
				cubeArray[(int)i,(int)j] = cube.GetComponent<CubeInfo>();
				cubeArray[(int)i,(int)j].jRatio = jRatio;
				cubeArray[(int)i,(int)j].jWidth = jWidth;

				
				// no collisions pour un plus faible temps de calcul
				cube.collider.enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
