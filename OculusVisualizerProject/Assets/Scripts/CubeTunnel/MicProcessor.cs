using UnityEngine;
using System.Collections;
using System;

public class MicProcessor : AudioProcessor
{
	// Update is called once per frame
	public override void FixedUpdate ()
	{
		GetComponent<AudioSource>().GetSpectrumData (amplitudes, 0, FFTWindow.BlackmanHarris);
		base.FixedUpdate(); 
	}
}
