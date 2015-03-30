using UnityEngine;
using System.Collections;

public class TunnelMover : MonoBehaviour {

	public CubeTunnel tunnel;
	public Song song;

	private bool flexion = false;
	private bool[] flexionDone;
	private bool[] firstFlexFrame;
	private float[] ampFlexion;
	private float[] freqFactorY;

	private float zSpinSpeed1;
	private float zSpinSpeed2;
	private float rock1sec;

	// Use this for initialization
	void Start () {

		// ------------------------F L E X I O N   T U N N E L ------------------------------
		flexionDone = new bool[song.flexionTime.Length];
		firstFlexFrame= new bool[song.flexionTime.Length];
		ampFlexion = new float[song.flexionTime.Length];
		freqFactorY = new float[song.flexionTime.Length];
		
		for (int i = 0; i<song.flexionTime.Length; i++){
			flexionDone[i] = false;
			firstFlexFrame[i] = true;
			freqFactorY[i] = UnityEngine.Random.Range(0.2f, 2f);
		}

		// ------------------------S P I N   T U N N E L ------------------------------
		zSpinSpeed1 = 360f / song.zSpinLength1;
		zSpinSpeed2 = 360f / song.zSpinLength2;
		rock1sec = song.zSpinTime1 + song.zSpinLength1;

	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log (song.time ());
		// ------------------------F L E X I O N   T U N N E L ------------------------------
		// Make the tunnel bend after the specified time
//		for (int i=0; i<song.flexionTime.Length;i++){
//			if (song.time() >song.flexionTime[i] && !flexionDone[i]) {
//				
//				if (song.flexionLength[i] < 4f) {
//					Debug.Log("A song.flexion_length is too short. It is ignored");
//				}
//				else {
//					flexionDone[i] = flexTunnel(tunnel.cubeCone1Array,tunnel.cubeCone2Array,
//					                            Mathf.Sin (8f*(song.time()-song.flexionTime[i])/song.flexionLength[i])/100f,	
//					                            Mathf.Sin (2f*(song.time()-song.flexionTime[i])/song.flexionLength[i])/100f, i);
//				}
//			}
//		}
		if (estDans (song.flexionTime [0], song.flexionLength [0])) {
			flexionDone [0] = flexTunnel (tunnel.cubeCone1Array, tunnel.cubeCone2Array,
			                            Mathf.Sin (4f * Mathf.PI * (song.time () - song.flexionTime [0]) / song.flexionLength [0]) / 100f,	
			                            Mathf.Sin (2f * Mathf.PI * (song.time () - song.flexionTime [0]) / song.flexionLength [0]) / 100f, 0);
		} else if (estDans (song.zSpinTime1, song.zSpinLength1)) {
			doubleSpin (zSpinSpeed1, 0f);
		}
		else if (estDans (rock1sec, 1f)) {
			doubleSpin ( 0f , 45f * Mathf.Sin (1f*Mathf.PI*(song.time () - rock1sec)) );
		}
		else if ( estDans (song.zSpinTime2 , song.zSpinLength2) ){
			doubleSpin (-2f*song.zSpinLength2/Mathf.PI*Mathf.Sin(2f*Mathf.PI*(song.time ()-song.zSpinTime2)/song.zSpinLength2), 0f );
		}
	}


	private bool estDans( float startTime, float length ){
		return (song.time () >= startTime) && (song.time () < startTime + length);
	}
	

