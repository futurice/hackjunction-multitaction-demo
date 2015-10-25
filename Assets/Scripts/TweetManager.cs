using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections.Generic;

public class TweetManager : MonoBehaviour
{
	public string 		query 				= "#sten";
	public Transform	tweetContainer		= null;
	public GameObject 	tweetPrefab 		= null;
	public GameObject	spinnerPrefab		= null;
	public List<Sprite> tweetBackgrounds 	= new List<Sprite> ();

	// Use this for initialization
	void Start ()
	{
		// Make a buffered and throttled stream from the space key
		// presses
		IObservable<long> clickStream = Observable.EveryUpdate ()
			.Where (_ => Input.GetKeyDown (KeyCode.Space));
		
		clickStream.Buffer (clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
			.Where(x => x.Count >= 1)
			.Subscribe(x => GetTweets ());

		// Observe any tweets leaving the game field and destroy them
		this.OnTriggerEnter2DAsObservable ()
			.Subscribe (col => 
			            {
							Debug.Log ("Triggered!");
							if (col.gameObject.CompareTag ("Tweet"))
				    		{
								Destroy (col.gameObject);
							}
						});
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void GetTweets ()
	{
		// Instantiate a new loading animation prefab
		GameObject loadingAnimation = Instantiate (spinnerPrefab);

		Debug.Log (string.Format ("Searching for Tweets with query: {0}", query));
		QueryfeedAPI.SearchTweets (query)
			.Delay (TimeSpan.FromMilliseconds (500))
			.Subscribe (
				tl => 
				{
					Debug.Log (string.Format ("Retrieved {0} Tweets successfully", tl.Tweets.Count));
					Destroy (loadingAnimation);

					foreach (Tweet t in tl.Tweets)
					{
						Debug.Log (string.Format ("Author: {0}, Description: {1}", t.Author, t.Description));
						InstantiateTweetObject (t);
					}
				},
				ex =>
				{
					Debug.Log (string.Format ("Failed to retrieve Tweets for query {0}, ex: {1}", query, ex));
					Destroy (loadingAnimation);
				}
		);
	}

	void InstantiateTweetObject (Tweet t)
	{
		GameObject tweetObj = Instantiate (tweetPrefab);
		Bounds bounds = GetOrthographicBoundsForCamera (Camera.main);	
		
		// Set the parent and position of the Tweet
		tweetObj.transform.SetParent (tweetContainer);
		tweetObj.transform.position =
			new Vector3 (UnityEngine.Random.Range (bounds.min.x + 1.0f, bounds.max.x - 1.0f),
			             UnityEngine.Random.Range (bounds.min.y + 1.0f, bounds.max.y - 1.0f),
			             0.0f);
		
		// Set the Tweet content including the background sprite
		TweetController tweetController = tweetObj.GetComponent <TweetController> ();
		tweetController.title.text = t.Author;
		tweetController.description.text = t.Description;
		tweetController.pubDate.text = t.PubDate;
		tweetController.spriteRenderer.sprite = tweetBackgrounds[UnityEngine.Random.Range (0, tweetBackgrounds.Count)];
	}

	Bounds GetOrthographicBoundsForCamera (Camera camera)
	{
		float screenAspect = (float)Screen.width / (float)Screen.height;
		float cameraHeight = camera.orthographicSize * 2;
		Bounds bounds = new Bounds(
			camera.transform.position,
			new Vector3(cameraHeight * screenAspect, cameraHeight, 0));

		return bounds;
	}
}
