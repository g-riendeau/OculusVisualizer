using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public Material CubeMat;
	
	// parametres globaux
	private const float nCubesW = 10f;  			 	// nb de cubes sur la en Largeur
	private const float nCubesP = 200f;  			 	// nb de cubes sur la en Profondeur
	private const float height = 0.5f; 			 		// hauteur des cubes par defaut
	private const float longueur = 1f; 			 		// longueur des cubes par defaut

	private List<FloorCube> objetInRangeList  = new List<FloorCube>();	
	private GameObject[] activeFloorCubes;
	private Vector4 passiveColor;
	private Vector4 activeColor;

	private float pathConstant1;
	private float pathConstant2;
	private float pathConstant3;
	private float pathConstant4;
	private float pathConstant5;
	private float pathConstant6;
	private float pathConstant7;
	private float pathConstant8;
	private float pathConstant9;
	private float pathConstant10;

	// Use this for initialization
	void Awake () {

		//Creation des parametre du monde
		pathConstant1 = Random.Range (2f, 5f);
		pathConstant2 = Random.Range (-0.1f, 0.1f);
		pathConstant3 = Random.Range (1f, 5f);
		pathConstant4 = Random.Range (-0.03f, 0.03f);
		pathConstant5 = Random.Range (-0.2f, 0.2f);
		pathConstant6 = Random.Range (2f, 5f);
		pathConstant7 = Random.Range (-0.1f, 0.1f);
		pathConstant8 = Random.Range (1f, 5f);
		pathConstant9 = Random.Range (-0.03f, 0.03f);
		pathConstant10 = Random.Range (-0.2f, 0.2f);

		passiveColor = new Vector4(0.9f,0.9f,0.9f,1);
		activeColor = new Vector4(0.1f,0.1f,0.1f,1);

		// Creation du monde
		// Creation du premier chemin (chemin etroit)
		for (float j = 0f; j < nCubesP; j++)
		{
			float rowXPosition = pathConstant1*Mathf.Sin (j*pathConstant2)*pathConstant3*Mathf.Sin (j*pathConstant4)+ j*pathConstant5-j*j*pathConstant5/150;
			float rowYPosition = pathConstant6*Mathf.Sin (j*pathConstant7)*pathConstant8*Mathf.Sin (j*pathConstant9)+ j*pathConstant10-j*j*pathConstant10/150;
			for (float i = 0f; i < nCubesW; i++)
			{

				// on creer et place le cube
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = this.transform;
				cube.transform.localPosition = new Vector3(transform.position.x+longueur*(i-nCubesW/2)+rowXPosition,transform.position.y+rowYPosition,transform.position.z+longueur*(j-1));
				cube.transform.localScale = new Vector3(0.97f,1f,0.97f);

				cube.GetComponent<Renderer>().material = CubeMat;
				cube.GetComponent<Renderer>().material.color = new Vector4(0.9f,0.9f,0.9f,1);
				cube.tag = "PassiveCube";
		}

		}
	}
	
	// Update is called once per frame
	void Update () {

		activeFloorCubes =  GameObject.FindGameObjectsWithTag("ActiveCube");
		foreach (GameObject cube in activeFloorCubes) {
			Vector4 cubePreviousColor = cube.GetComponent<Renderer>().material.color;
			Vector4 cubeColor = Color.Lerp(cubePreviousColor, activeColor, Time.time/500);
			cube.GetComponent<Renderer>().material.color = cubeColor;
		}
	}
		

	public void ActivateFloorCube(GameObject unFloorCube) {


		
	}
}
