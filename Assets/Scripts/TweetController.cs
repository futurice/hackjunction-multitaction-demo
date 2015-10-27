using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TweetController : MonoBehaviour
{
	[SerializeField]
	private Text 			_title			= null;

	[SerializeField]
	private Text 			_description	= null;

	[SerializeField]
	private Text 			_pubDate		= null;

	[SerializeField]
	private SpriteRenderer 	_spriteRenderer = null;

	public Text Title
	{
		get
		{
			return _title;
		}
	}
	
	public Text Description
	{
		get
		{
			return _description;
		}
	}

	public Text	PubDate
	{
		get
		{
			return _pubDate;
		}
	}

	public SpriteRenderer SpriteRenderer
	{
		get
		{
			return _spriteRenderer;
		}
	}

	void Start ()
	{
	}
	
	void Update ()
	{
	}
}
