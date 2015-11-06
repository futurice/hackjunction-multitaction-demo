using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using TouchScript;
using System;

public interface IToolController
{
	void FollowTouch(ITouch touch);
}

public class EraserController : MonoBehaviour, IToolController
{
	private void Start ()
	{
		// Subscribe to collision events with Tweets
		// Destroy tweet object on collision
		this.gameObject.OnTriggerEnter2DAsObservable ()
			.Where (collider2D => collider2D.tag == "Tweet")
			.Subscribe (collider2D => Destroy (collider2D.gameObject));

	}

	private List<IDisposable> _disposables = new List<IDisposable>();
	
	private void Update ()
	{
		// TODO: track the tag and move with the tag
	}

	private void OnDestroy()
	{
		foreach (IDisposable disposable in _disposables)
		{
			disposable.Dispose();
		}
	}


	public void FollowTouch(ITouch touch)
	{
		var disposable = touch.ObserveEveryValueChanged(t => t.Position)
				.Subscribe( 
					newPos => 
					{
						Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector3(newPos.x, newPos.y, 0.0f));
						this.transform.position = new Vector3(pos.x, pos.y, 0.0f);
					}
				);

		_disposables.Add(disposable);
	}
}
