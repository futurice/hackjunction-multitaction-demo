using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TouchScript;

public class TagManager : MonoBehaviour
{
	[Serializable]
	public struct TagNameToToolMapping
	{
		public string 		tagName;
		public GameObject	toolPrefab;
	}

	// Note: arrays exist to expose the dictionaries to Unity editor
	//		 inspector view
	[SerializeField]
	public			TagNameToToolMapping[]			_tagNameToToolArray;
	private 		Dictionary<string, GameObject> 	_tagNameToToolDict		= new Dictionary<string, GameObject> ();

	[SerializeField]
	private			List<string>					_ignoredTagNames		= new List<string> ();

	private static 	TagManager 						_instance 				= null;

	public static TagManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<TagManager> ();
			}

			return _instance;
		}
	}

	private void Awake () 
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

		if (_tagNameToToolArray != null)
		{
			foreach (TagNameToToolMapping m in _tagNameToToolArray)
			{
				_tagNameToToolDict.Add (m.tagName, m.toolPrefab);
			}
		}
	}

	private void Start () 
	{
	}

	private void OnEnable ()
	{
		if (TouchManager.Instance != null)
		{
			TouchManager.Instance.TouchesBegan += TouchesBeganHandler;
		}
	}

	private void OnDisable ()
	{
		if (TouchManager.Instance != null)
		{
			TouchManager.Instance.TouchesBegan += TouchesBeganHandler;
		}
	}
	
	private void Update () 
	{
	}

	private void HandleTags (ITouch touch)
	{
		Tags tags = touch.Tags;

		if (tags.Count > 0)
		{
			Debug.Log (string.Format ("Tag detected: {0}", tags));
			
			foreach (string tag in tags.TagList)
			{
				// Check whether this tag should be ignored e.g. Mouse etc.
				if (_ignoredTagNames.Contains (tag))
				{
					Debug.Log (string.Format ("Ignoring tag: {0}", tag));
					continue;
				}
				// Check whether this tag is a tool tag
				else if (_tagNameToToolDict.ContainsKey (tag))
				{
					Debug.Log (string.Format ("Instantiating tool with tag: {0}", tag));

					Vector3 worldPosition = Camera.main.ScreenToWorldPoint (new Vector3 (touch.Position.x,
						                                             					 touch.Position.y,
						                                             					 0.0f));

					Instantiate (_tagNameToToolDict [tag], worldPosition, Quaternion.identity);
					// TODO: Instantiate at the tags position and rotation
					// TODO: Make the tool follow the tag
				}
				// If nothing else, search for Tweets with the tag
				else
				{
					Debug.Log (string.Format ("Searching for tweets with tag: {0}", tag));
					TweetManager.Instance.GetTweets (tag);
				}
			}
		}
	}

	private void TouchesBeganHandler (object sender, TouchEventArgs e)
	{
		foreach (ITouch touch in e.Touches)
		{
			HandleTags (touch);
		}
	}
}
