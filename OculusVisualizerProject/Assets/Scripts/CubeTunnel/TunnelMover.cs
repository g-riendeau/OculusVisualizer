using UnityEngine;
using System.Collections;

public class TunnelMover : MonoBehaviour {

	public CubeTunnel tunnel;
	public Song song;
	public GameObject camera;

	private float t;


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

		// ------------------------S P I N   T U N N E L ------------------------------------
		zSpinSpeed1 = 2160f / (song.zSpinLength1*song.zSpinLength1);
		zSpinSpeed2 = 720f*Mathf.PI / song.zSpinLength2;
		rockTime1 = song.zSpinTime1 + song.zSpinLength1;
		omega2eTiers1 = 10f * Mathf.PI/ song.length2eTiers;
		omega2eTiers2 = 11f * Mathf.PI /song.length2eTiers ;
		zSpinSpeed3 = 8f * 360f / song.length3eTiers;
		omega3eTiers = 8f * Mathf.PI / song.length3eTiers;
		rockTime2 = 215.5f;
	}
	

	// Update is called once per frame
	void FixedUpdate () {

		//Debug.Log (song.time ());
		//Debug.Log ("x:" + transform.position.x + ", y:" + transform.position.y+ ", z:" + transform.position.z);


		// 1er Tier
		if (song.time () < song.debut2eTiers) {
			GoToPosition (new Vector3 (0f, 0f, 5f), 70f, 10f);
			GoToAngle (180f, 360f, true, 50f, 39f);
		}

		// 2e Tier
		if ( estDans( song.debut2eTiers, song.debut3eTiers-song.debut2eTiers) ){
			GoToPosition (new Vector3 (0f, 0f, -10f), 140f, 12.5f);
			GoToPosition (new Vector3 (0f, 0f, 10f), 153f, 12.5f);
			GoToPosition (new Vector3 (0f, 0f, 5f), 166f, 12.5f);

		// Fin 2e Tier
			GoToPosition (new Vector3 (0f, 0f,  0f), 204f, 5f);
			GoToAngle (1440f, 0f, true, 204f, 10f);
			GoToPosition (new Vector3 (0f, 0f, -5f), rockTime2, 2f);
		}

		// 3e Tier
		if (estDans (song.debut3eTiers-1.5f*Time.fixedDeltaTime, song.descenteAuxEnfers - song.debut3eTiers)) {
			GoToPosition (new Vector3 (0f, -4f, -5f), song.debut3eTiers, 8f-Time.fixedDeltaTime);
			GoToPosition (new Vector3 (0f, 0f, -10f), song.debut3eTiers+8f+Time.fixedDeltaTime, 8f); 
			GoToAngle (720f, 360f, false, song.debut3eTiers, 47f);
			GoToPosition (new Vector3 (0f, 0f, -40.5f), song.descenteAuxEnfers-18f, 15f);
		}
		if ( song.time() >= song.descenteAuxEnfers-1.5f*Time.fixedDeltaTime){
			descenteAuxEnfers( 15f, 15f, song.descenteAuxEnfers, 32.5f );
		}


		// Spins et flexions
		if (estDans (song.flexionTime [0], song.flexionLength [0])) {
			flexionDone [0] = flexTunnel (tunnel.cubeCone1Array, tunnel.cubeCone2Array,
			                  Mathf.Sin (4f * Mathf.PI * (song.time () - song.flexionTime [0]) / song.flexionLength [0]) / 100f,	
			                  Mathf.Sin (2f * Mathf.PI * (song.time () - song.flexionTime [0]) / song.flexionLength [0]) / 100f, 0);
		} else if (estDans (song.zSpinTime1, song.zSpinLength1)) {
			t = song.time () - song.zSpinTime1;
			doubleSpin (zSpinSpeed1 * t * (1f - t / song.zSpinLength1), 0f);
		} else if (estDans (rockTime1, 1.2f)) {
			doubleSpin (0f, 45f * Mathf.Sin (1f * Mathf.PI * (song.time () - rockTime1) / 1.2f));
		} else if (estDans (song.zSpinTime2, song.zSpinLength2)) {
			doubleSpin (zSpinSpeed2 * Mathf.Sin (2f * Mathf.PI * (song.time () - song.zSpinTime2) / song.zSpinLength2), 0f);
		} else if (estDans (song.debut2eTiers, song.length2eTiers)) {
			t = song.time () - song.debut2eTiers;
			flexionDone [1] = flexTunnel (tunnel.cubeCone1Array, tunnel.cubeCone2Array,
			                              Mathf.Sin (omega2eTiers1 * Mathf.Sin (2f * Mathf.PI * t / song.length2eTiers) * t) / 100f,	
			                              Mathf.Cos (omega2eTiers1 * Mathf.Sin (2f * Mathf.PI * t / song.length2eTiers) * t) / 100f, 1);
			doubleSpin (0f, 50f * Mathf.Sin (omega2eTiers1 * t) );
		} else if (estDans (rockTime2, 2f)) {
			t = song.time () - rockTime2;
			doubleSpin (540f * t * (1f - t / 2f), 0f);
		}
		else if (estDans(song.debut3eTiers, song.length3eTiers)){
			doubleSpin( zSpinSpeed3, 50f*Mathf.Sin (omega3eTiers*(song.time()-song.debut3eTiers)) );
		}
	}

	// EST DANS L'INTERVALLE [startTime,startTime+length] ? -----------------------------------------------------------------------
	private bool estDans( float startTime, float length ){
		return (song.time () >= startTime) && (song.time () < startTime + length);
	}

	// BOUGE LA POSITION DU TUNNEL POUR DONNER L'IMPRESSION QUE LA CAMERA BOUGE ---------------------------------------------------
	// posTarget  : position ou l'on desire bouger la camera
	// startTime  : debut du deplacement
	// timeLength : duree du deplacement 
	private Vector3 pos0;
	private Vector3 dPos;
	private float posJerk;
	private float posAcc;

	private void GoToPosition( Vector3 posTarget, float startTime, float timeLength ){
		float t = song.time () - startTime;
		if ( (t > - 1.1*Time.fixedDeltaTime) && (t < 0f) ) {
			pos0 = transform.position;
			dPos = -posTarget - pos0;
			posJerk = -12f * dPos.magnitude / Mathf.Pow (timeLength, 3f);
			posAcc  = 6 * dPos.magnitude / Mathf.Pow (timeLength, 2f);
		}
		else if( (t >= timeLength-0.55f*Time.fixedDeltaTime) && (t <= 0.55f*timeLength+Time.fixedDeltaTime) ){
			transform.position = -posTarget;
		}
		else if( (t > 0 ) && (t < timeLength) ){
			transform.position = pos0 + dPos / dPos.magnitude * t * t * (posAcc/2f + posJerk/6f * t);
		}
	}

	// BOUGE L'ANGLE DU TUNNEL POUR DONNER L'IMPRESSION QUE LA CAMERA BOUGE -------------------------------------------------------
	// phiTarget   : angle dans le plan xz en degrees
	// thetaTarget : angle dans le plan yz en degrees
	// moveCam     : true pour rester dans le tunnel
	// startTime   : debut du deplacement
	// timeLength  : duree du deplacement 
	private Quaternion ang0;
	private float[] dEuler = new float[2];
	private float[] eulerAcc = new float[2];
	private float[] eulerJerk = new float[2];
	
	private void GoToAngle( float phiTarget, float thetaTarget, bool moveCam, float startTime, float timeLength ){
		float t = song.time () - startTime;
		if ( (t > - 1.1f*Time.fixedDeltaTime) && (t < 0) ) {
			ang0 = transform.rotation;
			dEuler[0] = thetaTarget - ang0.eulerAngles.x;
			dEuler[1] = phiTarget - ang0.eulerAngles.y;
			for(int i = 0; i<2; i++){
				eulerJerk[i] = -12f * dEuler[i] / Mathf.Pow (timeLength, 3f);
				eulerAcc[i]  = 6f * dEuler[i] / Mathf.Pow (timeLength, 2f);
			}
		}
		else if( (t >= timeLength-0.5f*Time.fixedDeltaTime) && (t <= timeLength+0.5f*Time.fixedDeltaTime) ){
			transform.rotation = Quaternion.Euler( thetaTarget, phiTarget, 0f );
		}
		else if( (t > 0 ) && (t < timeLength) ){
			transform.rotation = Quaternion.Euler (
				ang0.eulerAngles.x + t * t * (eulerAcc[0]/2f + eulerJerk[0]/6f * t),
				ang0.eulerAngles.y + t * t * (eulerAcc[1]/2f + eulerJerk[1]/6f * t), 0f );
		}

		if (moveCam) {
			camera.transform.position = transform.position - new Vector3(
				transform.position.z*Mathf.Cos (Mathf.PI*transform.rotation.eulerAngles.x/180f)*Mathf.Sin (Mathf.PI*transform.rotation.eulerAngles.y/180f),
			   -transform.position.z*Mathf.Sin (Mathf.PI*transform.rotation.eulerAngles.x/180f),
				transform.position.z*Mathf.Cos (Mathf.PI*transform.rotation.eulerAngles.x/180f)*Mathf.Cos (Mathf.PI*transform.rotation.eulerAngles.y/180f) );
		}
	}


	// ROTATION DU TUNNEL AUTOUR DE L'AXE Z + SPIN DES CUBES SUR EUX-MEMES --------------------------------------------------------
	// rotationSpeed en degres/seconde
	// spinAngle en degres
	private float zSpinSpeed1;
	private float zSpinSpeed2;
	private float zSpinSpeed3;
	private float rockTime1;
	private float rockTime2;
	private float omega2eTiers1;
	private float omega2eTiers2;
	private float omega3eTiers;

	private void doubleSpin( float rotationSpeedZ, float spinAngle ){

		float Theta_i;
		float dTheta = rotationSpeedZ * Time.fixedDeltaTime * Mathf.PI / 180f;;
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

	// FLEX THAT SHIT MON AMI ! ---------------------------------------------------------------------------------------------------
	private bool flexion = false;
	private bool[] flexionDone;
	private bool[] firstFlexFrame;
	private float[] ampFlexion;
	private float[] freqFactorY;
	private bool flexTunnel(CubeInfo[,] cubes1,CubeInfo[,] cubes2, float sinOffsetX, float sinOffsetY, int flexionID)  {	
		
		bool flexionDone = false;
		float amp = 1000f;
		
		// Si c'est la premiere frame de cette flexion, avertir TunnelSpinner pour qu'il cesse d'enregistrer la position
		if ( firstFlexFrame[flexionID]==true ) {
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
				cubes1[i,j].transform.localPosition = new Vector3 ((cubes1[i,j].posSansFlexion.x+ amp*Mathf.Pow((float)j,2f)*sinOffsetX),cubes1[i,j].posSansFlexion.y + amp*Mathf.Pow((float)j,2f)*sinOffsetY,cubes1[i,j].transform.localPosition.z);
			}
		}
		for(int i = 0; i<cubes2.GetLength(0); i++){
			for(int j = 1; j<cubes2.GetLength(1); j++){
				cubes2[i,j].transform.localPosition = new Vector3 ((cubes2[i,j].posSansFlexion.x+ amp*Mathf.Pow((float)j,2f)*sinOffsetX),cubes2[i,j].posSansFlexion.y+ amp*Mathf.Pow((float)j,2f)*sinOffsetY,cubes2[i,j].transform.localPosition.z);
			}
		}
		// Stop the flexion script when amp and the offset are small
		if (amp < 0.0001f){	
			flexionDone = true;
			endFlexion();
		}
		return flexionDone;
	}
	
	private void startFlexion(){
		flexion = true;
		for (int i = 0 ; i<tunnel.cubeCone1Array.GetLength(0); i++)
		{
			for (int j = 0 ; j<tunnel.cubeCone1Array.GetLength(1); j++)
			{
				tunnel.cubeCone1Array[i,j].posSansFlexion = tunnel.cubeCone1Array[i,j].transform.localPosition;
			}
		}
		for (int i = 0 ; i<tunnel.cubeCone2Array.GetLength(0); i++)
		{
			for (int j = 0 ; j<tunnel.cubeCone2Array.GetLength(1); j++)
			{
				tunnel.cubeCone2Array[i,j].posSansFlexion = tunnel.cubeCone2Array[i,j].transform.localPosition;
			}
		}
	}
	
	private void endFlexion(){
		flexion = false;
		for (int i = 0 ; i<tunnel.cubeCone1Array.GetLength(0); i++)
		{
			for (int j = 0 ; j<tunnel.cubeCone1Array.GetLength(1); j++)
			{
				tunnel.cubeCone1Array[i,j].transform.localPosition = tunnel.cubeCone1Array[i,j].posSansFlexion;
			}
		}
		for (int i = 0 ; i<tunnel.cubeCone2Array.GetLength(0); i++)
		{
			for (int j = 0 ; j<tunnel.cubeCone2Array.GetLength(1); j++)
			{
				tunnel.cubeCone2Array[i,j].transform.localPosition = tunnel.cubeCone2Array[i,j].posSansFlexion;
			}
		}
	}


	// LA DESCENTE AUX ENFERS ! ---------------------------------------------------------------------------------------------------
	private float speedDae;
	private float accelDae;
	private float[] resetDistDae;
	private bool separation = false;
	private int iDae = 0;

	private void descenteAuxEnfers( float topSpeed, float accelTime, float startTime, float topSpeedtimeLength ){
		float t = song.time () - startTime;

		// initialisation
		if ((t > - 1.5 * Time.fixedDeltaTime) && (t < 0f)) {
			transform.position = new Vector3 (0f,0f,transform.position.z);
			// Il faut que la camera soit en (0,0,0)
			camera.transform.position = new Vector3 (0f, 0f, 0f);
			transform.rotation = Quaternion.Euler( 0f, 0f, 0f );
			speedDae = 0f;
			accelDae = topSpeed / accelTime;
			resetDistDae = new float[2] {134.5f,543f};
		
			// deplacement
		} else if ((t >= 0f) && (t < accelTime)) {
			transform.position -= new Vector3( 0f, 0f, Time.fixedDeltaTime * (speedDae + 0.5f * accelDae * Time.fixedDeltaTime) );
			speedDae += accelDae * Time.fixedDeltaTime; 

		} else if ((t >= accelTime) && (t < accelTime + topSpeedtimeLength)) {
			transform.position -= new Vector3( 0f, 0f, topSpeed * Time.fixedDeltaTime );

		} else if ((t >= accelTime + topSpeedtimeLength) && (t < 2f * accelTime + topSpeedtimeLength)) {
			if (speedDae > 0f) {
				transform.position -= new Vector3( 0f, 0f, Time.fixedDeltaTime * (speedDae + 0.5f * -accelDae * Time.fixedDeltaTime) );
				speedDae -= accelDae * Time.fixedDeltaTime; 
			}
		}

		// offset et separation
		if (transform.position.z <= -31f){
			for (int i = 0; i<tunnel.cubeCone1Array.GetLength(0); i++) {
				for (int j = 0; j<tunnel.cubeCone1Array.GetLength(1); j++) {
					tunnel.cubeCone1Array[i,j].transform.localPosition -= new Vector3( 0f, 0f, resetDistDae[iDae] );
				}
			}
			separation = true;
			transform.position += new Vector3( 0f, 0f, resetDistDae[iDae] );
		}

		// recombinaison
		if (separation && transform.position.z <= 31f) {
			for (int i = 0; i<tunnel.cubeCone1Array.GetLength(0); i++) {
				for (int j = 0; j<tunnel.cubeCone1Array.GetLength(1); j++) {
					tunnel.cubeCone1Array [i,j].transform.localPosition += new Vector3( 0f, 0f, resetDistDae[iDae] );
				}
			}
			separation = false;
			iDae++;
		}
	
	}

}
