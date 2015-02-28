using UnityEngine;
using System.Collections;

public class Lights : MonoBehaviour {

	public GameObject DirLight1;
	public GameObject PointLight1;

	private GameObject[] PointLights;
	private Vector3[] TargetPLPos;
	private int i;

	// Use this for initialization
	void Start () {
		GameObject[] PointLights = GameObject.FindGameObjectsWithTag("PointLight");
		TargetPLPos = new Vector3[PointLights.Length];

		for( int i = 0 ; i < TargetPLPos.Length ;i++ )
		{			
			TargetPLPos[i] = new Vector3(0,0,0);

		}
	}
	
	// Update is called once per frame
	void Update () {
		//Bouge cette folle lumiere directionnel
		DirLight1.transform.Rotate 	(0, Time.deltaTime*100f, 0);

		i = 0;
		//Déplace cette grosse lumiere ponctuel
		GameObject[] PointLights = GameObject.FindGameObjectsWithTag("PointLight");
		foreach( GameObject light in PointLights )
		{
			light.transform.position = Vector3.Lerp(light.transform.position, TargetPLPos[i],Time.deltaTime*1.5f);

			// Check if light reached its destination. If so, set new target position
			if (Vector3.Distance(TargetPLPos[i],light.transform.position) <=0.5f) {
				TargetPLPos[i] = new Vector3(Random.Range (-3f,3f),0,Random.Range (-9.0f,18f));
			}
			i++;
		}

	}
}