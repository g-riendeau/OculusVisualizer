using UnityEngine;
using System.Collections;

public class gSphere 
{

	public GameObject go;
	public float minDist;
	public float maxDist;
	public float attractionForceXZ;
	
	public gSphere(GameObject newgo, float newminDist, float newmaxDist, float newAttractionForceXZ)
	{			
		go = newgo;
		attractionForceXZ = newAttractionForceXZ;
		minDist = newminDist;
		maxDist = newmaxDist;

	}

}
