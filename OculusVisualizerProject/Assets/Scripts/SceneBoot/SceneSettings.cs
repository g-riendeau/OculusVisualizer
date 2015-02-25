using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class SceneSettings 
{
	/// Scenes available
	public enum MindfuckScenes
	{
		Cubes = 0,
		Clouds = 1,
		Creepy = 2
	}
	
	[SerializeField]
	private float _openingTransitionDuration;
	[SerializeField]
	private float _sceneDuration;
	[SerializeField]
	private float _closingTransitionDuration;
	[SerializeField]
	private MindfuckScenes _nextMindfuckScene;

	/// <summary>
	/// Gets the duration of the opening transition.
	/// </summary>
	public float OpeningTransitionDuration
	{
		get { return _openingTransitionDuration; }
	}

	/// <summary>
	/// Gets the scene time.
	/// </summary>
	public float SceneDuration
	{
		get { return _sceneDuration; }
	}

	/// <summary>
	/// Gets the duration of the closing transition.
	/// </summary>
	public float ClosingTransitionDuration
	{
		get { return _closingTransitionDuration; }
	}

	/// <summary>
	/// Gets the name of the next scene.
	/// </summary>
	public string GetNextSceneName()
	{
		switch (_nextMindfuckScene)
		{
			case MindfuckScenes.Cubes:
			{
				return "scn_Cubes";
			}
			case MindfuckScenes.Clouds:
			{
				return "scn_Clouds";
			}
			case MindfuckScenes.Creepy:
			{
				return "scnbyoto_CreepyHouse";
			}
			default:
			{
				Debug.LogError("SceneSettings: Unknown scene");
				return string.Empty;
			}
		}
	}
}
