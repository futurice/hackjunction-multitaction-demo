/*
 * @author Valentin Simonov / http://va.lent.in/
 */

using System;
using TouchScript.Gestures.Simple;
using UnityEngine;

namespace TouchScript.Behaviors
{
	/// <summary>
	/// Simple Component which transforms an object according to events from gestures.
	/// </summary>
	[AddComponentMenu("TouchScript/Behaviors/Transformer2DRB")]
	public class Transformer2DRB : MonoBehaviour
	{
		#region Public properties
		
		/// <summary>Max movement speed.</summary>
		public float Speed = 10f;
		
		/// <summary>Controls pan speed.</summary>
		public float PanMultiplier = 1f;
		
		public bool AllowChangingFromOutside = true;
		
		#endregion
		
		#region Private variables
		
		private Vector3 localPositionToGo, localScaleToGo;
		private Quaternion localRotationToGo;
		
		// last* variables are needed to detect when Transform's properties were changed outside of this script
		private Vector3 lastLocalPosition, lastLocalScale;
		private Quaternion lastLocalRotation;

		private Rigidbody2D _rigidbody;

		[SerializeField]
		private float _velocityMultiplier = 50.0f;

		[SerializeField]
		private float _maxVelocity = 50.0f;

		[SerializeField]
		private float _maxScaleComponent = 3.0f;
		
		[SerializeField]
		private float _minScaleComponent = 0.1f;

		#endregion
		
		#region Unity methods
		
		private void Awake()
		{
			setDefaults();
			_rigidbody = GetComponent<Rigidbody2D> ();
		}
		
		private void OnEnable()
		{
			if (GetComponent<SimplePanGesture>() != null) GetComponent<SimplePanGesture>().Panned += panHandler;
			if (GetComponent<SimpleScaleGesture>() != null) GetComponent<SimpleScaleGesture>().Scaled += scaleHandler;
			if (GetComponent<SimpleRotateGesture>() != null) GetComponent<SimpleRotateGesture>().Rotated += rotateHandler;
		}
		
		private void OnDisable()
		{
			if (GetComponent<SimplePanGesture>() != null) GetComponent<SimplePanGesture>().Panned -= panHandler;
			if (GetComponent<SimpleScaleGesture>() != null) GetComponent<SimpleScaleGesture>().Scaled -= scaleHandler;
			if (GetComponent<SimpleRotateGesture>() != null) GetComponent<SimpleRotateGesture>().Rotated -= rotateHandler;
		}
		
		private void Update()
		{
			var fraction = Speed * Time.deltaTime;
			
			if (AllowChangingFromOutside)
			{
				// changed by someone else
				if (!Mathf.Approximately(transform.localPosition.x, lastLocalPosition.x))
					localPositionToGo.x = transform.localPosition.x;
				if (!Mathf.Approximately(transform.localPosition.y, lastLocalPosition.y))
					localPositionToGo.y = transform.localPosition.y;
				if (!Mathf.Approximately(transform.localPosition.z, lastLocalPosition.z))
					localPositionToGo.z = transform.localPosition.z;
			}

			lastLocalPosition = Vector3.Lerp(transform.localPosition, localPositionToGo, fraction);

			if (AllowChangingFromOutside)
			{
				// changed by someone else
				if (!Mathf.Approximately(transform.localScale.x, lastLocalScale.x))
					localScaleToGo.x = transform.localScale.x;
				if (!Mathf.Approximately(transform.localScale.y, lastLocalScale.y))
					localScaleToGo.y = transform.localScale.y;
				if (!Mathf.Approximately(transform.localScale.z, lastLocalScale.z))
					localScaleToGo.z = transform.localScale.z;
			}
			var newLocalScale = lastLocalScale = Vector3.Lerp(transform.localScale, localScaleToGo, fraction);

			// prevent recalculating colliders when no scale occurs
			if (newLocalScale != transform.localScale)
			{
				transform.localScale = new Vector3 (Mathf.Clamp (newLocalScale.x, _minScaleComponent, _maxScaleComponent),
				                                    Mathf.Clamp (newLocalScale.x, _minScaleComponent, _maxScaleComponent),
				                                    1.0f);
				_rigidbody.mass = _rigidbody.mass * transform.localScale.x;
			}
			
			if (AllowChangingFromOutside)
			{
				// changed by someone else
				if (transform.localRotation != lastLocalRotation) localRotationToGo = transform.localRotation;
			}

			transform.localRotation = lastLocalRotation = Quaternion.Lerp(transform.localRotation, localRotationToGo, fraction);
		}
		
		#endregion
		
		#region Private functions
		
		private void setDefaults()
		{
			localPositionToGo = lastLocalPosition = transform.localPosition;
			localRotationToGo = lastLocalRotation = transform.localRotation;
			localScaleToGo = lastLocalScale = transform.localScale;
		}
		
		#endregion
		
		#region Event handlers
		
		private void panHandler(object sender, EventArgs e)
		{
			var gesture = (SimplePanGesture)sender;		
			localPositionToGo += gesture.LocalDeltaPosition * PanMultiplier;

			if (float.IsNaN (gesture.ScreenPosition.x) || float.IsNaN (gesture.ScreenPosition.y))
			{
				return;
			}

			Vector2 velocity = new Vector2 (gesture.WorldDeltaPosition.x, gesture.WorldDeltaPosition.y) * _velocityMultiplier;
			_rigidbody.velocity = Vector2.ClampMagnitude (velocity, _maxVelocity);
		}
		
		private void rotateHandler(object sender, EventArgs e)
		{
			var gesture = (SimpleRotateGesture)sender;
			
			if (Math.Abs(gesture.DeltaRotation) > 0.01)
			{
				if (transform.parent == null)
				{
					localRotationToGo = Quaternion.AngleAxis(gesture.DeltaRotation, gesture.RotationAxis) * localRotationToGo;
				}
				else
				{
					localRotationToGo = Quaternion.AngleAxis(gesture.DeltaRotation, transform.parent.InverseTransformDirection(gesture.RotationAxis)) * localRotationToGo;
				}
			}
		}
		
		private void scaleHandler(object sender, EventArgs e)
		{
			var gesture = (SimpleScaleGesture)sender;
			
			if (Math.Abs(gesture.LocalDeltaScale - 1) > 0.00001)
			{
				localScaleToGo *= gesture.LocalDeltaScale;
			}
		}
		
		#endregion
	}
}