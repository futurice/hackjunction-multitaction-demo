using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class LoadAnimationController : MonoBehaviour
{
	[SerializeField]
	private Text 	_loadingText 				= null;

	[SerializeField]
	private float	_loadingTextAnimDuration 	= 0.5f;

	[SerializeField]
	private string 	_startText 					= "Loading";

	[SerializeField]
	private string 	_endText					= "Loading...";

	void Start ()
	{
		// Start the loading text animation
		_loadingText.text = _startText;

		DOTween.To (
			() => _loadingText.text,
			x => _loadingText.text = x,
			_endText,
			_loadingTextAnimDuration)
			.SetLoops (-1, LoopType.Restart);
	}

}
