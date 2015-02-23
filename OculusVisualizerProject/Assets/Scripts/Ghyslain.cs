using UnityEngine;
using System.Collections;

public class Ghyslain : MonoBehaviour {

	public Material sphereMat;
	public GameObject EvilLight;

	private int sphereNb;
	private GameObject[] spheres;
	private Vector3 RelD;
	private float AttractionForce;
	private float timer;

	// Use this for initialization
	void Start () {
		AttractionForce = 200;
		sphereNb = 13;
		spheres = new GameObject[sphereNb];

		for (int i=0; i<sphereNb; i++) {

			timer =0;
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
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		timer += Time.deltaTime;

		foreach (GameObject sphere in spheres) {		
			RelD =  transform.position - sphere.transform.position;
			sphere.rigidbody.AddForce(AttractionForce/(RelD.magnitude*RelD.magnitude)*RelD);
			sphere.rigidbody.AddForce(0,11,0);
		}
		if (timer > 1f)	{	
			EvilLight.light.intensity = Mathf.Lerp (EvilLight.light.intensity,1,Time.deltaTime*5);
			EvilLight.light.range =  Mathf.Lerp (EvilLight.light.range,20,Time.deltaTime*5);
		}

		if (timer > 5)	{	
			timer = 0;
			EvilLight.light.intensity = Mathf.Lerp (EvilLight.light.intensity,8,Time.deltaTime*5);
			EvilLight.light.range = Mathf.Lerp (EvilLight.light.range,200,Time.deltaTime*5);
		}

	
	}
}
