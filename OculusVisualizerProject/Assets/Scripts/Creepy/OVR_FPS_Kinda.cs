using UnityEngine;
using System.Collections;

public class OVR_FPS_Kinda : MonoBehaviour {


	public Transform cameraObject = null;
	private float Xpos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Xpos = transform.position.x + cameraObject.rotation.w;
		//transform.position = new Vector3 (Xpos,transform.position.y,transform.position.z);

	}
}
