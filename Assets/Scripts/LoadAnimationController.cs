using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class LoadAnimationController : MonoBehaviour
{
	public Text 	loadingText 			= null;
	public float	loadingTextAnimDuration = 0.5f;
	public string 	startText 				= "Loading";
	public string 	endText					= "Loading...";

	void Start ()
	{
		// Start the loading text animation
		loadingText.text = startText;

		DOTween.To (
			() => loadingText.text,
			x => loadingText.text = x,
			endText,
			loadingTextAnimDuration)
			.SetLoops (-1, LoopType.Restart);
	}

}
