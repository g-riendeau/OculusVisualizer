using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeWallComponent : MonoBehaviour 
{
	private const float kWidth = 10f;
	private const float kDepth = 100f;

	// Use this for initialization
	void Start() 
	{
		for (float i = -(kWidth * 0.5f); i < (kWidth * 0.5f); i++)
		{
			for (float j = 0f; j < kDepth; j++)
			{
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = this.transform;
				cube.transform.localPosition = new Vector3(i, 0f, j);
				cube.transform.localScale = new Vector3(1f, 0.1f, 1f);

				TestScaleEffectComponent effectComp = cube.AddComponent<TestScaleEffectComponent>();
				effectComp.Initialize(i, j);
			}
		}
	}
}
