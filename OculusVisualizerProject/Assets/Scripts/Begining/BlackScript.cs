using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlackScript : MonoBehaviour {

	public Material blackToonMat;
	private List<LineRenderer> lines; 
	private float tailSpeed;
	private float headSpeed;
	private float zSpeed;
	private float zStart;
	private List<Vector3> headTo; //head objective
	private List<Vector3> p0s; 	//head positions
	private List<Vector3> p1s;	// tail positions
	private List<float> spikeTimeIntervals; // secondes
	private  float lineTimeInterval; // secondes
	private  List<float> timesSinceLastSpike; // secondes
	private  float timeSinceLastLine; // secondes
	private  float planeWidth;

	// Use this for initialization
	void Start () {

		//Initiliazations
		lines = new List<LineRenderer>(); 
		p0s = new List<Vector3>();
		p1s = new List<Vector3>();
		headTo = new List<Vector3>();
		timesSinceLastSpike = new List<float>();
		spikeTimeIntervals = new List<float>();
		zStart = 100f;
		zSpeed = 200;
		tailSpeed = 5f;
		headSpeed = 7f;
		timeSinceLastLine = 0f;
		lineTimeInterval = 5f;
		planeWidth = 15f;

		//Add first line
		addLine();
	}
	
	// Update is called once per frame
	void Update () {

		timeSinceLastLine += Time.deltaTime;
		if (timeSinceLastLine > lineTimeInterval){
			addLine ();
			timeSinceLastLine = 0f;
		}

		for (int i = 0 ; i < lines.Count ; i++)
		{
			findNewSpikes(i);
			lines[i].SetPosition(0, p0s[i]);
			seek (Time.deltaTime, i);
			follow (Time.deltaTime, i);
		}
	}

	//Find new headTo
	void findNewSpikes(int idx)
	{
		timesSinceLastSpike[idx] += Time.deltaTime;
		if(timesSinceLastSpike[idx] > spikeTimeIntervals[idx])
		{
			//Vector3 spike = new Vector3(0f,0f,0f);
			Vector3 spike = new Vector3(Random.Range (-planeWidth,planeWidth), Random.Range (-planeWidth,planeWidth), Mathf.Max (p0s[idx].z - zSpeed*Time.deltaTime, 0f)); 	
			headTo[idx] = spike;
			timesSinceLastSpike[idx] = 0f;
			spikeTimeIntervals[idx] = Random.Range (0.1f,1.1f);
		}
	}

	//Tail of a line following the head
	void follow(float elaspedTime, int idx){
	
		Vector3 dir = p0s[idx] - p1s[idx];
		p1s[idx] = p1s[idx] + (tailSpeed * elaspedTime *  dir);
		lines[idx].SetPosition(1, p1s[idx]);
	}

	//Head of line seeking headTo
	void seek(float elaspedTime, int idx){
		
		Vector3 dir = headTo[idx] - p0s[idx];
		p0s[idx] = p0s[idx] + (headSpeed * elaspedTime *  dir);
		lines[idx].SetPosition(0, p0s[idx]);
	}

	//Add new line
	void addLine(){
		GameObject line = new GameObject();
		LineRenderer blackLine = line.AddComponent<LineRenderer>();
		lines.Add(blackLine);
		p0s.Add (new Vector3(0f,0f,zStart));
		p1s.Add (new Vector3(0.5f,0f,zStart));
		headTo.Add(new Vector3(0f, 0f, zStart));
		int max = lines.Count-1;
		spikeTimeIntervals.Add (1.0f);
		lines[max].SetPosition(0, p0s[0]);
		lines[max].SetPosition(1, p1s[0]);
		lines[max].SetWidth(0.1f, 0.07f);		
		lines[max].material = new Material(Shader.Find("Diffuse"));
		lines[max].SetColors(Color.black, Color.white);
		timesSinceLastSpike.Add(0f);
	}
}
