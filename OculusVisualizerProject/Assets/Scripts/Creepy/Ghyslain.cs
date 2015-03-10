using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghyslain : MonoBehaviour {

	public List<gSphere> sphereList = new List<gSphere>();
	public Material sphereMat;
	public GameObject Eclair;
	public GameObject evilLight;

	private List<GameObject> sphereInRangeList  = new List<GameObject>();		//All spheres that are in range but not pulled in Ghyslain gravity
	private float timerOnAddSphere = 2;
	private float timer = 0;

	// pour l'initialisation
	private GameObject uneSphere;
	private int iniSphereNb = 13;
	private float iniSpherePos;
	//private float iniExcentricite;
	private float iniAttractionForceXZ;

	// pour l'elimination
	private float criticalDist = 200f;
//	private List<int> sphereToDeleteList = new List<int>();
	
	// parametres qui influencent la force
	private Vector3 relDist;
	private float forceChangeRate = 250f;
	private float topSpeed = 15f;
	private float cDrag = 0.5f;

	private float temps1 = 5;
	
	// Use this for initialization
	void Start () {

		for (int i=0; i<iniSphereNb; i++) {
			//Création d'une sphere
			uneSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			uneSphere.renderer.material = sphereMat;
			iniSpherePos = Random.Range(10f,50f);
			
			//La position de la sphere est variante en x seulement (plus facile pour savoir quelle vitesse lui appliquer pour qu'elle entre en orbite
			uneSphere.transform.position = new Vector3(transform.position.x+iniSpherePos, transform.position.y+i, transform.position.z);
			uneSphere.transform.localRotation = Quaternion.identity;
			uneSphere.transform.localScale = new Vector3(1f, 1f, 1f);
			uneSphere.AddComponent<Rigidbody>();
			uneSphere.rigidbody.angularDrag = 0;
			uneSphere.rigidbody.mass = 1;
			uneSphere.gameObject.tag = "ActiveSphere";
			
			// On donne a la sphere une vitesse en z avec direction aléatoirement positive ou négative
//			iniExcentricite = Random.Range(0,1f);
			iniAttractionForceXZ = Random.Range(500f,1000f);
			uneSphere.rigidbody.velocity = new Vector3(0,0, Mathf.Sqrt(iniAttractionForceXZ/iniSpherePos));
			sphereList.Add ( new gSphere(uneSphere, Random.Range(5f,10f), Random.Range(50f,75f), iniAttractionForceXZ ) );
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		// timerOnAddSphere varies with the number of spheres in the list sphereInRangeList
		if (sphereInRangeList.Count>0) {
			timer += Time.deltaTime;
			relDist = transform.position - sphereInRangeList[0].transform.position;
			timerOnAddSphere = relDist.magnitude/5-5;

			if (timer > timerOnAddSphere )	{
				timer = 0;
				PullSphere(sphereInRangeList[0]);
				sphereInRangeList.RemoveAt(0);
			}
		}


		// Mouvement simple de Ghyslain pour tester le ontriggerCollider
		transform.position = new Vector3 (transform.position.x + 1f*Time.deltaTime, transform.position.y, transform.position.z);
		
		
		for (int i = sphereList.Count-1; i >= 0; i--) {

			//Calcul de la distance relative de la sphere avec Ghyslain
			relDist = transform.position - sphereList [i].go.transform.position;


			if (relDist.magnitude > criticalDist ){
			Destroy( sphereList[i].go );
			sphereList.Remove( sphereList[i] );
			Debug.Log ("Sphere destroyed cause it was out of range");
			}
			
			// forece d'attraction
			AddForceXZ ( relDist, sphereList[i] );
			AddForceY ( relDist, sphereList[i] );
			AddDrag( sphereList[i] );
			
			
			// anti gravite
			//sphereList[i].go.rigidbody.AddForce (0, Mathf.Max (9, 100/(relDist.y + 0.5f) ),0);
		}


		// Constantly dim the lights
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("EvilLight");
		foreach (GameObject eLight in gos) {
			eLight.light.intensity = Mathf.Lerp ( eLight.light.intensity,0,Time.deltaTime*3);
			eLight.light.range = Mathf.Lerp ( eLight.light.range,0,Time.deltaTime*3);	
			
		}
		// Un éclair!
		if (Time.realtimeSinceStartup > temps1)	{
			CoupDeTonnerre();
			temps1 = temps1+Random.Range (4,20);			
		}
	}

	IEnumerator WaitAndDestroy(float waitTime,GameObject gameObject) {
		yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);
	}

	private void CoupDeTonnerre(){


		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("EvilLight");
		foreach (GameObject eLight in gos) {
			eLight.light.intensity = 2;
			eLight.light.range = 500;	
			
		}

		for (int j = 0; j < Random.Range(1,4); j++) {
			GameObject unEclair = (GameObject)Instantiate (Eclair);
			LineRenderer elcairLR = unEclair.GetComponent<LineRenderer>();
			
			elcairLR.SetPosition( 0, transform.position);
			
			for (int i = 1; i < 10; i++) {
				elcairLR.SetPosition( i, new Vector3(transform.position.x + Random.Range (-5,5),i*10-1,transform.position.z + Random.Range (-5,5)));
			}
			StartCoroutine(WaitAndDestroy(0.1f,unEclair));
		}
	}
	// Calcul de la force radiale --------------------------------------------------------------
	private void AddForceXZ( Vector3 relDist, gSphere sphere){
		// variables
		Vector2 relDistXZ = new Vector2 (relDist.x, relDist.z);
		Vector2 forceXZ = new Vector2(0f,0f);

		// Pour eviter que la force explose
		if (relDistXZ.magnitude < sphere.minDist) {
			sphere.attractionForceXZ -= forceChangeRate * Time.deltaTime;
			relDistXZ = sphere.minDist * relDistXZ / relDistXZ.magnitude;
		}
		// Augmentation de la force
		else if (relDistXZ.magnitude > sphere.maxDist) {
			sphere.attractionForceXZ += forceChangeRate * Time.deltaTime;
			relDistXZ = sphere.maxDist * relDistXZ / relDistXZ.magnitude;
		}

		// Calcul et application de la force
		forceXZ = sphere.attractionForceXZ / Mathf.Pow (relDistXZ.magnitude, 3f) * relDistXZ;
		sphere.go.rigidbody.AddForce( new Vector3(forceXZ.x,0f,forceXZ.y) );
	}

	// Calcul de la force en y-----------------------------------------------------------------
	private void AddForceY( Vector3 relDist, gSphere sphere){
		// variables
		float upForce = 5f;
		float downForce = -5f;

		// relDistMag vaut 0 si relDist < minDist et 1 si relDist > maxDist
		float relDistMag = Mathf.Clamp(relDist.magnitude, sphere.minDist, sphere.maxDist) - sphere.minDist;
		relDistMag /= sphere.maxDist - sphere.minDist;

		// On applique une force verticale interpolee entre unForce et downForce
		float forceY = upForce * (1f - relDistMag) + downForce * relDistMag;
		// Terme anti gravite
		forceY += 9.81f;
		// Terme de rebond
		if (relDist.y < 0)
			forceY += 1f;
		// Pour que les spheres tombent lentement
		if (sphere.go.rigidbody.velocity.y < 0)
			sphere.go.rigidbody.drag = 1;
		else
			sphere.go.rigidbody.drag = 0;

		sphere.go.rigidbody.AddForce( new Vector3(0f, forceY, 0f) );
	}

	// Calcul du drag---------------------------------------------------------------------------
	private void AddDrag( gSphere sphere){
		// Variables
		Vector3 dragForce;

		// Il n'y a de drag qu'au dessus d'une vitesse seuil
		if (sphere.go.rigidbody.velocity.magnitude < topSpeed)
			return;

		// Drag en v carre
		dragForce = - cDrag * sphere.go.rigidbody.velocity.magnitude / topSpeed *
			sphere.go.rigidbody.velocity / topSpeed;
		sphere.go.rigidbody.AddForce( dragForce );
	}

//	// routine pour supprimer les pheres perdues
//	private void DeleteSpheres(  List<gSphere> sphereList, List<int> sphereToDeleteList ){
//		for (int i=0; i<sphereToDeleteList.Count; i++) {
//			Destroy( sphereList[sphereToDeleteList[i]].go );
//			sphereList.Remove( sphereList[sphereToDeleteList[i]] );
//			Debug.Log ("Sphere destroyed cause it was out of range");
//		}
//		sphereToDeleteList = new List<int> ();
//	}

	// Add a new sphere to Gyslain gravitational pull
	public void PullSphere(GameObject uneSphere)  {
		uneSphere.transform.rigidbody.isKinematic = false;
		sphereList.Add (new gSphere(uneSphere,Random.Range (3,8),Random.Range (22,35), Random.Range (1500f,2500f)));

	}

	// Add a new sphere. A soft pull will attrack some spheres in range
	public void SphereInRange(GameObject uneSphere)  {
		sphereInRangeList.Add (uneSphere);
	}

	/*
	// Add a new sphere. A soft pull will attrack some spheres in range
	public void SphereOutOfRange(GameObject uneSphere)  {
		sphereInRangeList.Remove (uneSphere);
	}
	*/
}

