using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using Windows.Kinect;
using UnityEngine.Networking;

public class BodySource : NetworkBehaviour
{
	public GameObject FaceMultiSourceManager;
	private FaceMultiSourceManager _MultiManager;

	public Material BoneMaterial;

	private Vector3 TargetPosition;

	private ulong TrackedBody;

	private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();

	/// <summary>
	/// The bone map.
	/// </summary>
	private Dictionary<Kinect.JointType, Kinect.JointType> _BoneMap = new Dictionary<Kinect.JointType, Kinect.JointType>()
	{
		{ Kinect.JointType.FootLeft, Kinect.JointType.AnkleLeft },
		{ Kinect.JointType.AnkleLeft, Kinect.JointType.KneeLeft },
		{ Kinect.JointType.KneeLeft, Kinect.JointType.HipLeft },
		{ Kinect.JointType.HipLeft, Kinect.JointType.SpineBase },
		
		{ Kinect.JointType.FootRight, Kinect.JointType.AnkleRight },
		{ Kinect.JointType.AnkleRight, Kinect.JointType.KneeRight },
		{ Kinect.JointType.KneeRight, Kinect.JointType.HipRight },
		{ Kinect.JointType.HipRight, Kinect.JointType.SpineBase },
		
		{ Kinect.JointType.HandTipLeft, Kinect.JointType.HandLeft },
		{ Kinect.JointType.ThumbLeft, Kinect.JointType.HandLeft },
		{ Kinect.JointType.HandLeft, Kinect.JointType.WristLeft },
		{ Kinect.JointType.WristLeft, Kinect.JointType.ElbowLeft },
		{ Kinect.JointType.ElbowLeft, Kinect.JointType.ShoulderLeft },
		{ Kinect.JointType.ShoulderLeft, Kinect.JointType.SpineShoulder },
		
		{ Kinect.JointType.HandTipRight, Kinect.JointType.HandRight },
		{ Kinect.JointType.ThumbRight, Kinect.JointType.HandRight },
		{ Kinect.JointType.HandRight, Kinect.JointType.WristRight },
		{ Kinect.JointType.WristRight, Kinect.JointType.ElbowRight },
		{ Kinect.JointType.ElbowRight, Kinect.JointType.ShoulderRight },
		{ Kinect.JointType.ShoulderRight, Kinect.JointType.SpineShoulder },
		
		{ Kinect.JointType.SpineBase, Kinect.JointType.SpineMid },
		{ Kinect.JointType.SpineMid, Kinect.JointType.SpineShoulder },
		{ Kinect.JointType.SpineShoulder, Kinect.JointType.Neck },
		{ Kinect.JointType.Neck, Kinect.JointType.Head },
	};

