using UnityEngine;
using System.Collections;

public class ForestEntry : MonoBehaviour {

	public Transform playerPosition;
	public Camera mainCamera;
	private bool inForest = false;
	private Vector3 entryPosition;

	void OnTriggerEnter(Collider Enterer) {
		if (Enterer.name == "Wallas" & inForest == false) {

			entryPosition = Enterer.transform.position;
			RenderSettings.fog = true;
			RenderSettings.fogMode = FogMode.Exponential;
			RenderSettings.fogDensity = 0;
			inForest = true;
			mainCamera.backgroundColor = Color.black;
		}
		else if (Enterer.name == "Wallas" & inForest == true) {
			
			entryPosition = Enterer.transform.position;
			RenderSettings.fog = false;
			RenderSettings.fogMode = FogMode.ExponentialSquared;
			RenderSettings.fogDensity = 0;
			inForest = false;
		}
	}

	void Update() 
	{
		if (inForest == true) {


			float distance = (playerPosition.position - entryPosition).magnitude;
			Vector4 fogColor =  new Vector4(Mathf.Min (distance/100,0.1f), Mathf.Min (distance/100,0.1f),Mathf.Min (distance/100,0.1f), 1);
			RenderSettings.fogDensity = Mathf.Min(distance/100,0.05f);
			RenderSettings.fogColor = fogColor;
			mainCamera.backgroundColor = fogColor;
		}	
	}
}
