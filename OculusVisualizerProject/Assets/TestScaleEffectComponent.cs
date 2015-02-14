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
		float scaleEffect = Mathf.Sin(_x * Time.time * 0.5f) * Mathf.Sin(_y * Time.time);
		this.transform.localScale = new Vector3(1f, scaleEffect, 1f);
	}
}
