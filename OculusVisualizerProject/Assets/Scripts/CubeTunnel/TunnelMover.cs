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
	void Update () {

		//Debug.Log (song.time ());
		//Debug.Log ("x:" + transform.position.x + ", y:" + transform.position.y+ ", z:" + transform.position.z);

		GoToPosition (new Vector3 (0f, 0f, 5f), 70f, 10f);
		GoToAngle( 180f, 360f, true, 50f,  39f );

		GoToPosition (new Vector3 (0f, 0f,-10f), 140f, 12.5f);
		GoToPosition (new Vector3 (0f, 0f, 10f), 153f, 12.5f);
		GoToPosition (new Vector3 (0f, 0f,  5f), 166f, 12.5f);
		GoToPosition (new Vector3 (0f, 0f, 12f), 202f, 12f);
		GoToPosition (new Vector3 (0f, 0f, 0f), rockTime2, 2f);


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
			doubleSpin (0f, 90f * Mathf.Sin (omega2eTiers1 * t) * Mathf.Sin (omega2eTiers2 * t));
		} else if (estDans (rockTime2, 2f)) {
			t = song.time () - rockTime2;
			doubleSpin (540f * t * (1f - t / 2f), 0f);
		}
		else if (estDans(song.debut3eTiers, song.length3eTiers)){
			doubleSpin( zSpinSpeed3, 45f+25f*Mathf.Sin (omega3eTiers*(song.time()-song.debut3eTiers)) );
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
	private float posDt;
	private Vector3 pos0;
	private Vector3 dPos;
	private float posJerk;
	private float posAcc;

	private void GoToPosition( Vector3 posTarget, float startTime, float timeLength ){
		posDt = song.time () - startTime;
		if ( (posDt > - 1.5*Time.deltaTime) && (posDt < 0f) ) {
			pos0 = transform.position;
			dPos = -posTarget - pos0;
			posJerk = -12f * dPos.magnitude / Mathf.Pow (timeLength, 3f);
			posAcc  = 6 * dPos.magnitude / Mathf.Pow (timeLength, 2f);
		}
		else if( (posDt >= timeLength-0.5f*Time.deltaTime) && (posDt <= 0.5f*timeLength+Time.deltaTime) ){
			transform.position = -posTarget;
		}
		else if( (posDt > 0 ) && (posDt < timeLength) ){
			transform.position = pos0 + dPos / dPos.magnitude * posDt * posDt * (posAcc/2f + posJerk/6f * posDt);
		}
	}

	// BOUGE L'ANGLE DU TUNNEL POUR DONNER L'IMPRESSION QUE LA CAMERA BOUGE -------------------------------------------------------
	// phiTarget   : angle dans le plan xz en degrees
	// thetaTarget : angle dans le plan yz en degrees
	// moveCam     : true pour rester dans le tunnel
	// startTime   : debut du deplacement
	// timeLength  : duree du deplacement 
	private float angDt;
	private Quaternion ang0;
	private float[] dEuler = new float[2];
	private float[] eulerAcc = new float[2];
	private float[] eulerJerk = new float[2];
	
	private void GoToAngle( float phiTarget, float thetaTarget, bool moveCam, float startTime, float timeLength ){
		angDt = song.time () - startTime;
		if ( (angDt > - 1.5f*Time.deltaTime) && (angDt < 0) ) {
			ang0 = transform.rotation;
			dEuler[0] = thetaTarget - ang0.eulerAngles.x;
			dEuler[1] = phiTarget - ang0.eulerAngles.y;
			for(int i = 0; i<2; i++){
				eulerJerk[i] = -12f * dEuler[i] / Mathf.Pow (timeLength, 3f);
				eulerAcc[i]  = 6f * dEuler[i] / Mathf.Pow (timeLength, 2f);
			}
		}
		else if( (angDt >= timeLength-0.5f*Time.deltaTime) && (angDt <= timeLength+0.5f*Time.deltaTime) ){
			transform.rotation = Quaternion.Euler( thetaTarget, phiTarget, 0f );
		}
		else if( (angDt > 0 ) && (angDt < timeLength) ){
			transform.rotation = Quaternion.Euler (
				ang0.eulerAngles.x + angDt * angDt * (eulerAcc[0]/2f + eulerJerk[0]/6f * angDt),
				ang0.eulerAngles.y + angDt * angDt * (eulerAcc[1]/2f + eulerJerk[1]/6f * angDt), 0f );
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
				cubes1[i,j].transform.localPosition = new Vector3 ((cubes1[i,j].posSansFlexion.x+ amp*Mathf.Pow(j,2)*sinOffsetX),cubes1[i,j].posSansFlexion.y + amp*Mathf.Pow(j,2)*sinOffsetY,cubes1[i,j].transform.localPosition.z);
			}
		}
		for(int i = 0; i<cubes2.GetLength(0); i++){
			for(int j = 1; j<cubes2.GetLength(1); j++){
				cubes2[i,j].transform.localPosition = new Vector3 ((cubes2[i,j].posSansFlexion.x+ amp*Mathf.Pow(j,2)*sinOffsetX),cubes2[i,j].posSansFlexion.y+ amp*Mathf.Pow(j,2)*sinOffsetY,cubes2[i,j].transform.localPosition.z);
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
				cubes[i,j].transform.localPosition = cubes[i,j].posSansFlexion;
			}
		}
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

}
