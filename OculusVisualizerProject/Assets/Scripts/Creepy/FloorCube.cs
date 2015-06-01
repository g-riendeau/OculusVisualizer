using UnityEngine;
using System.Collections;

public class FloorCube : MonoBehaviour {
		public GameObject go;
		public int id;
		public float relDist;
		
	public FloorCube(GameObject newgo, int newid, float newRelDist)
	{			
			go = newgo;
			id = newid;
			relDist = newRelDist;
	}
		
}

