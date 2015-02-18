using UnityEngine;
using System.Collections;

public class AudioProcessor : MonoBehaviour {
	public float[] amplitude = new float[4096];

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float[] spectrum = audio.GetSpectrumData(4096, 0, FFTWindow.BlackmanHarris);
		int i = 1;
		while (i < 1023) {
			Debug.DrawLine(new Vector3((float)((i-1)/100.0f - 5.12f), (float)( 12.0f+5*spectrum[i - 1]  - 10.0f ), 1.0f), 
			               new Vector3((float)((i)/100.0f - 5.12f), (float)(12.0f+5*spectrum[i] - 10.0f), 1.0f), 
			               Color.green);
			i++;
		}
	}
}
