using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

	public CubeCylinder cylinder;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.realtimeSinceStartup > 224){

			//Spin le tunnel
			transform.Rotate(new Vector3(0f, 0f, 1f), 8.1f*Time.deltaTime, Space.World);

			//Spin les cubes!?
			for (int i = 0 ; i<cylinder.cubeArray.GetLength(0); i++)
			{
				for (int j = 0 ; j<cylinder.cubeArray.GetLength(1); j++)
				{
					cylinder.cubeArray[i,j].transform.Rotate(new Vector3(0f, 0f, 1f), 20f*Time.deltaTime, Space.World);
				}
			}


		}
	}
}
