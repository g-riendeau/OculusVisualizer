using UnityEngine;
using System.Collections;

public class CubeCylinder : MonoBehaviour {

	public CubeInfo[,] cubeArray = new CubeInfo[(int)cWidth,(int)cDepth];
	public Material FloorMat;
	//private AudioProcessor audioProcessor = new AudioProcessor();
	private const float nTheta = 60f;   // nb de cubes sur la circonference
	private const float nZ = 20f;       // nb de cubes de profondeur
	private const float cRadius = 10f;  // rayon du cylindre
	private const float cHole = 0.1f;   // fraction restante du rayon

	
	float width = 2f * cRadius * Mathf.Sin (180f / nTheta);
	float height = 0.1f;
	float dTheta = 360f / (nTheta - 1f);
	float dAlpha = (1f - cHole) * cRadius / (width * nZ);

	// Use this for initialization
	void Start() 
	{
		//On veut des indice entiers
		for (float i = 0f; i < nTheta; i++)//Sans espacement
		{
			for (float j = 0f; j < nZ; j++)
			{
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = this.transform;
				cube.transform.localPosition = new Vector3( ((j/nZ)*(cHole-1f)+1f)*cRadius * Mathf.Cos (i*dTheta),
				                                            ((j/nZ)*(cHole-1f)+1f)*cRadius * Mathf.Sin (i*dTheta),
				                                            j*width );
				float tiltAroundZ = Input.GetAxis("Horizontal") * dTheta;
				float tiltAroundX = Input.GetAxis("Vertical") * dAlpha;
				Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
				cube.transform.localRotation = target;
				cube.transform.localScale = new Vector3(width, 0.1f, width);
				cube.renderer.material = FloorMat;

				
				CubeInfo info = cube.AddComponent<CubeInfo>();
				
				cubeArray[(int)i,(int)j] = cube.GetComponent<CubeInfo>();
				
				// no collisions pour un plus faible temps de calcul
				cube.collider.enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
