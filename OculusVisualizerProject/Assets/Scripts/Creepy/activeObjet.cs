﻿using UnityEngine;
using System.Collections;

public class activeObjet
{

	public GameObject go;
	public float minDist;
	public float maxDist;
	public float attractionForceXZ;
	public int id;
	
	public activeObjet(GameObject newgo, float newminDist, float newmaxDist, float newAttractionForceXZ, int newid)
	{			
		go = newgo;
		attractionForceXZ = newAttractionForceXZ;
		minDist = newminDist;
		maxDist = newmaxDist;
		id = newid;

	}

}
