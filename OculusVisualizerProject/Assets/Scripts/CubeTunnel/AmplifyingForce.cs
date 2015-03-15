using UnityEngine;
using System.Collections;

public class AmplifyingForce : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position +=new Vector3( Time.deltaTime,0,0);
	
	}
}
