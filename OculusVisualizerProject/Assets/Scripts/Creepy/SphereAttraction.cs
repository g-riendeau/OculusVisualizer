using UnityEngine;
using System.Collections;

public class SphereAttraction : MonoBehaviour {

	private GameObject Enterer;
	
	void OnTriggerEnter(Collider Enterer) {

		if (Enterer.tag == "PassiveSphere") {
			Debug.Log (Enterer);

			//GetComponent<Ghyslain> ().AddThisSphere(gameObject);
		}


	}
}
