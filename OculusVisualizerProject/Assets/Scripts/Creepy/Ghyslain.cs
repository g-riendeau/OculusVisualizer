using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghyslain : MonoBehaviour {

	public List<gSphere> sphereList;

	public Material sphereMat;
	public GameObject Eclair;
	public GameObject evilLight;

	private int sphereNb;
	private GameObject uneSphere;
    private LineRenderer[] eclairs;
	private Vector3 relDist;
	private float temps1 = 5;
	private float iniSpherePos;

	// parametres qui influencent la force
	private float attractionForce = 300f;
	private float topSpeed = 10f;
	private float cDrag = 0.5f;

	// Use this for initialization
	void Start () {

		//Creating the listof spheres
		sphereList = new List<gSphere>();
		//sphereList = new List<GameObject>();

		attractionForce = 2000;
		sphereNb = 13;

		// On créé une matrices qui contient tous les objects spheres qui orbitent autour de Ghyslain
		//Spheres = new GameObject[sphereNb];
		eclairs = new LineRenderer[sphereNb];

		for (int i=0; i<sphereNb; i++) {

			//Création d'une sphere
			uneSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			uneSphere.renderer.material = sphereMat;
			iniSpherePos = Random.Range(10,20);

			//La position de la sphere est variante en x seulement (plus facile pour savoir quelle vitesse lui appliquer pour qu'elle entre en orbite
			uneSphere.transform.localPosition = new Vector3(transform.position.x+iniSpherePos, transform.position.y+i, transform.position.z);
			uneSphere.transform.localRotation = Quaternion.identity;
			uneSphere.transform.localScale = new Vector3(1f, 1f, 1f);
			uneSphere.AddComponent<Rigidbody>();
			uneSphere.rigidbody.angularDrag = 0;
			uneSphere.rigidbody.mass = 1;

			// On donne a la sphere une vitesse en z avec direction aléatoirement positive ou négative
			if (Random.Range(-1,1) < 0){
				uneSphere.rigidbody.velocity = new Vector3(0,0,400/uneSphere.transform.position.x);
			}
			else{
				uneSphere.rigidbody.velocity = new Vector3(0,0,-400/uneSphere.transform.position.x);
			}

			sphereList.Add (new gSphere(uneSphere,Random.Range (3,8),Random.Range (22,35)));
		
			/* Création d'un éclair entre chaque sphere p/r a Ghyslain
			GameObject unEclair = new GameObject();
			unEclair.AddComponent<LineRenderer>();
			eclairs[i] = unEclair.GetComponent<LineRenderer>();
			eclairs[i].SetPosition( 0, transform.position );
			eclairs[i].SetPosition( 1, spheres[i].transform.position );
			eclairs[i].SetWidth( 0.1f, 0.1f);
			*/
		}
	}

	// Update is called once per frame
	void Update () {

		// Mouvement simple de Ghyslain pour tester le ontriggerCollider
		transform.position = new Vector3 (transform.position.x+0.01f, transform.position.y, transform.position.z);


		for (int i = 0; i<sphereList.Count; i++) {
			//Calcul de la distance relative de la sphere avec Ghyslain
			relDist = transform.position - sphereList [i].go.transform.position;

			// kind of drag en v^4 mais pas dans la direction de la vitesse
			if( sphereList[i].go.rigidbody.velocity.magnitude > topSpeed ){
				sphereList[i].go.rigidbody.AddForce ( relDist / relDist.magnitude *
				                                     cDrag * Mathf.Pow (sphereList[i].go.rigidbody.velocity.magnitude/topSpeed , 4f) );
			}


			if (relDist.magnitude > 200){
				Destroy(sphereList[i].go);
				sphereList.Remove(sphereList[i]);
				Debug.Log ("Sphere destroyed cause it was out of range");

			}
			// force si la sphere s'éloigne trop
			else if (relDist.magnitude > sphereList[i].maxDist){
				sphereList[i].go.rigidbody.AddForce ((50*(relDist.magnitude-sphereList[i].maxDist)/relDist.magnitude+1)*attractionForce / Mathf.Pow (relDist.magnitude, 3f) * relDist);

			}
			// force d'attraction normal pour un corp en orbite
			else if (relDist.magnitude > sphereList[i].minDist){
				sphereList[i].go.rigidbody.AddForce (attractionForce / Mathf.Pow (relDist.magnitude, 3f) * relDist);
			}
			//La sphere est trop proche. Donner une force négligeable pour ne pas qu'elle reste coincé au milieu
			else {
				sphereList[i].go.rigidbody.AddForce (Vector3.Cross(new Vector3(0,10,0),relDist));	
			}
			// anti gravite
			sphereList[i].go.rigidbody.AddForce (0, Mathf.Max (10, 100/( Mathf.Pow(relDist.magnitude,1)+0.1f)),0);

			/*
			// force en 1/r^2
			spheres[i].rigidbody.AddForce (attractionForce / Mathf.Pow (relDist.magnitude, 3f) * relDist);
			// force de repulsion a courte distance en 1/r^5
			spheres[i].rigidbody.AddForce (- attractionForce / Mathf.Pow (relDist.magnitude, 6f) * relDist);
			// anti gravite
			spheres[i].rigidbody.AddForce (0, 10, 0);

			eclairs[i].SetPosition (0, transform.position);
			eclairs[i].SetPosition (1, spheres[i].transform.position);
*/
		}

		
		// Un éclair!
		if (Time.realtimeSinceStartup > temps1)	{

			evilLight.light.intensity = Mathf.Lerp (evilLight.light.intensity,8,Time.deltaTime*5);
			evilLight.light.range = Mathf.Lerp (evilLight.light.range,200,Time.deltaTime*5);

			for (int j = 0; j < Random.Range(1,4); j++) {
				GameObject unEclair = (GameObject)Instantiate (Eclair);
				LineRenderer elcairLR = unEclair.GetComponent<LineRenderer>();
				
				elcairLR.SetPosition( 0, new Vector3(0,-1,0));
				
				for (int i = 1; i < 10; i++) {
					elcairLR.SetPosition( i, new Vector3(Random.Range (-5,5),i*10-1,Random.Range (-5,5)));
				}
				StartCoroutine(WaitAndDestroy(0.1f,unEclair));
			}
			temps1 = temps1+Random.Range (2,15);			
			evilLight.light.intensity = Mathf.Lerp (evilLight.light.intensity,1,Time.deltaTime*50);
			evilLight.light.range =  Mathf.Lerp (evilLight.light.range,20,Time.deltaTime*50);
		}	
	}

	IEnumerator WaitAndDestroy(float waitTime,GameObject gameObject) {
		yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);

	}

	public void AddThisSphere(GameObject uneSphere)  {
		uneSphere.transform.rigidbody.isKinematic = false;
		sphereList.Add (new gSphere(uneSphere,Random.Range (3,8),Random.Range (22,35)));

	}

}
