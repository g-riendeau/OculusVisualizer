using UnityEngine;
using System.Collections;

public class EffectField : MonoBehaviour {

	private World World;

	void OnTriggerEnter(Collider Enterer) {
		if (Enterer.tag == "PassiveCube") {

			//GetComponent<World>().ActivateFloorCube(Enterer.gameObject);
			Enterer.tag = "ActiveCube";
		}
	}
	void OnTriggerExit(Collider Exiter) {
		if (Enterer.tag == "ActiveCube") {
			
			//GetComponent<World>().ActivateFloorCube(Enterer.gameObject);
			Enterer.tag = "PassiveCube";
		}
	}
}
