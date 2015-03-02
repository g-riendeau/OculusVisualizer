﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghyslain : MonoBehaviour {

	public List<gSphere> sphereList = new List<gSphere>();
	public Material sphereMat;
	public GameObject Eclair;
	public GameObject evilLight;

	private List<GameObject> sphereInRangeList  = new List<GameObject>();		//All spheres that are in range but not pulled in Ghyslain gravity
	private float timerOnAddSphere = 2000;
	private float timer = 0;
	private int pulledSphere;

	// pour l'initialisation
	private GameObject uneSphere;
	private int iniSphereNb = 13;
	private float iniSpherePos;
	private float iniExcentricite;
	private float iniAttractionForceXZ;

	// pour l'elimination
	private float criticalDist = 200f;
//	private List<int> sphereToDeleteList = new List<int>();
	
	// parametres qui influencent la force
	private Vector3 relDist;
	private float forceChangeRate = 500f;
	private float topSpeed = 10f;
	private float cDrag = 0.5f;

	private float temps1 = 5;
	
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
			uneSphere.gameObject.tag = "ActiveSphere";
			
			// On donne a la sphere une vitesse en z avec direction aléatoirement positive ou négative
			iniExcentricite = Random.Range(0,1.5f);
			iniAttractionForceXZ = Random.Range(1500f,2500f);
			uneSphere.rigidbody.velocity = new Vector3(0,0, (0.5f+iniExcentricite) * Mathf.Sqrt(iniAttractionForceXZ/iniSpherePos));
			sphereList.Add ( new gSphere(uneSphere, Random.Range(3f,8f), Random.Range(22f,35f), iniAttractionForceXZ ) );
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		// timerOnAddSphere varies with the number of spheres in the list sphereInRangeList
		timer += Time.deltaTime;
		if (timer > timerOnAddSphere)	{
			timer = 0;
			pulledSphere = Random.Range( 0, sphereInRangeList.Count );
			PullSphere(sphereInRangeList[pulledSphere]);
			sphereInRangeList.RemoveAt(pulledSphere);
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
			//AddDrag( sphereMat[i] );
			
			
			// anti gravite
			//sphereList[i].go.rigidbody.AddForce (0, Mathf.Max (9, 100/(relDist.y + 0.5f) ),0);
			sphereList[i].go.rigidbody.AddForce (0, 9.81f,0);
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

	// calcul de la force radiale
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
		forceXZ = sphere.attractionForceXZ / Mathf.Pow (relDistXZ.magnitude, 3f) * relDistXZ;
		sphere.go.rigidbody.AddForce( new Vector3(forceXZ.x,0f,forceXZ.y) );
	}

	private void AddDrag( gSphere sphere){
	
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
		sphereInRangeList.RemoveAt(pulledSphere);
	}

	// Add a new sphere. A soft pull will attrack some spheres in range
	public void SphereInRange(GameObject uneSphere)  {
		sphereInRangeList.Add (uneSphere);
		timerOnAddSphere = Mathf.Clamp(1,50 / Mathf.Pow (sphereInRangeList.Count,0.5f),20);
		//Debug.Log (timerOnAddSphere);

	}

}
