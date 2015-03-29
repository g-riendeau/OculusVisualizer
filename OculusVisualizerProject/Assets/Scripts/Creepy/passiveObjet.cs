using UnityEngine;
using System.Collections;

public class passiveObjet
{

	public GameObject go;
	public int id;
	public float relDist;
	public float mass;
	
	public passiveObjet(GameObject newgo, int newid, float newRelDist, float newMass)
	{			
		go = newgo;
		id = newid;
		relDist = newRelDist;
		mass = newMass;
	}

}