	void Update()
	{

		if (FaceMultiSourceManager == null)
		{
			return;
		}
		
		_MultiManager = FaceMultiSourceManager.GetComponent<FaceMultiSourceManager>();
		if (_MultiManager == null)
		{
			return;
		}

		//begin body
		Kinect.Body[] BodydataArray = _MultiManager.GetBodyData ();
		if (BodydataArray == null)
		{
			return;
		}
		
		List<ulong> trackedIds = new List<ulong>();
		foreach(var body in BodydataArray)
		{
			if (body == null)
			{
				continue;
			}
			
			if(body.IsTracked)
			{
				trackedIds.Add (body.TrackingId);// msaw edit the next lines
				//if (TrackedBody == 0){
				//	TrackedBody = body.TrackingId;
				//Debug.LogError(body.TrackingId);
				//}
				//msaw end
			}
		}

		List<ulong> knownIds = new List<ulong>(_Bodies.Keys);
		
		// First delete untracked bodies
		foreach(ulong trackingId in knownIds)
		{
			if(!trackedIds.Contains(trackingId))// msaw edit the next lines
				//if(TrackedBody != trackingId)
				//msaw end
			{
				Destroy(_Bodies[trackingId]);
				_Bodies.Remove(trackingId);
			}
		} 
		foreach(var body in BodydataArray)
		{
			if (body == null)
			{
				continue;
			}
			
			if(body.IsTracked)
			{
				if(!_Bodies.ContainsKey(body.TrackingId)) // msaw edit the next lines
					//if(!_Bodies.ContainsKey(TrackedBody))
					//msaw end
				{
					_Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId); //msaw edit the next lines
					//_Bodies[TrackedBody] = CreateBodyObject(TrackedBody);
					// msaw end
					print ("got tracking so awakening....");
				}
				
				RefreshBodyObject(body, _Bodies[body.TrackingId]); //msaw edit the next lines
				//RefreshBodyObject(body, _Bodies[TrackedBody]);
				//msaw end
				//SET FACE IS CAUSING AN ERROR SOMETIMES
				
				//SetFace(body, _Bodies[body.TrackingId]); // msaw edit the next lines
				//SetFace(body, _Bodies[TrackedBody]);
				// msaw end
				SetMovingTarget(body);
				//break; // not shure if break helps to only get one body
			}
		}
		//end body
	}
	// set the body head as target for tracking
	private void SetMovingTarget(Kinect.Body body){
		Kinect.Joint targetJoint = body.Joints[Kinect.JointType.Head];
		if (targetJoint.TrackingState == TrackingState.Tracked) {
			TargetPosition = GetVector3FromJoint (targetJoint);
		}
	}
	/// <summary>
	/// Gets the moving target (Head joint) for tracking.
	/// </summary>
	/// <returns>The moving target.</returns>
	public Vector3 GetMovingTarget(){
		return TargetPosition;
	}
	/// <summary>
	/// Creates the body object.
	/// </summary>
	/// <returns>The body object.</returns>
	/// <param name="id">Identifier.</param>
	private GameObject CreateBodyObject(ulong id)
	{
		GameObject body = new GameObject("Body:" + id);
		
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
		{
			
			GameObject jointObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
			
			LineRenderer lr = jointObj.AddComponent<LineRenderer>();
			lr.SetVertexCount(2);
			lr.material = BoneMaterial;
			lr.SetWidth(2f, 2f);
			
			jointObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			jointObj.name = jt.ToString();
			jointObj.transform.parent = body.transform;
			
		}
		
		return body;
	}
	/// <summary>
	/// Refreshs the body object.
	/// And dose the line rendering of Joints.
	/// </summary>
	/// <param name="body">Body.</param>
	/// <param name="bodyObject">Body object.</param>
	private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
	{
		for (Kinect.JointType jt = Kinect.JointType.SpineBase; jt <= Kinect.JointType.ThumbRight; jt++)
		{
			Kinect.Joint sourceJoint = body.Joints[jt];
			Kinect.Joint? targetJoint = null;
			
			if(_BoneMap.ContainsKey(jt))
			{
				targetJoint = body.Joints[_BoneMap[jt]];
			}
			
			Transform jointObj = bodyObject.transform.FindChild(jt.ToString());
			jointObj.localPosition = GetVector3FromJoint(sourceJoint);
			
			LineRenderer lr = jointObj.GetComponent<LineRenderer>();
			if(targetJoint.HasValue)
			{
				lr.SetPosition(0, jointObj.localPosition);
				lr.SetPosition(1, GetVector3FromJoint(targetJoint.Value));
				lr.SetColors(GetColorForState (sourceJoint.TrackingState), GetColorForState(targetJoint.Value.TrackingState));
			}
			else
			{
				lr.enabled = false;
			}
		}
	}
	/// <summary>
	/// Gets the state of the Bone color.
	/// </summary>
	/// <returns>The color for state.</returns>
	/// <param name="state">State.</param>
	private static Color GetColorForState(Kinect.TrackingState state)
	{
		switch (state)
		{
		case Kinect.TrackingState.Tracked:
			return Color.green;
			
		case Kinect.TrackingState.Inferred:
			return Color.red;
			
		default:
			return Color.black;
		}
	}
	/// <summary>
	/// Gets the vector3 from joint.
	/// </summary>
	/// <returns>The vector3 from joint.</returns>
	/// <param name="joint">Joint.</param>
	private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
	{
		//return new Vector3(joint.Position.X * 25 , joint.Position.Y * 25, joint.Position.Z *25 -50-500);
		return new Vector3(joint.Position.X * -75 , joint.Position.Y * 75+20, joint.Position.Z *-75 +500);
	}
}