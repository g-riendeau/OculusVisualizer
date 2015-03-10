using UnityEngine;
using System.Collections;

public class TunnelSpinner : MonoBehaviour {

	public CubeTunnel tunnel;
	private float zSpinSpeed;
	private float finalSpinSpeed;
	private Quaternion iniCamRotation;
	private float timeSpentOut;
	private float timeStopped;
	private float timePerExit;
	private int nbTimeOut;
	private int TotNbTimeOut;
	public Song song;

	// Use this for initialization
	void Start () {
		zSpinSpeed = 360f/(song.fin2eTiers - song.debut2eTiers);
		nbTimeOut = 1;
		TotNbTimeOut = 4;
		timePerExit = (song.finLastStretch - song.debutLastStretch) / TotNbTimeOut;
		timeStopped = 10;
		timeSpentOut = timePerExit-timeStopped;
		finalSpinSpeed = 360f/(song.finLastStretch - song.debutLastStretch-timeSpentOut*TotNbTimeOut/2)*TotNbTimeOut;
		//finalSpinSpeed = 360f/(20f - 5f)*3;
		iniCamRotation = transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {

		//Spin le tunnel
		if((Time.time > song.debut2eTiers && Time.time < song.fin2eTiers) || Time.time > song.finLastStretch ){
			transform.Rotate(new Vector3(0f, 0f, 1f), zSpinSpeed*Time.deltaTime, Space.World);
		}

		// Rotate pour faire sortir du tunnel
		if((Time.time > song.debutLastStretch && Time.time < song.finLastStretch )){

			if((Time.time > song.debutLastStretch+ timeSpentOut+(nbTimeOut-1)*(timePerExit))){
				// Slowly get to the initial position and stay there a little while
				transform.rotation = Quaternion.Lerp(transform.rotation, iniCamRotation, Time.deltaTime);
				if (Time.time > song.debutLastStretch+ nbTimeOut*timePerExit){
					nbTimeOut +=1;					
				}
			}
			else {
				transform.Rotate(new Vector3(1f, 0f, 1f), finalSpinSpeed*Time.deltaTime, Space.World);

			}
		}

		if(Time.time > song.bassDrop){

			//Spin les cubes!?
			for (int i = 0 ; i<tunnel.cubeCone1Array.GetLength(0); i++)
			{
				for (int j = 0 ; j<tunnel.cubeCone1Array.GetLength(1); j++)
				{
					tunnel.cubeCone1Array[i,j].transform.Rotate(new Vector3(0f, 0f, 1f), 20f*Time.deltaTime, Space.World);
				}
			}

			for (int i = 0 ; i<tunnel.cubeCone2Array.GetLength(0); i++)
			{
				for (int j = 0 ; j<tunnel.cubeCone2Array.GetLength(1); j++)
				{
					tunnel.cubeCone2Array[i,j].transform.Rotate(new Vector3(0f, 0f, 1f), 20f*Time.deltaTime, Space.World);
				}
			}
			/*
			for (int i = 0 ; i<tunnel.cubeCylinderArray.GetLength(0); i++)
			{
				for (int j = 0 ; j<tunnel.cubeCylinderArray.GetLength(1); j++)
				{
					tunnel.cubeCylinderArray[i,j].transform.Rotate(new Vector3(0f, 0f, 1f), 20f*Time.deltaTime, Space.World);
				}
			}
			*/
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
