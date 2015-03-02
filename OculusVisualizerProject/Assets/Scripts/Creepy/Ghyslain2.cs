using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghyslain2 : MonoBehaviour {
	
	public List<gSphere> sphereList = new List<gSphere>();
	public Material sphereMat;

	// pour l'initialisation
	private GameObject uneSphere;
	private int iniSphereNb = 13;
	private float iniSpherePos;
	private float iniExcentricite;
	private float iniAttractionForceXZ;

	// pour l'elimination
	private float criticalDist = 200f;
	private List<int> sphereToDeleteList = new List<int>();
	
	// parametres qui influencent la force
	private Vector3 relDist;
	private float forceChangeRate = 20f;
	private float topSpeed = 10f;
	private float cDrag = 0.5f;

	
	// Use this for initialization
	void Start () {

		for (int i=0; i<iniSphereNb; i++) {
			//Création d'une sphere
			uneSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			uneSphere.renderer.material = sphereMat;
			iniSpherePos = Random.Range(10f,20f);

			//La position de la sphere est variante en x seulement (plus facile pour savoir quelle vitesse lui appliquer pour qu'elle entre en orbite
			uneSphere.transform.position = new Vector3(transform.position.x+iniSpherePos, transform.position.y+i, transform.position.z);
			uneSphere.transform.localRotation = Quaternion.identity;
			uneSphere.transform.localScale = new Vector3(1f, 1f, 1f);
			uneSphere.AddComponent<Rigidbody>();
			uneSphere.rigidbody.angularDrag = 0;
			uneSphere.rigidbody.mass = 1;
			
			// On donne a la sphere une vitesse en z avec direction aléatoirement positive ou négative
			iniExcentricite = Random.Range(-1,1);
			iniAttractionForceXZ = Random.Range(250f,350f);
			if ( iniExcentricite > 0f){
				uneSphere.rigidbody.velocity = new Vector3(0,0, (0.5f+iniExcentricite) * Mathf.Sqrt(iniAttractionForceXZ/iniSpherePos));
			}
			else{
				uneSphere.rigidbody.velocity = new Vector3(0,0,(-0.5f+iniExcentricite) * Mathf.Sqrt(iniAttractionForceXZ/iniSpherePos));
			}
			
			sphereList.Add ( new gSphere(uneSphere, Random.Range(3f,8f), Random.Range(22f,35f)), iniAttractionForceXZ );
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		// Mouvement simple de Ghyslain pour tester le ontriggerCollider
		transform.position = new Vector3 (transform.position.x + 1f*Time.deltaTime, transform.position.y, transform.position.z);
		
		
		for (int i = 0; i<sphereList.Count; i++) {
			//Calcul de la distance relative de la sphere avec Ghyslain
			relDist = transform.position - sphereList [i].go.transform.position;
			

			// Destruction de la sphere si elle se perd
			if (relDist.magnitude > criticalDist ){
				sphereToDeleteList.Add(i);
			}


			// forece d'attraction
			AddForceXZ ( relDist, sphereList[i] );
	

			// anti gravite
			sphereList[i].go.rigidbody.AddForce (0, Mathf.Max (10, 100/(relDist.y + 0.1f) ),0);

		}

		DeleteSpheres( sphereList, sphereToDeleteList );
		
	}

	

	private void AddForceXZ( Vector3 relDist, gSphere sphere){
		Vector2 relDistXZ = new Vector2 (relDist.x, relDist.z);
		Vector2 forceXZ = new Vector2(0f,0f);

		if (relDistXZ.magnitude < sphere.minDist) {
			sphere.attractionForceXZ -= forceChangeRate * Time.deltaTime;
			if (sphere.attractionForceXZ < 0f)
				sphere.attractionForceXZ = 0f;
		}
		else if (relDistXZ.magnitude > sphere.maxDist) {
			sphere.attractionForceXZ += forceChangeRate * Time.deltaTime;
		}
		forceXZ = iniAttractionForceXZ / Mathf.Pow (relDistXZ.magnitude, 3f) * relDistXZ;
		sphere.go.rigidbody.AddForce( new Vector3(forceXZ.x,0f,forceXZ.y) );
	}
	
	private void DeleteSpheres(  List<gSphere> sphereList, List<int> sphereToDeleteList ){
		for (int i=0; i<sphereToDeleteList.Count; i++) {
			Destroy( sphereList[sphereToDeleteList[i]].go );
			sphereList.Remove( sphereToDeleteList[i] );
			Debug.Log ("Sphere destroyed cause it was out of range");
		}
		sphereToDeleteList = new List<int> ();
	}

	public void AddThisSphere(GameObject uneSphere)  {
		uneSphere.transform.rigidbody.isKinematic = false;
		sphereList.Add (new gSphere(uneSphere,Random.Range (3,8),Random.Range (22,35), Random.Range (250f,350f)) );
		
	}
	
}
