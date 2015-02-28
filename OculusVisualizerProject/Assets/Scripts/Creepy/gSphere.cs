using UnityEngine;
using System.Collections;

public class gSphere 
{

	public GameObject go;
	public float minDist;
	public float maxDist;	
	
	public gSphere(GameObject newgo, float newminDist,float newmaxDist)
	{			
		go = newgo;
		minDist = newminDist;
		maxDist = newmaxDist;

	}

}
