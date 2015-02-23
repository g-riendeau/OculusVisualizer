using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.realtimeSinceStartup > 224f){
			transform.Rotate(new Vector3(0f, 0f, 1f), 8*Time.deltaTime, Space.World);
		}
	}
}
