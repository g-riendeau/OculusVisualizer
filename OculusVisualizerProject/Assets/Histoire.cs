using UnityEngine;
using System.Collections;

public class Histoire : MonoBehaviour {

	public GameObject Eclair;

	private float tempsPresent;
	private float temps1 = 5;
	private float temps2 = 12;
	//private bool calmWeather = true;

	// Use this for initialization
	void Start () {			



		
	}
	
	// Update is called once per frame
	public void Update () {

		// Un éclair!
		if (Time.realtimeSinceStartup > temps1)	{
			//calmWeather = false;
			for (int j = 0; j < Random.Range(1,4); j++) {
				GameObject unEclair = (GameObject)Instantiate (Eclair);
				LineRenderer elcairLR = unEclair.GetComponent<LineRenderer>();

				elcairLR.SetPosition( 0, new Vector3(0,-1,0));

				for (int i = 1; i < 10; i++) {
					elcairLR.SetPosition( i, new Vector3(Random.Range (-5,5),i*10-1,Random.Range (-5,5)));
				}
				StartCoroutine(WaitAndDestroy(0.1f,unEclair));
			}
			temps1 = temps1+5;
			
		}
	}


	IEnumerator WaitAndDestroy(float waitTime,GameObject gameObject) {
		yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);
	}
}
