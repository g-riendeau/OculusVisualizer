using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFinder : MonoBehaviour 
{
	[SerializeField]
	private string _name;

	private static Dictionary<string, GameObject> _instanceList = new Dictionary<string, GameObject>();

	void Awake()
	{
		_name = string.IsNullOrEmpty(_name) ? gameObject.name : _name;
		_instanceList.Add(_name, this.gameObject);
	}

	public static GameObject GetObject(string name)
	{
		GameObject value;
		if (_instanceList.TryGetValue(name, out value))
		{
			return value;
		}
		return null;
	}

	public static T GetComponent<T>(string name) 
		where T : MonoBehaviour
	{
		GameObject value;
		if (_instanceList.TryGetValue(name, out value))
		{
			T comp = value.GetComponent<T>();
			if (comp != null)
			{
				return comp;
			}
		}
		return null;
	}
}
