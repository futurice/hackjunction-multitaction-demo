using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections.Generic;

public class TweetManager : MonoBehaviour
{
	[SerializeField]
	private string 					_testQuery 			= "#sten";

	[SerializeField]
	private int						_maxTweetsPerSearch	= 5;

	[SerializeField]
	private Transform				_tweetContainer		= null;

	[SerializeField]
	private GameObject		 		_tweetPrefab 		= null;

	[SerializeField]
	private GameObject				_spinnerPrefab		= null;

	[SerializeField]
	private List<Sprite> 			_tweetBackgrounds 	= new List<Sprite> ();

	private static 	TweetManager 	_instance 			= null;
	
	public static TweetManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<TweetManager> ();
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
			Debug.LogWarning ("Multiple TweetManagers present, disabling: {0}", this.gameObject);
			enabled = false;
		}
	}

	private void Start ()
	{
		// Make a buffered and throttled stream from the space key
		// presses
		IObservable<long> clickStream = Observable.EveryUpdate ()
			.Where (_ => Input.GetKeyDown (KeyCode.Space));
		
		clickStream.Buffer (clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
			.Where(x => x.Count >= 1)
			.Subscribe(x => CreateTweets (_testQuery, new Vector3 (0.0f, 0.0f, 0.0f)));

		// Observe any tweets leaving the game field and destroy them
		this.OnTriggerEnter2DAsObservable ()
			.Subscribe (col => 
	            {
					if (col.gameObject.CompareTag ("Tweet"))
		    		{
						Destroy (col.gameObject);
					}
				});
	}
	
	private void Update ()
	{
	}

	public void CreateTweets (string query, Vector3 worldPosition)
	{
		// Instantiate a new loading animation prefab
		GameObject loadingAnimation = Instantiate (_spinnerPrefab, worldPosition, Quaternion.identity) as GameObject;

		Debug.Log (string.Format ("Searching for Tweets with query: {0}", query));
		QueryfeedAPI.SearchTweets (query)
			.Delay (TimeSpan.FromMilliseconds (500))
			.Subscribe (
				tl => 
				{
					Debug.Log (string.Format ("Retrieved {0} Tweets successfully", tl.Tweets.Count));
					Destroy (loadingAnimation);

					// Throttle the number of tweets shown according to the set value
					if (tl.Tweets.Count > _maxTweetsPerSearch)
					{
						tl.Tweets.RemoveRange (_maxTweetsPerSearch-1, tl.Tweets.Count - _maxTweetsPerSearch);
					}

					foreach (Tweet t in tl.Tweets)
					{
						Debug.Log (string.Format ("Author: {0}, Description: {1}", t.Author, t.Description));
						InstantiateTweetObject (t, worldPosition);
					}
				},
				ex =>
				{
					Debug.Log (string.Format ("Failed to retrieve Tweets for query {0}, ex: {1}", query, ex));
					Destroy (loadingAnimation);
				}
		);
	}

	private void InstantiateTweetObject (Tweet t, Vector3 worldPosition)
	{
		GameObject tweetObj = Instantiate (_tweetPrefab);
		Bounds bounds = GetOrthographicBoundsForCamera (Camera.main);	
		
		// Set the parent and position of the Tweet
		tweetObj.transform.SetParent (_tweetContainer);

		float scale = UnityEngine.Random.Range (0.9f, 1.5f);
		tweetObj.transform.localScale = new Vector3 (scale, scale, 1.0f);

		// Get a position on a circular path around the world position and clamp to scene
		Vector3 tweetSpriteExtents = tweetObj.GetComponent<SpriteRenderer> ().sprite.bounds.extents;
		Vector3 position = GetRandomPointOnCircle (worldPosition,
		                                           UnityEngine.Random.Range (0.0f, Mathf.PI * 2.0f),
		                                           2.0f);
		
		tweetObj.transform.position =
			new Vector3 (Mathf.Clamp (position.x, bounds.min.x + tweetSpriteExtents.x, bounds.max.x - tweetSpriteExtents.x),
			             Mathf.Clamp (position.y, bounds.min.y + tweetSpriteExtents.y, bounds.max.y - tweetSpriteExtents.y),
			             0.0f);
			             
		// Set the Tweet content including the background sprite
		TweetController tweetController = tweetObj.GetComponent <TweetController> ();
		tweetController.Title.text = t.Author;
		tweetController.Description.text = t.Description;
		tweetController.PubDate.text = t.PubDate;
		tweetController.SpriteRenderer.sprite = _tweetBackgrounds[UnityEngine.Random.Range (0, _tweetBackgrounds.Count)];
	}

	private Vector3 GetRandomPointOnCircle (Vector3 center, float angle, float radius)
	{
		float x = Mathf.Cos (angle) * radius + center.x;
		float y = Mathf.Sin (angle) * radius + center.y;
		float z = center.z;

		return new Vector3 (x, y, z);
	}

	private Bounds GetOrthographicBoundsForCamera (Camera camera)
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = camera.orthographicSize * 2;
		Bounds bounds = new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

		return bounds;
	}
}
