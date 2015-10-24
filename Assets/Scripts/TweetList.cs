using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot("rss")]
[XmlType("rss")]
public class TweetList
{
	public TweetList ()
	{
		Channel = new Channel ();
	}

	[XmlElement("channel")]
	public Channel Channel
	{
		get;
		protected set;
	}

	public List<Tweet> Tweets
	{
		get
		{
			return Channel.Tweets;
		}
	}
}

[XmlType("channel")]
public class Channel
{
	public Channel ()
	{
		Tweets = new List<Tweet> ();
	}

	[XmlElement("item")]
	public List<Tweet> Tweets 
	{
		get;
		set;
	}
}
