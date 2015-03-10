using UnityEngine;
using System.Collections;

public class POVSpinner : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time > 10){

			//Spin le tunnel
			transform.Rotate(new Vector3(1f, 0f, 0f), 8.1f*Time.deltaTime, Space.World);

		}
	}
}
