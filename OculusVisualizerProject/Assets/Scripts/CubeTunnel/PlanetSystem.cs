using UnityEngine;
using System.Collections;

public class PlanetSystem : MonoBehaviour {

	public Song song;
	public Material FloorMat;

	private const int nCubes = 32;
	private const int nFois = 2;
	public CubeInfo[,] cubePlanets = new CubeInfo[ nCubes, nFois ];

	public bool supernovae = false;
	private float gravite = 1000f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (song.time () >= song.supernovae) {
			// On creer les planetes
			if (!supernovae) {
				CreatePlanets ();
			// On applique la force de gravite
			} else {
				for (int i = 0; i < nCubes; i++){	
					for (int j = 0; j < nFois; j++){
						if( cubePlanets[i,j].transform.localPosition.magnitude > 5f && cubePlanets[i,j].transform.localPosition.magnitude < 100f){
							cubePlanets[i,j].cubeRB.AddForce( - gravite * cubePlanets[i,j].transform.localPosition / 
						                                      Mathf.Pow ( cubePlanets[i,j].transform.localPosition.magnitude, 3f ) );
						}
					}
				}
			}
		}
	}

	private void CreatePlanets(){
		float radius;
		float phi;
		float theta;
		float vx;
		float vy;
		float vz;
		float Cnorm;

		// APPARITION DES PLANETES --------------------------------------------------------------------
		for (int i = 0; i < nCubes; i++){	
			for (int j = 0; j < nFois; j++){
				
				// on creer et place le cube
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = this.transform;

				// initialisation des positions
				radius = Random.Range (10f,30f);
				phi = Random.Range (0f,2f) * Mathf.PI;
				theta = Random.Range (1f,2f) *Mathf.PI/3f;
				cube.transform.localPosition = new Vector3( radius * Mathf.Sin (theta) * Mathf.Cos (phi),
				                                            radius * Mathf.Sin (theta) * Mathf.Sin (phi),
				                                            radius * Mathf.Cos (theta) );
				cube.transform.localScale = new Vector3( Random.Range (1f,4f), 0.1f, Random.Range(1f,4f) );
				cube.GetComponent<Renderer>().material = FloorMat;
				cube.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f,1f,1f));
				
				
				// rotation du cube
				Quaternion target = Quaternion.Euler( Random.Range (0f,360f), Random.Range (0f,360f), Random.Range(0f,360f) );
				cube.transform.localRotation = target;

				// pas de collisions
				cube.GetComponent<Collider>().enabled = false;
								
				// ajout de CubeInfo
				cube.AddComponent<CubeInfo>();
				cubePlanets[i,j] = cube.GetComponent<CubeInfo>();
				cubePlanets[i,j].jRatio = 1f;
				cubePlanets[i,j].jWidth = cube.transform.localScale.x;
				cubePlanets[i,j].posSansFlexion = cube.transform.localPosition;
				cubePlanets[i,j].lastScale = 0.1f;
				cubePlanets[i,j].lastColor = cube.GetComponent<Renderer>().material.color;

				// ajout de rigidbody
				cube.AddComponent<Rigidbody>();
				cubePlanets[i,j].cubeRB = cube.GetComponent<Rigidbody>();

				// initialisation des vitesses
				cubePlanets[i,j].cubeRB.angularVelocity.Set( Random.Range (0f,360f) * Time.fixedDeltaTime, Random.Range (0f,360f) * Time.fixedDeltaTime,
				                                    Random.Range (0f,360f) * Time.fixedDeltaTime );
				vx = Random.Range (1f,10f);
				vy = -vx*cube.transform.localPosition.x /cube.transform.localPosition.y;
				Cnorm = Mathf.Sqrt( gravite / ( cube.transform.localPosition.magnitude * (vx*vx+vy*vy) ) );
				cubePlanets[i,j].cubeRB.velocity = new Vector3 ( Cnorm*vx, Cnorm*vy, 0f );

				// pas de gravite
				cubePlanets[i,j].cubeRB.useGravity = false;
			}
		}
		supernovae = true;
	}
}
