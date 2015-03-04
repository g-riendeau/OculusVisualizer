using UnityEngine;
using System.Collections;

public class SphereAttractionStrong : MonoBehaviour {

	
	void OnTriggerEnter(Collider Enterer) {
		if (Enterer.tag == "PassiveSphere") {			
	
			GetComponentInParent<Ghyslain> ().PullSphere(Enterer.gameObject);
			Enterer.tag = "ActiveSphere";		
			
		}
		
	}




}
