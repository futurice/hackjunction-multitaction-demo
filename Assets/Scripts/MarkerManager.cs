using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MarkerManager : MonoBehaviour
{
	[Serializable]
	public struct MarkerIdToQueryMapping
	{
		public int 		markerId;
		public string	query;
	}

	[Serializable]
	public struct MarkerIdToToolMapping
	{
		public int 			markerId;
		public GameObject	toolPrefab;
	}

	// Note: arrays exist to expose the dictionaries to Unity editor
	//		 inspector view

	[SerializeField]
	public			MarkerIdToQueryMapping[]	_markerIdToQueryArray;
	private 		Dictionary<int, string> 	_markerIdToQueryDict 	= new Dictionary<int, string> ();

	[SerializeField]
	public			MarkerIdToToolMapping[]		_markerIdToToolArray;
	private 		Dictionary<int, GameObject> _markerIdToToolDict		= new Dictionary<int, GameObject> ();

	private static 	MarkerManager 				_instance 				= null;

	public static MarkerManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<MarkerManager> ();
			}

			return _instance;
		}
	}

	void Awake () 
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else
		{
			Debug.LogWarning ("Multiple MarkerManagers present, disabling: {0}", this.gameObject);
			enabled = false;
		}

		// Populate the marker id to x -dictionaries
		if (_markerIdToQueryArray != null)
		{
			foreach (MarkerIdToQueryMapping m in _markerIdToQueryArray)
			{
				_markerIdToQueryDict.Add (m.markerId, m.query);
			}
		}

		if (_markerIdToToolArray != null)
		{
			foreach (MarkerIdToToolMapping m in _markerIdToToolArray)
			{
				_markerIdToToolDict.Add (m.markerId, m.toolPrefab);
			}
		}
	}

	void Start () 
	{
		// TODO: subscribe to marker recognition stream and
		// call actions based on received markers
	}
	
	void Update () 
	{
	}
}
