using UnityEngine;
using System.Collections;
using UniRx;

public class Tester : MonoBehaviour
{
	public string query = "#sten";
	public GameObject tweetPrefab = null;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	void FixedUpdate ()
	{
		if (Input.GetKeyDown (KeyCode.Space))
		{
			GetTweets ();
		}
	}

	void GetTweets ()
	{
		Debug.Log (string.Format ("Searching for Tweets with query: {0}", query));
		QueryfeedAPI.SearchTweets (query).Subscribe (
			tl => 
			{
				Debug.Log (string.Format ("Retrieved {0} Tweets successfully", tl.Tweets.Count));
				foreach (Tweet t in tl.Tweets)
				{
					Debug.Log (string.Format ("Author: {0}, Description: {1}", t.Author, t.Description));

					GameObject tweetObj = Instantiate (tweetPrefab);
					TweetController tweetController = tweetObj.GetComponent <TweetController> ();
					tweetController.title.text = t.Author;
					tweetController.description.text = t.Description;
				}
			},
			ex =>
			{
				Debug.Log (string.Format ("Failed to retrieve Tweets for query {0}, ex: {1}", query, ex));
			}
		);
	}
}