	// ROTATION DU TUNNEL AUTOUR DE L'AXE Z + SPIN DES CUBES SUR EUX-MEMES
	// rotationSpeed en degres/seconde
	// spinAngle en degres
	private void doubleSpin( float rotationSpeedZ, float spinAngle ){

		float Theta_i;
		float dTheta = rotationSpeedZ * Time.deltaTime * Mathf.PI / 180f;;
		Quaternion Quat_i;
		Vector3 Pos_ij;
		float[,] RotMat = new float[2,2];
		RotMat [0, 0] = Mathf.Cos (dTheta); RotMat [0, 1] = - Mathf.Sin (dTheta);
		RotMat [1, 0] = Mathf.Sin (dTheta); RotMat [1, 1] = Mathf.Cos (dTheta);

		for (int i = 0 ; i<tunnel.cubeCone1Array.GetLength(0); i++)
		{
			Theta_i = Mathf.Atan2(tunnel.cubeCone1Array[i,0].transform.localPosition.y , tunnel.cubeCone1Array[i,0].transform.localPosition.x);
			Theta_i += dTheta;
			Quat_i = Quaternion.Euler(-CubeTunnel.dAlpha*Mathf.Sin (Theta_i), CubeTunnel.dAlpha*Mathf.Cos (Theta_i), 180f*Theta_i/Mathf.PI + 90f + spinAngle);

			for (int j = 0 ; j<tunnel.cubeCone1Array.GetLength(1); j++)
			{
				tunnel.cubeCone1Array[i,j].transform.localRotation = Quat_i;
				Pos_ij = tunnel.cubeCone1Array[i,j].transform.localPosition;
				tunnel.cubeCone1Array[i,j].transform.localPosition = new Vector3(
					Pos_ij.x*RotMat[0,0] + Pos_ij.y*RotMat[0,1], Pos_ij.x*RotMat[1,0] + Pos_ij.y*RotMat[1,1], Pos_ij.z );
			}
		}
		
		for (int i = 0 ; i<tunnel.cubeCone2Array.GetLength(0); i++)
		{
			Theta_i = Mathf.Atan2(tunnel.cubeCone2Array[i,0].transform.localPosition.y , tunnel.cubeCone2Array[i,0].transform.localPosition.x);
			Quat_i = Quaternion.Euler(CubeTunnel.dAlpha*Mathf.Sin (Theta_i), -CubeTunnel.dAlpha*Mathf.Cos (Theta_i), 180f*Theta_i/Mathf.PI + 90f + spinAngle);
			for (int j = 0 ; j<tunnel.cubeCone2Array.GetLength(1); j++)
			{
				tunnel.cubeCone2Array[i,j].transform.localRotation = Quat_i;
				Pos_ij = tunnel.cubeCone2Array[i,j].transform.localPosition;
				tunnel.cubeCone2Array[i,j].transform.localPosition = new Vector3(
					Pos_ij.x*RotMat[0,0] + Pos_ij.y*RotMat[0,1], Pos_ij.x*RotMat[1,0] + Pos_ij.y*RotMat[1,1], Pos_ij.z );
			}
		}
		
		for (int i = 0 ; i<tunnel.cubeCenterArray.GetLength(0); i++)
		{
			Theta_i = Mathf.Atan2(tunnel.cubeCone2Array[i,0].transform.localPosition.y , tunnel.cubeCone2Array[i,0].transform.localPosition.x);
			Quat_i = Quaternion.Euler(0f, 0f, 180f*Theta_i/Mathf.PI + 90f + spinAngle);
			for (int j = 0 ; j<tunnel.cubeCenterArray.GetLength(1); j++)
			{
				tunnel.cubeCenterArray[i,j].transform.localRotation = Quat_i;
				Pos_ij = tunnel.cubeCenterArray[i,j].transform.localPosition;
				tunnel.cubeCenterArray[i,j].transform.localPosition = new Vector3(
					Pos_ij.x*RotMat[0,0] + Pos_ij.y*RotMat[0,1], Pos_ij.x*RotMat[1,0] + Pos_ij.y*RotMat[1,1], Pos_ij.z );
			}
		}
	
	}



