using UnityEngine;
using System.Collections;

public class SphereAttraction : MonoBehaviour {
	


	void OnTriggerEnter(Collider Enterer) {
		if (Enterer.tag == "PassiveSphere") {
				//GetComponentInParent<Ghyslain> ().PullSphere(Enterer.gameObject);
				GetComponentInParent<Ghyslain> ().SphereInRange(Enterer.gameObject);
				Enterer.tag = "ActiveSphere";
		}
	}
	/*
	void OnTriggeExit(Collider Exiter) {
		if (Exiter.tag == "ActiveSphere") {			
			
			GetComponentInParent<Ghyslain> ().SphereOutOfRange(Exiter.gameObject);
			Exiter.tag = "PassiveSphere";		
			
		}
		
	}
	*/

}
