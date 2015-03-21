using UnityEngine;
using System.Collections;

public class BlackScript : MonoBehaviour {

	public Material BlackToonMat;
	private LineRenderer blackLine; 
	private float tailSpeed;
	private float headSpeed;
	private float zSpeed;
	private float zStart;
	private Vector3 headTo;
	private Vector3 p0; //Line positions
	private Vector3 p1;
	private  float spikeTimeInterval; // secondes
	private  float timeSinceLastSpike; // secondes

	// Use this for initialization
	void Start () {
	
		//Initialize line positions
		blackLine = GetComponent<LineRenderer>();
		zStart = 100f;
		p0 = new Vector3(0f,0f,zStart);
		p1 = new Vector3(0.5f,0f,zStart);
		headTo = new Vector3(0f, 0f, zStart);
		blackLine.SetPosition(0, p0);
		blackLine.SetPosition(1, p1);
		blackLine.SetColors(Color.blue, Color.red);
		zSpeed = 2f;
		blackLine.material = new Material(Shader.Find("Particles/Alpha Blended"));
		blackLine.SetColors(Color.black, Color.white);
		tailSpeed = 8f;
		headSpeed = 25f;
		timeSinceLastSpike = 0f;
		spikeTimeInterval = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {

		timeSinceLastSpike += Time.deltaTime;
		if(timeSinceLastSpike > spikeTimeInterval)
		{
			//Vector3 spike = new Vector3(0f,0f,0f);
			Vector3 spike = new Vector3(Random.Range (-7f,7f), Random.Range (-7f,7f), Mathf.Max (zStart - zSpeed*Time.time, 0f)); 	
			headTo = spike;
			timeSinceLastSpike = 0f;
			spikeTimeInterval = Random.Range (0.05f,0.7f);

		}


		blackLine.SetPosition(0, p0);
		seek (Time.deltaTime, headTo);
		follow (Time.deltaTime);
	}

	void follow(float elaspedTime){
	
		Vector3 dir = p0 - p1;
		p1 = p1 + (tailSpeed * elaspedTime *  dir);
		blackLine.SetPosition(1, p1);
	}

	void seek(float elaspedTime, Vector3 headTo){
		
		Vector3 dir = headTo - p0;
		p0 = p0 + (headSpeed * elaspedTime *  dir);
		blackLine.SetPosition(0, p0);
	}
}
