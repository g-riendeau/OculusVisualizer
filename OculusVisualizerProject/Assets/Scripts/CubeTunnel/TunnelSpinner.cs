using UnityEngine;
using System.Collections;

public class TunnelSpinner : MonoBehaviour {

	public CubeTunnel tunnel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.realtimeSinceStartup > 224){

			//Spin le tunnel
			transform.Rotate(new Vector3(0f, 0f, 1f), 8.1f*Time.deltaTime, Space.World);

			//Spin les cubes!?
			for (int i = 0 ; i<tunnel.cubeConeArray.GetLength(0); i++)
			{
				for (int j = 0 ; j<tunnel.cubeConeArray.GetLength(1); j++)
				{
					tunnel.cubeConeArray[i,j].transform.Rotate(new Vector3(0f, 0f, 1f), 20f*Time.deltaTime, Space.World);
				}
			}

			for (int i = 0 ; i<tunnel.cubeCylinderArray.GetLength(0); i++)
			{
				for (int j = 0 ; j<tunnel.cubeCylinderArray.GetLength(1); j++)
				{
					tunnel.cubeCylinderArray[i,j].transform.Rotate(new Vector3(0f, 0f, 1f), 20f*Time.deltaTime, Space.World);
				}
			}

			for (int i = 0 ; i<tunnel.cubeCenterArray.GetLength(0); i++)
			{
				for (int j = 0 ; j<tunnel.cubeCenterArray.GetLength(1); j++)
				{
					tunnel.cubeCenterArray[i,j].transform.Rotate(new Vector3(0f, 0f, 1f), 20f*Time.deltaTime, Space.World);
				}
			}


		}
	}
}
