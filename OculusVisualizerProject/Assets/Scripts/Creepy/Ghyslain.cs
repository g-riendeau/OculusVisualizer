using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ghyslain : MonoBehaviour {

	public List<activeObjet> objetList = new List<activeObjet>();
	public Material sphereMat;
	public GameObject Eclair;
	public GameObject evilLight;

	private List<passiveObjet> objetInRangeList  = new List<passiveObjet>();		//All spheres that are in range but not pulled in Ghyslain gravity
	private float timerOnAddObjet = 2;
	private float timer = 0;
	private float minEnergy = 1000000;
	private int minEnergyId;



	// pour l'initialisation
	private GameObject unObjet;
	private int iniObjetNb = 13;
	private float iniObjetPos;
	private float iniAttractionForceXZ;

	// pour l'elimination
	private float criticalDist = 500f;
//	private List<int> sphereToDeleteList = new List<int>();
	
	// parametres qui influencent la force
	private Vector3 yVector = new Vector3 (0f, 1f, 0f); 
	private Vector3 relDist;
	private float topSpeed = 15f;
	private float cDrag = 0.5f;
	private int objNbLowAlt = 10;  // Nombre d'objets qui restent à une altitude d'orbite basse p/r à Ghyslain

	private float tempsEclair = 5;

	// paramètres qui influencent le déplacement
	private bool reachedDestination;
	private GameObject destination;

	// Use this for initialization
	void Start () {
		reachedDestination = true;
		destination = null;
		/*

		for (int i=0; i<iniObjetNb; i++) {
			//Création d'une sphere
			unObjet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			unObjet.renderer.material = sphereMat;
			iniObjetPos = Random.Range(10f,50f);
			
			//La position de la sphere est variante en x seulement (plus facile pour savoir quelle vitesse lui appliquer pour qu'elle entre en orbite
			unObjet.transform.position = new Vector3(transform.position.x+iniObjetPos, transform.position.y+i, transform.position.z);
			unObjet.transform.localRotation = Quaternion.identity;
			unObjet.transform.localScale = new Vector3(1f, 1f, 1f);
			unObjet.AddComponent<Rigidbody>();
			unObjet.rigidbody.drag = 0;
			unObjet.rigidbody.angularDrag = 0;
			unObjet.rigidbody.mass = 1;
			unObjet.gameObject.tag = "ActiveObject";
			
			// On donne a la sphere une vitesse en z avec direction aléatoirement positive ou négative
//			iniExcentricite = Random.Range(0,1f);
			iniAttractionForceXZ = Random.Range(500f,1000f);
			unObjet.rigidbody.velocity = new Vector3( 0, 0, -Mathf.Sqrt(iniAttractionForceXZ/iniObjetPos));
			objetList.Add ( new activeObjet(unObjet, Random.Range(3f,8f), Random.Range(15f,35f), iniAttractionForceXZ, i) );

}
*/
	}

	// Update is called once per frame
	void FixedUpdate () {
// --------------------------------------------------------------------------
// ----------------------Ghyslain mouvement code-----------------------------
// --------------------------------------------------------------------------
		if (reachedDestination)  {
			// Trouve la prochaine destination
			destination = getNextDestination (transform.position);
			reachedDestination= false;			

		}
		else if (!reachedDestination && destination != null) {		
			// Se dirige vers la prochaine destination
			reachedDestination= Move (destination.transform.position, 2f);
		}
// --------------------------------------------------------------------------


// --------------------------------------------------------------------------
// --------------Ajout d'objets dans l'attraction de Ghyslain--------------
// --------------------------------------------------------------------------
		if (objetInRangeList.Count>0) {
			timer += Time.deltaTime;
			minEnergy = 100000;
			for (int i = objetInRangeList.Count-1; i >= 0; i--) {
				objetInRangeList[i].relDist = (transform.position - objetInRangeList[i].go.transform.position).magnitude;

				if (objetInRangeList[i].relDist *objetInRangeList[i].mass <  minEnergy){
					minEnergy = objetInRangeList[i].relDist *objetInRangeList[i].mass;
					minEnergyId = i;
				}
			}

			timerOnAddObjet = objetInRangeList[minEnergyId].relDist/5-5;
			if (timer > timerOnAddObjet )	{
				timer = 0;
				PullObjet(objetInRangeList[minEnergyId]);
				objetInRangeList.RemoveAt(minEnergyId);
			}
		}

// --------------------------------------------------------------------------
// ----------------------------Forces sur les objets-------------------------
// --------------------------------------------------------------------------
		// Forces agissant sur les objets qui tournent autour de Ghyslain
		for (int i = objetList.Count-1; i >= 0; i--) {

			//Calcul de la distance relative de l'objet avec Ghyslain
			relDist = transform.position - objetList [i].go.transform.position;

			if (relDist.magnitude > criticalDist ){
				Destroy( objetList[i].go );
				objetList.Remove( objetList[i] );
			Debug.Log ("Objet destroyed cause it was out of range");
			}
			// Forces
			AddForceXZ ( relDist, objetList[i] );
			AddForceY ( relDist, objetList[i], objetList.Count);
			AddDrag( objetList[i] );
		}

		// Constantly dim the lights
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("EvilLight");
		foreach (GameObject eLight in gos) {
			eLight.GetComponent<Light>().intensity = Mathf.Lerp ( eLight.GetComponent<Light>().intensity,0,Time.deltaTime*3);
			eLight.GetComponent<Light>().range = Mathf.Lerp ( eLight.GetComponent<Light>().range,0,Time.deltaTime*3);	
			
		}
		// Un éclair!
		if (Time.time > tempsEclair)	{
			CoupDeTonnerre();
			tempsEclair = tempsEclair+Random.Range (4,20);			
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
			eLight.GetComponent<Light>().intensity = 2;
			eLight.GetComponent<Light>().range = 500;	
			
		}

		for (int j = 0; j < Random.Range(1,4); j++) {
			GameObject unEclair = (GameObject)Instantiate (Eclair);
			LineRenderer elcairLR = unEclair.GetComponent<LineRenderer>();
			
			elcairLR.SetPosition( 0, transform.position);
			
			for (int i = 1; i < 10; i++) {
				elcairLR.SetPosition( i, new Vector3(transform.position.x + Random.Range (-5,5),transform.position.y + i*10-1,transform.position.z + Random.Range (-5,5)));
			}
			StartCoroutine(WaitAndDestroy(0.1f,unEclair));
		}
	}

	private GameObject getNextDestination (Vector3 pos)	{
		GameObject nextDestination = null;
		GameObject[] Checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
		float distance = 500f;
		if (destination !=null){
			
			destination.SetActive(false);
		}

		foreach (GameObject cp in Checkpoints) {

			if ((cp.transform.position -pos).magnitude < distance) {
				distance = (cp.transform.position -pos).magnitude;
				nextDestination = cp;

			}
		}
		return nextDestination;
	}

	private bool Move(Vector3 Destination, float speed){
		if ((Destination-transform.position).magnitude  >1){
			transform.position = transform.position +speed*(Destination-transform.position).normalized*Time.deltaTime;
			return reachedDestination =false;
		}
		else {
			return reachedDestination =true;
		}
	}
	// Calcul de la force radiale --------------------------------------------------------------
	private void AddForceXZ( Vector3 relDist, activeObjet objet){
		// variables
		Vector2 relDistXZ = new Vector2 (relDist.x, relDist.z);
		Vector2 forceXZ;

		// Force tangentielle si r < r_min
		if (relDistXZ.magnitude < objet.minDist) {
			forceXZ = - yVector.y * new Vector2 ( relDist.z , -relDist.x )/10;
			}
		// Force axiale minimum lorsque r > r_max
		else if (relDistXZ.magnitude > objet.maxDist) {
			forceXZ =objet.attractionForceXZ * relDistXZ / relDistXZ.magnitude / Mathf.Pow ( objet.maxDist, 2f );
			//forceXZ = (relDistXZ.magnitude-objet.maxDist)*objet.attractionForceXZ * relDistXZ / Mathf.Pow (relDistXZ.magnitude, 3f );
		}
		// Force axiale en 1/r
		else {
			forceXZ =objet.attractionForceXZ * relDistXZ / Mathf.Pow (relDistXZ.magnitude, 3f );
		}

		// Application de la force axiale
		objet.go.GetComponent<Rigidbody>().AddForce( new Vector3(forceXZ.x,0f,forceXZ.y) );

		// Force de Coriolis
		if( Vector3.Cross((objet.go.transform.position-transform.position),- Vector3.Cross (yVector , objet.go.GetComponent<Rigidbody>().velocity)).y  > 0) {
			objet.go.GetComponent<Rigidbody>().AddForce( - Vector3.Cross (yVector , objet.go.GetComponent<Rigidbody>().velocity)/2);
		}

	}

	// Calcul de la force en y-----------------------------------------------------------------
	private void AddForceY( Vector3 relDist, activeObjet objet, int objetNb){
		// variables
		/*
		float upForce = 0.1f;
		float downForce = -0.1f;

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


*/
		float forceY = 0;
		if (objet.id >= objetNb - objNbLowAlt){
			forceY = relDist.y/2;
		}
		else  {
			forceY = (objetNb-objNbLowAlt-objet.id)/2+relDist.y/2;
		}

		// Terme de drag
		//forceY += - 1f * objet.go.rigidbody.velocity.y;

		// Terme anti gravite
		forceY += 9.81f;

		objet.go.GetComponent<Rigidbody>().AddForce( new Vector3(0f, forceY, 0f) );
	}

	// Calcul du drag---------------------------------------------------------------------------
	private void AddDrag( activeObjet objet){
		// Variables
		Vector3 dragForce;
		// Il n'y a de drag qu'au dessus d'une vitesse seuil
		if (objet.go.GetComponent<Rigidbody>().velocity.magnitude < topSpeed){
			return;
		}
			// Drag en v^2
			dragForce = - cDrag * objet.go.GetComponent<Rigidbody>().velocity.magnitude / topSpeed *
				objet.go.GetComponent<Rigidbody>().velocity / topSpeed;

		objet.go.GetComponent<Rigidbody>().AddForce( dragForce );
	}


	// Add a new objet to Gyslain gravitational pull
	public void PullObjet(passiveObjet unObjet)  {
		unObjet.go.transform.GetComponent<Rigidbody>().isKinematic = false;
		if (unObjet.go.GetComponent<Rigidbody>().mass >=5) {
			objetList.Add ( new activeObjet(unObjet.go, Random.Range(20f,35f), Random.Range(40f,100f), Random.Range(2000f,10000f), objetList.Count));
		}
		else if (unObjet.go.GetComponent<Rigidbody>().mass >=2) {
			objetList.Add ( new activeObjet(unObjet.go, Random.Range(7f,20f), Random.Range(20f,60f), Random.Range(1000f,5000f), objetList.Count));
		}
		else if (unObjet.go.GetComponent<Rigidbody>().mass >=1){
			objetList.Add ( new activeObjet(unObjet.go, Random.Range(3f,10f), Random.Range(15f,35f), Random.Range(500f,2000f), objetList.Count));
		}
		else{
			objetList.Add ( new activeObjet(unObjet.go, Random.Range(3f,10f), Random.Range(15f,35f), Random.Range(500f,2000f), objetList.Count));		
		}

		// Set la masse de tout les objets a 1.
		unObjet.go.GetComponent<Rigidbody>().mass = 1;
	}

	// Add a new objet. A soft pull will attrack some objets in range
	public void ObjetInRange(GameObject unObjet)  {
		objetInRangeList.Add (new passiveObjet(unObjet,objetInRangeList.Count,(transform.position-unObjet.transform.position).magnitude, unObjet.GetComponent<Rigidbody>().mass));
	}
	
}

