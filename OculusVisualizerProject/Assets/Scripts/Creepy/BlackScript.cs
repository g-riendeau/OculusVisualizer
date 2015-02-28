using UnityEngine;
using System.Collections;

public class BlackScript : MonoBehaviour {

	public Material BlackToonMat;
	private LineRenderer blackLine; 
	private float tailSpeed;
	private float headSpeed;
	private Vector3 headTo;
	private Vector3 p0; //Line positions
	private Vector3 p1;
	private  const float cSpikeTimeInterval = 1f; // secondes
	private  float timeSinceLastSpike; // secondes

	// Use this for initialization
	void Start () {
	
		//Initialize line positions
		blackLine = GetComponent<LineRenderer>();
		p0 = new Vector3(0f,0f,0f);
		p1 = new Vector3(0.5f,0f,0f);
		headTo = new Vector3(0f, 0f, 0f);
		blackLine.SetPosition(0, p0);
		blackLine.SetPosition(1, p1);
		blackLine.SetColors(Color.blue, Color.red);
		//blackLine.material = new Material(Shader.Find("Particles/Additive"));
		//blackLine.material.SetColor("_Color", Color.red);

		tailSpeed = 1f;
		headSpeed = 30f;
		timeSinceLastSpike = 0f;

	}
	
	// Update is called once per frame
	void Update () {

		timeSinceLastSpike += Time.deltaTime;
		if(timeSinceLastSpike > cSpikeTimeInterval)
		{
			Vector3 spike = new Vector3(0f,0f,0f);
			while( Mathf.Abs(spike.x) < 1.5f || Mathf.Abs(spike.y) < 1.5f )
			{
				spike = new Vector3(Random.Range (-5f,5f), Random.Range (-5f,5f), 0); 
			}
			headTo = p0 + spike;
			timeSinceLastSpike = 0f;
		}


		blackLine.SetPosition(0, p0);
		blackLine.SetColors(Color.black, Color.white);
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
