﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeWallComponent : MonoBehaviour 
{
	private const float cWidth = 10f;
	private const float cDepth = 100f;
	public GameObject[,] cubeArray = new GameObject[(int)cWidth,(int)cDepth];

	// Use this for initialization
	void Start() 
	{
		//On veut des indice entiers
		int idxi = 0;
		int idxj = 0;
		for (float i = -(cWidth * 0.5f); i < (cWidth * 0.5f); i++)
		{
			idxj = 0;
			for (float j = 0f; j < cDepth; j++)
			{
				GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.parent = this.transform;
				cube.transform.localPosition = new Vector3(i, 0f, j);
				cube.transform.localScale = new Vector3(1f, 0.1f, 1f);

				TestScaleEffectComponent effectComp = cube.AddComponent<TestScaleEffectComponent>();
				effectComp.Initialize(i, j);

				CubeIndex depth = cube.AddComponent<CubeIndex>();
				depth.Initialize(idxi);

				cubeArray[idxi,idxj] = cube;
				idxj ++;
			}
			idxi ++;
		}
	}
}
