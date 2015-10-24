using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;
using System.Xml.Serialization;
using System.IO;

public class QueryfeedAPI 
{
	public static string GetQueryURL (string query)
	{
		string escapedQuery = Uri.EscapeDataString (query);
		return string.Format ("https://queryfeed.net/twitter?q={0}", escapedQuery);
	}

	public static IObservable<TweetList> SearchTweets (string query)
	{
		string url = GetQueryURL (query);

		return ObservableWWW.Get (url)
			.Select (
				xml => {
					try
					{
						Debug.Log (string.Format ("Retrieved Tweets for query {0} successfully", query));
						XmlSerializer ser = new XmlSerializer(typeof(TweetList));
						
						// Declare an object variable of the type to be deserialized.					
						using (Stream reader = GenerateStreamFromString (xml))
						{
							// Call the Deserialize method to restore the object's state.
							return (ser.Deserialize (reader) as TweetList);    
						}
					}
					catch (Exception e)
					{
						Debug.LogError (string.Format ("Failed to deserialize Tweets from query: {0}, ex: {1}", query, e));
						return new TweetList ();
					}
				}
			);
	}

	private static Stream GenerateStreamFromString (string s)
	{
		MemoryStream stream = new MemoryStream();
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(s);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}
}
