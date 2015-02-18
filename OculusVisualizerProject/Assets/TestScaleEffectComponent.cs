using UnityEngine;
using System.Collections;

public class TestScaleEffectComponent : MonoBehaviour 
{
	private float _x;
	private float _y;

	public void Initialize(float x, float y)
	{
		_x = x;
		_y = y;
	}

	// Update is called once per frame
	void Update () 
	{
		float scaleEffect = Mathf.Sin((2.0f*_x+0.5f*_y)*Time.time * 1.0f);
		this.transform.localScale = new Vector3(1f, scaleEffect, 1f);
	}
}
