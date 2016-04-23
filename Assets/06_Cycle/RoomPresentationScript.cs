using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomPresentationScript : MonoBehaviour 
{
	[Tooltip("Speed of spinning, when presentation slides change.")]
	public float speedMultiplier = 0.5f;
	public float maxSpinSpeed = 5.0f;

	//Vector3 newRotation;

	// reference to the gesture listener
	private RoomGestureListener gestureListener;
	

	void Start() 
	{

		// get the gestures listener
		gestureListener = RoomGestureListener.Instance;

	}
	
	void Update() 
	{
		// dont run Update() if there is no gesture listener
		if(!gestureListener)
			return;

		// get the lean factor and cap the value to maxSpinSpeed if to high
		float turnAngle = Mathf.Min(gestureListener.GetLeanFactor(),maxSpinSpeed);

		// rotate the Room Left
		if (gestureListener.IsLeaningLeft())
		{
			Vector3 newRotation = transform.rotation.eulerAngles;
			newRotation.y += turnAngle;
			
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), speedMultiplier * Time.deltaTime);
		}
		// rotate the Room Right
		else if (gestureListener.IsLeaningRight())
		{
			Vector3 newRotation = transform.rotation.eulerAngles;
			newRotation.y -= turnAngle;
			
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(newRotation), speedMultiplier * Time.deltaTime);

		}
	}
}
