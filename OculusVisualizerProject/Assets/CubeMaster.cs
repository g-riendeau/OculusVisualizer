using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class CubeMaster : MonoBehaviour {
	//private float _posX;
	//private float _posY;
	public AudioProcessor audioProcessor ;
	public CubeWallComponent floor;

	private const int cGraves = 512;
	private float[] amplitudesGraves;

	private GameObject[,] floorCubes;


	// Use this for initialization
	void Start () {
		amplitudesGraves = new float[cGraves];
		floorCubes = floor.cubeArray;
	}
	
	// Update is called once per frame
	void Update () {
		Array.Copy(audioProcessor.amplitudes, 0, amplitudesGraves, 0, 512);
		float meanGraves = 500 * amplitudesGraves.Average();
		//Debug.Log(meanGraves);

		floorCubes[0,0].transform.localScale = new Vector3(1f, meanGraves, 1f);


	}
}
 