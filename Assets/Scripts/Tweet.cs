using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

[XmlType("item")]
public class Tweet
{
	private string 		_title;
	private string 		_author;
	private string 		_description;
	private DateTime 	_pubDate;

	public Tweet ()
	{
	}

	public Tweet (string link, string title, string author, string pubDate, string description)
	{
		Link = link;
		Title =  title;
		Author = author;
		PubDate = pubDate;
		Description = description;
	}

	[XmlElement("link")]
	public string Link 
	{
		get;
		set;
	}

	[XmlElement("title")]
	public string Title
	{
		get
		{
			return _title;
		}

		set
		{
			_title = RemoveHTMLFromString (value);
		}
	}

	[XmlElement("author")]
	public string Author
	{
		get
		{
			return _author;
		}

		set
		{
			_author = RemoveHTMLFromString (value);
		}
	}

	[XmlElement("pubDate")]
	public string PubDate
	{
		get
		{
			return _pubDate.ToShortDateString ();
		}

		set
		{
			_pubDate = DateTime.Parse (value);
		}
	}

	[XmlElement("description")]
	public string Description
	{
		get
		{
			return _description;
		}

		set
		{
			_description = RemoveHTMLFromString (value);
		}
	}

	protected static string RemoveHTMLFromString (string str)
	{
		if (string.IsNullOrEmpty (str))
		{
			return null;
		}

		return Regex.Replace(str, @"<[^>]+>|&nbsp;", "").Trim();
	}
}
