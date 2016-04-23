using UnityEngine;
using System.Collections;
using System;
//using Windows.Kinect;

public class RoomGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
	[Tooltip("GUI-Text to display gesture-listener messages and gesture information.")]
	public GUIText gestureInfo;

	// singleton instance of the class
	private static RoomGestureListener instance = null;
	
	// internal variables to track if progress message has been displayed
	private bool progressDisplayed;
	private float progressGestureTime;

	// whether the needed gesture has been detected or not
	private bool wheel;
	private float wheelAngle = 0f;

	private bool leanLeft;
	private bool leanRight;
	private float leanFactor = 0f;


	private bool raiseHand = false;


	/// <summary>
	/// Gets the singleton RoomGestureListener instance.
	/// </summary>
	/// <value>The RoomGestureListener instance.</value>
	public static RoomGestureListener Instance
	{
		get
		{
			return instance;
		}
	}
	/// <summary>
	/// Determines whether the user is leaning left.
	/// </summary>
	/// <returns><c>true</c> if the user is leaning left; otherwise, <c>false</c>.</returns>
	public bool IsLeaningLeft()
	{
		return leanLeft;
	}
	
	/// <summary>
	/// Determines whether the user is leaning right.
	/// </summary>
	/// <returns><c>true</c> if the user is leaning right; otherwise, <c>false</c>.</returns>
	public bool IsLeaningRight()
	{
		return leanRight;
	}
	
	/// <summary>
	/// Gets the lean factor.
	/// </summary>
	/// <returns>The lean factor.</returns>
	public float GetLeanFactor()
	{
		return leanFactor;
	}
	

	/// <summary>
	/// Determines whether the user is turning wheel.
	/// </summary>
	/// <returns><c>true</c> if the user is turning wheel; otherwise, <c>false</c>.</returns>
	public bool IsTurningWheel()
	{
		return wheel;
	}

	/// <summary>
	/// Gets the wheel angle.
	/// </summary>
	/// <returns>The wheel angle.</returns>
	public float GetWheelAngle()
	{
		return wheelAngle;
	}
	
	/// <summary>
	/// Invoked when a new user is detected. Here you can start gesture tracking by invoking KinectManager.DetectGesture()-function.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	public void UserDetected(long userId, int userIndex)
	{
		// the gestures are allowed for the primary user only
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return;
		
		// detect these user specific gestures
		manager.DetectGesture(userId, KinectGestures.Gestures.LeanLeft);
		manager.DetectGesture(userId, KinectGestures.Gestures.LeanRight);

		manager.DetectGesture(userId, KinectGestures.Gestures.Wheel);
				
		if(gestureInfo != null)
		{
			gestureInfo.GetComponent<GUIText>().text = "Lean-left, Lean-right or wheel to rotate the Room. Raise hand to reset it.";
		}
	}

	/// <summary>
	/// Invoked when a user gets lost. All tracked gestures for this user are cleared automatically.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	public void UserLost(long userId, int userIndex)
	{
		// the gestures are allowed for the primary user only
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return;
		
		if(gestureInfo != null)
		{
			gestureInfo.GetComponent<GUIText>().text = string.Empty;
		}
	}

	/// <summary>
	/// Invoked when a gesture is in progress.
	/// </summary>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	/// <param name="gesture">Gesture type</param>
	/// <param name="progress">Gesture progress [0..1]</param>
	/// <param name="joint">Joint type</param>
	/// <param name="screenPos">Normalized viewport position</param>
	public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
	{
		// the gestures are allowed for the primary user only
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return;

		switch (gesture){
		case KinectGestures.Gestures.LeanLeft:
			if (progress > 0.5f){
				leanLeft = true;
				leanFactor = screenPos.z;
				
				if(gestureInfo != null)
				{
					string sGestureText = string.Format ("{0} - {1:F0} degrees - {2}", "LeanLeft", screenPos.z, leanLeft ? "leanLeft":"set to false");
					gestureInfo.GetComponent<GUIText>().text = sGestureText;
					
					progressDisplayed = true;
					progressGestureTime = Time.realtimeSinceStartup;
				}
			}
			else
			{
				leanLeft = false;
			}
			return;
		case KinectGestures.Gestures.LeanRight:
			if (progress > 0.5f){
				leanRight = true;
				leanFactor = screenPos.z;
			
				if(gestureInfo != null)
				{
					string sGestureText = string.Format ("{0} - {1:F0} degrees - {2}", "LeanRight", screenPos.z , leanRight ? "leanRight":"set to false");
					gestureInfo.GetComponent<GUIText>().text = sGestureText;
					
					progressDisplayed = true;
					progressGestureTime = Time.realtimeSinceStartup;
				}
			}
			else
			{
				leanRight = false;
			}
			return;
		default:
			leanLeft = false;
			leanRight = false;
			return;

		}

//		if(gesture == KinectGestures.Gestures.Wheel)
//		{
//			if(progress > 0.5f)
//			{
//				wheel = true;
//				wheelAngle = screenPos.z;
//				
//				if(gestureInfo != null)
//				{
//					string sGestureText = string.Format ("Wheel angle: {0:F0} degrees", screenPos.z);
//					gestureInfo.GetComponent<GUIText>().text = sGestureText;
//					
//					progressDisplayed = true;
//					progressGestureTime = Time.realtimeSinceStartup;
//				}
//			}
//			else
//			{
//				wheel = false;
//			}
//		}
	}

	/// <summary>
	/// Invoked if a gesture is completed.
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	/// <param name="gesture">Gesture type</param>
	/// <param name="joint">Joint type</param>
	/// <param name="screenPos">Normalized viewport position</param>
	public bool GestureCompleted (long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint, Vector3 screenPos)
	{
		if(gesture == KinectGestures.Gestures.RaiseLeftHand)
			raiseHand = true;
		else if(gesture == KinectGestures.Gestures.RaiseRightHand)
			raiseHand = true;

		return true;
	}

	/// <summary>
	/// Invoked if a gesture is cancelled.
	/// </summary>
	/// <returns>true</returns>
	/// <c>false</c>
	/// <param name="userId">User ID</param>
	/// <param name="userIndex">User index</param>
	/// <param name="gesture">Gesture type</param>
	/// <param name="joint">Joint type</param>
	public bool GestureCancelled (long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint)
	{
		// the gestures are allowed for the primary user only
		KinectManager manager = KinectManager.Instance;
		if(!manager || (userId != manager.GetPrimaryUserID()))
			return false;
		
		if(gesture == KinectGestures.Gestures.LeanLeft)
		{
			leanLeft = false;
		}
		else if(gesture == KinectGestures.Gestures.LeanRight)
		{
			leanRight = false;
		}
		else if(gesture == KinectGestures.Gestures.Wheel)
		{
			wheel = false;
		}
		
		if(gestureInfo != null && progressDisplayed)
		{
			progressDisplayed = false;
			gestureInfo.GetComponent<GUIText>().text = "Lean-left, Lean-right to rotate the Room.";
		}

		return true;
	}

	
	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		if(progressDisplayed && ((Time.realtimeSinceStartup - progressGestureTime) > 2f))
		{
			//leanLeft = false;
			//leanRight= false;
			progressDisplayed = false;
			gestureInfo.GetComponent<GUIText>().text = string.Empty;

			Debug.LogError("Forced progress to end.");
		}
	}

}