	private bool flexTunnel(CubeInfo[,] cubes1,CubeInfo[,] cubes2, float sinOffsetX, float sinOffsetY, int flexionID)  {	
		
		bool flexionDone = false;
		float amp = 1000f;
		
		// Si c'est la premiere frame de cette flexion, avertir TunnelSpinner pour qu'il cesse d'enregistrer la position
		if (firstFlexFrame[flexionID]==true) {
			firstFlexFrame[flexionID] = false;
			startFlexion();
		}
		
		if ((song.time() - song.flexionTime[flexionID])<Mathf.Min (2,song.flexionLength[flexionID]/3)) {
			// La force d'amplication du sinus commence a 0 et augmente jusqu'a 4 
			amp = (song.time()-song.flexionTime[flexionID])/2;
			
		}
		else if ((song.time() - song.flexionTime[flexionID])<(song.flexionLength[flexionID]-4))  {
			
			amp = 1f;
		}
		else {
			// La force d'amplication du sinus diminue jusqu'a 0 (parabole inversée)
			amp = Mathf.Max ((song.flexionLength[flexionID]-(song.time()-song.flexionTime[flexionID]))/4,0);
		}
		
		for(int i = 0; i<cubes1.GetLength(0); i++){
			for(int j = 1; j<cubes1.GetLength(1); j++){
				cubes1[i,j].transform.position = new Vector3 ((cubes1[i,j].posSansFlexion.x+ amp*Mathf.Pow(j,2)*sinOffsetX),cubes1[i,j].posSansFlexion.y + amp*Mathf.Pow(j,2)*sinOffsetY,cubes1[i,j].transform.position.z);
			}
		}
		for(int i = 0; i<cubes2.GetLength(0); i++){
			for(int j = 1; j<cubes2.GetLength(1); j++){
				cubes2[i,j].transform.position = new Vector3 ((cubes2[i,j].posSansFlexion.x+ amp*Mathf.Pow(j,2)*sinOffsetX),cubes2[i,j].posSansFlexion.y+ amp*Mathf.Pow(j,2)*sinOffsetY,cubes2[i,j].transform.position.z);
			}
		}
		// Stop the flexion script when amp and the offset are small
		if (amp < 0.0001f){	
			flexionDone = true;
			ApplyDefaultPosition(cubes1);
			ApplyDefaultPosition(cubes2);
		}
		return flexionDone;
	}

	void ApplyDefaultPosition(CubeInfo[,] cubes)  {	
		endFlexion();
		for(int i = 0; i<cubes.GetLength(0); i++){
			for(int j = 1; j<cubes.GetLength(1); j++){
				cubes[i,j].transform.position = cubes[i,j].posSansFlexion;
				
			}
		}
	}

	private void startFlexion(){
		flexion = true;
		for (int i = 0 ; i<tunnel.cubeCone1Array.GetLength(0); i++)
		{
			for (int j = 0 ; j<tunnel.cubeCone1Array.GetLength(1); j++)
			{
				tunnel.cubeCone1Array[i,j].posSansFlexion = tunnel.cubeCone1Array[i,j].transform.position;
			}
		}
		for (int i = 0 ; i<tunnel.cubeCone2Array.GetLength(0); i++)
		{
			for (int j = 0 ; j<tunnel.cubeCone2Array.GetLength(1); j++)
			{
				tunnel.cubeCone2Array[i,j].posSansFlexion = tunnel.cubeCone2Array[i,j].transform.position;
			}
		}
	}
	
	private void endFlexion(){
		
		flexion = false;
		for (int i = 0 ; i<tunnel.cubeCone1Array.GetLength(0); i++)
		{
			for (int j = 0 ; j<tunnel.cubeCone1Array.GetLength(1); j++)
			{
				tunnel.cubeCone1Array[i,j].posSansFlexion = tunnel.cubeCone1Array[i,j].transform.position;
			}
		}
		for (int i = 0 ; i<tunnel.cubeCone2Array.GetLength(0); i++)
		{
			for (int j = 0 ; j<tunnel.cubeCone2Array.GetLength(1); j++)
			{
				tunnel.cubeCone2Array[i,j].posSansFlexion = tunnel.cubeCone2Array[i,j].transform.position;
			}
		}
	}

}
