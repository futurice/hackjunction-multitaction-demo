using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class EraserController : MonoBehaviour
{
	private void Start ()
	{
		// Subscribe to collision events with Tweets
		// Destroy tweet object on collision
		this.gameObject.OnTriggerEnter2DAsObservable ()
			.Where (collider2D => collider2D.tag == "Tweet")
			.Subscribe (collider2D => Destroy (collider2D.gameObject));
	}
	
	private void Update ()
	{
		// TODO: track the tag and move with the tag
	}
}
