using UnityEngine;
using System;
using System.Collections;

public class TransitionDoctor : MonoBehaviour 
{
	public event Action<float> OnOpeningTransitionStarted = delegate {};
	public event Action OnSceneStarted = delegate {};
	public event Action<float> OnClosingTransitionStarted = delegate {};

	[SerializeField]
	private SceneSettings _sceneSettings;

	private float _elapsedTime;

	// Use this for initialization
	void Start () 
	{
		if (_sceneSettings != null)
		{
			StartCoroutine(WaitForEndOfScene());
		}
		else
		{
			Debug.LogError("SceneStarter: Scene settings object was not found...");
		}
	}
	
	// Increment the elapsed time
	void Update () 
	{
		_elapsedTime += Time.deltaTime;
	}

	/// <summary>
	/// Waits till the time is elapsed
	/// </summary>
	private IEnumerator WaitForEndOfScene()
	{
		// Trigger the opening animation and gives the transition duration
		OnOpeningTransitionStarted(_sceneSettings.OpeningTransitionDuration);

		// Wait...
		while (_elapsedTime < _sceneSettings.OpeningTransitionDuration)
		{
			yield return null;
		}

		// The opening transition is over, start animation stuff...
		OnSceneStarted();
		
		// Wait...
		_elapsedTime = 0;
		while (_elapsedTime < _sceneSettings.SceneDuration)
		{
			yield return null;
		}
		
		// Trigger the closing animation and gives the transition duration
		OnClosingTransitionStarted(_sceneSettings.ClosingTransitionDuration);
		
		// Wait...
		_elapsedTime = 0;
		while (_elapsedTime < _sceneSettings.ClosingTransitionDuration)
		{
			yield return null;
		}

		// Load the level
		Application.LoadLevel(_sceneSettings.GetNextSceneName());
	}
}
