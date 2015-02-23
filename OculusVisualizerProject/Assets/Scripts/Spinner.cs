using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.realtimeSinceStartup > 10f){
			transform.Rotate(transform.forward, 5*Time.deltaTime);
		}
	}
}
