using UnityEngine;
using System.Collections;

public class FirstPerson : MonoBehaviour {

	public Camera FirstPersonCamera;

	private float MouseSensitivity;
	private float MouvSpeed;
	private float PitchRange;
	private float rotPitch;

	private float VertVelocity =0f;

	private CharacterController characterController;

	// Use this for initialization
	void Start () {

		FirstPersonCamera.enabled = true;

		MouvSpeed = 4;
		MouseSensitivity = 3;
		PitchRange = 60f;

		// Hides the mouse
		//Screen.lockCursor = false;

		characterController = GetComponent<CharacterController>();
		if (characterController == null) {
			print ("No Character Controller");
		}
	}
	
	// Update is called once per frame
	void Update () {		

		//Rotation
		float rotYaw = Input.GetAxis ("Mouse X")*MouseSensitivity;
		transform.Rotate (0, rotYaw, 0);

		rotPitch -= Input.GetAxis ("Mouse Y")*MouseSensitivity;
		//rotPitch = Mathf.Clamp (rotPitch, -PitchRange, PitchRange);
		FirstPersonCamera.transform.localRotation = Quaternion.Euler (rotPitch,0, 0);

		float FwdSpeed = Input.GetAxis ("Vertical") * MouvSpeed;
		float SideSpeed = Input.GetAxis ("Horizontal") * MouvSpeed;

		Vector3 speed = new Vector3(SideSpeed,VertVelocity,FwdSpeed);

		speed = transform.rotation * speed;


		if (characterController.isGrounded) {
			VertVelocity = 0;
		}
		else {
			
			VertVelocity += Physics.gravity.y * Time.deltaTime;
		}

		characterController.Move (speed * Time.deltaTime);

	}
}
