using UnityEngine;
using System.Collections;

public class Ghyslain : MonoBehaviour {

	public Material sphereMat;
	public GameObject evilLight;

	private int sphereNb;
	private GameObject[] spheres;
	private Vector3 relDist;
	private float attractionForce = 5000f;
	private float orbiteMax = 100;
	private float timer;

	private LineRenderer[] eclairs;

	// Use this for initialization
	void Start () {
		attractionForce = 5000f;
		sphereNb = 13;
		spheres = new GameObject[sphereNb];

		eclairs = new LineRenderer[sphereNb];

		for (int i=0; i<sphereNb; i++) {

			timer = 0;
			spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//sphere.transform.parent = this.transform;
			spheres[i].renderer.material = sphereMat;
			spheres[i].transform.localPosition = new Vector3(transform.position.x+Random.Range(10,20), transform.position.y-6+2*i, transform.position.z);
			spheres[i].transform.localRotation = Quaternion.identity;
			spheres[i].transform.localScale = new Vector3(1f, 1f, 1f);
			spheres[i].AddComponent<Rigidbody>();

			if (Random.Range(-1,1) < 0){
				spheres[i].rigidbody.velocity = new Vector3(0,0,200/spheres[i].transform.position.x);
			}
			else		{
				spheres[i].rigidbody.velocity = new Vector3(0,0,-200/spheres[i].transform.position.x);
			}
			spheres[i].rigidbody.angularDrag = 0;
			spheres[i].rigidbody.mass = 1;

			GameObject unEclair = new GameObject();
			unEclair.AddComponent<LineRenderer>();
			eclairs[i] = unEclair.GetComponent<LineRenderer>();
			eclairs[i].SetPosition( 0, transform.position );
			eclairs[i].SetPosition( 1, spheres[i].transform.position );
			eclairs[i].SetWidth( 0.1f, 0.1f);

		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		timer += Time.deltaTime;

		for (int i = 0; i<sphereNb; i++) {
			relDist = transform.position - spheres [i].transform.position;
			//if( relDist 
			spheres [i].rigidbody.AddForce (attractionForce / Mathf.Pow (relDist.magnitude, 3f) * relDist);
			spheres [i].rigidbody.AddForce (0, 10, 0);
			eclairs[i].SetPosition (0, transform.position);
			eclairs[i].SetPosition (1, spheres[i].transform.position);
		}

		if (timer > 1f)	{	
			evilLight.light.intensity = Mathf.Lerp (evilLight.light.intensity,1,Time.deltaTime*5);
			evilLight.light.range =  Mathf.Lerp (evilLight.light.range,20,Time.deltaTime*5);
		}

		if (timer > 5)	{	
			timer = 0;
			evilLight.light.intensity = Mathf.Lerp (evilLight.light.intensity,8,Time.deltaTime*5);
			evilLight.light.range = Mathf.Lerp (evilLight.light.range,200,Time.deltaTime*5);
		}

	
	}
}
