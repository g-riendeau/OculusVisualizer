using UnityEngine;
using System.Collections;

public class AudioProcessor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float[] spectrum = audio.GetSpectrumData(4096, 0, FFTWindow.BlackmanHarris);
		int i = 1;
		while (i < 1023) {
			//Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
			//Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
			Debug.DrawLine(new Vector3((float)((i-1)/100.0f - 5.12f), (float)( 12.0f+5*spectrum[i - 1]  - 10.0f ), 1.0f), 
			               new Vector3((float)((i)/100.0f - 5.12f), (float)(12.0f+5*spectrum[i] - 10.0f), 1.0f), 
			               Color.green);
			//Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.yellow);
			i++;
		}
	}
}
