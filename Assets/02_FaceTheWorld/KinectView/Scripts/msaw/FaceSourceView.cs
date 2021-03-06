﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;
using Windows.Kinect;
using UnityEngine.Networking;

using System.IO;

public class FaceSourceView : NetworkBehaviour
{

	public GameObject ClientCam;


	private int MaximumFaceImages = 120;

	public GameObject FaceMultiSourceManager;
    private FaceMultiSourceManager _MultiManager;

	private int FaceImageNumber = 1;
	private int FaceFrameSize = 300;

	public Material BoneMaterial;

	public GameObject _MonitorSource;
	private GameObject _Monitor;
	private MonitorState _MonitorState;
	public GameObject _LookAtTarget;
	private Vector3 TargetPosition;
	private GameObject[] _MonitorList;// = new GameObject[144];

	private ulong TrackedBody;

	public GameObject Rig;
//del	private int RigSlot_X = 0;
//del	private int ReigSlot_Y = 0;
//del	private int RigWidth = 15;
	private int RigSlot;

	
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


    void Start ()
    {
		//NetworkServer.Spawn(ClientCam);
		_MonitorList = new GameObject[Rig.transform.childCount];
		CreateMonitors();
    }

	void CreateMonitors(){
		Debug.Log(Rig.transform.childCount);
		for (int slot = 0; slot < Rig.transform.childCount; slot++){
			_Monitor = (GameObject)Instantiate(_MonitorSource, new Vector3(0, 0, 0), Quaternion.identity);
			_Monitor.transform.parent = Rig.transform.GetChild(slot).transform;
			_Monitor.transform.position = Rig.transform.GetChild(slot).transform.position;
			_Monitor.transform.rotation = Rig.transform.GetChild(slot).transform.rotation;
			_MonitorList[slot] = _Monitor;
			_Monitor.GetComponent<FaceTextureAnimation>().RigSlot = slot;
			NetworkServer.Spawn(_Monitor);

		}
		RigSlot = 0;//Random.Range(0,Rig.transform.childCount);
		_Monitor = _MonitorList[RigSlot];
		iTweenPath.ChangePath("Path_1",0,Rig.transform.GetChild(RigSlot).transform.position);

	}

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

		if (_Monitor == null)
		{
			return;
		}
		
		_MonitorState = _Monitor.GetComponent<MonitorState>();
		if (_MonitorState == null)
		{
			return;
		}

		// EXPERIMANTAL depth data imlementation

//		ushort[] depthData = _MultiManager.GetDepthData();
//		if (depthData == null)
//		{
//			return;
//		}

		//

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
					_MonitorState._Behavior = MonitorState.AI.Awake;
					print ("got tracking so awakening....");
				}
				
				 RefreshBodyObject(body, _Bodies[body.TrackingId]); //msaw edit the next lines
				//RefreshBodyObject(body, _Bodies[TrackedBody]);
				//msaw end
				//SET FACE IS CAUSING AN ERROR SOMETIMES

				SetFace(body, _Bodies[body.TrackingId]); // msaw edit the next lines
				//SetFace(body, _Bodies[TrackedBody]);
				// msaw end
				SetMovingTarget(body);
				//break; // not shure if break helps to only get one body
			}
		}
		//end body


		// if we captured all faceframes play the animation
		//int lastIndex = FaceFramesArray.Length-1;
		//if (FaceFramesArray[lastIndex]!= null){
			//AnimateFaceFrame();
		//}
		doMonitorBehavior();
	}
	//------------
	public void MonitorEngage(){
		print ("setting Engage");
		_MonitorState._Behavior = MonitorState.AI.Engage;
	}

	public void MonitorShowcase(){
		print ("setting Showcase");
		_Monitor.transform.rotation = Rig.transform.GetChild(RigSlot).transform.rotation;
		_MonitorState._Behavior = MonitorState.AI.Showcase;
		/// <summary>
		/// select a new random monitor 
		/// childCount-2 is set because the index starts at 0 and
		/// to enshure that the last index won't be picked to permanently change 
		/// the searching direction PickEmptyMonitorBackwards inside of PickEmptyMonitor()
		/// random range begins at 1 because 0 will be started with anyways 
		/// and it will break out of the serching in PickEmptyMonitor() when 0 is reached in backwards search direction
		/// </summary>
		RigSlot += 1; // = Random.Range(1,Rig.transform.childCount-2);
		if (RigSlot == Rig.transform.childCount){RigSlot=0;}
		PickEmptyMonitor();
		PickEmptyMonitorBackwards = false;
		/// <summary>
		/// set the starting point of the awakening animation path to new Rig slot position
		/// </summary>
		iTweenPath.ChangePath("Path_1",0,Rig.transform.GetChild(RigSlot).transform.position);
		/// <summary>
		/// destroy the body by restarting the sensor
		/// </summary>
		TrackedBody = 0; //msaw test
		_MultiManager.RestartSensor();
	}


	bool PickEmptyMonitorBackwards = false;
	public void PickEmptyMonitor(){
		_Monitor = _MonitorList[RigSlot];
		_MonitorState = _Monitor.GetComponent<MonitorState>();
		while (_MonitorState._Display == MonitorState.Show.FaceAnimation){
			if (RigSlot == 0){
				Debug.LogError ("All monitors are taken breaking at Monitor_"+RigSlot);
				// we need an end condition here
				break;
			}
			if (RigSlot == Rig.transform.childCount-1){
				PickEmptyMonitorBackwards = true;
				Debug.LogError ("Changing Slot direction at Monitor_"+RigSlot);
			}
			if (PickEmptyMonitorBackwards){
				RigSlot -= 1;
			}else{
				RigSlot += 1;
			}
			Debug.LogError ("trying to pick Monitor_"+RigSlot);
			PickEmptyMonitor();
		}
	}

	public Transform[] waypoints;
	private void doMonitorBehavior(){

		switch (_MonitorState._Behavior) {

		case MonitorState.AI.SetToSleep:
			print ("monitor sleeping");
			_MonitorState._Behavior = MonitorState.AI.Sleeping;
			break;

		case MonitorState.AI.Sleeping:
			break;

		case MonitorState.AI.Awake:
			//Transform drone = _Monitor.transform.GetChild(0);
			//drone.transform.Rotate(Vector3.up,180,Space.Self);
			iTween.MoveTo(_Monitor, iTween.Hash(
				//"path", iTweenPath.GetPath(Rig.GetComponent<iTweenPath>().pathName), 
				"path", iTweenPath.GetPath("Path_1"), 
				"speed", 300, 
				//"looptype" , "pingPong",
				"orientToPath",true, 
				"lookTime", 0.1, 
				"easetype", iTween.EaseType.easeInExpo,
				"onComplete", "MonitorEngage",
				"onCompleteTarget", gameObject
				));
			_MonitorState._Behavior = MonitorState.AI.Idle;
			break;

		case MonitorState.AI.Idle:
			//_Monitor.transform.GetChild(0).transform.LookAt (_LookAtTarget.transform.position);
			break;

		case MonitorState.AI.Engage:
			Vector3 target = GetMovingTarget ();

			//limit the bounds of tracking the target
			//Might replace Taht If sentences with Min Max Math sentences 
			if (target.z > 40.0F){target.z = 40.0F;}
			if (target.z < 2.0F){target.z = 2.0F;}
			if (target.x > 10.0F + (40 - target.z) * 0.132F){target.x = 5.0F + (40 - target.z) * 0.132F;}
			if (target.x < -10.0F - (40 - target.z) * 0.132F){target.x = -5.0F - (40 - target.z) * 0.132F;}
			float heightmod = -30;//*10;//((40.0F - target.z) *0.5F)-30;
			Vector3 heightAdjustment = new Vector3 (0, heightmod, 0);
			//Debug.Log(target);
			// apply the kinekt depth values to the 3D space drone
			// but reverse the x direction so it will always go on the opposite side of the shadow
			Vector3 targetPosition = new Vector3(target.x *2, target.y + heightAdjustment.y, 50.0F - target.z * 5.0F);
			//Debug.Log(targetPosition);
			Vector3 monitorCurrentPosition = _Monitor.transform.position;
			float moveSpeed = 0.05F;
			_Monitor.transform.position = Vector3.Lerp (monitorCurrentPosition, targetPosition, moveSpeed);
			// hovering animation			
			float amplitude = 1F;
			float speed = 2F;
			float tempVal = target.y + heightAdjustment.y;
			Vector3 tempPos;
			tempPos.y = tempVal + amplitude * Mathf.Sin(speed * Time.time);
			_Monitor.transform.position = new Vector3(_Monitor.transform.position.x,tempPos.y,_Monitor.transform.position.z);
			// make the monitor face the sphere target for nice rotations
			_Monitor.transform.LookAt (_LookAtTarget.transform.position);
			break;

		case MonitorState.AI.Mimic:
			break;

		case MonitorState.AI.Return:
			//iTweenPath.ChangePath("Path_1",0,_Monitor.transform.position);
			iTween.MoveTo(_Monitor, iTween.Hash(
				//"path", iTweenPath.GetPath(Rig.GetComponent<iTweenPath>().pathName), 
				"path", iTweenPath.GetPathReversed("Path_1"), 
				"speed", 300, 
				//"looptype" , "pingPong",
				"orientToPath",true, 
				"lookTime", 0.1, 
				"easetype", iTween.EaseType.linear,
				"onComplete", "MonitorShowcase",
				"onCompleteTarget", gameObject
				));
			_MonitorState._Behavior = MonitorState.AI.Idle;
			break;

		case MonitorState.AI.Showcase:
			//_MonitorState._Behavior = MonitorState.AI.Idle;
			break;

		default:
			Debug.LogError ("The Monitor is missing a Behavior");
			break;
		}
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
	/// this function alignes the depth and color data
	/// tracks the head joint
	/// and cuts out a texture scaled by the distance of the person,
	/// so it will only contain the head
	/// than it saves the files to disk
	/// and sets the loading state for the monitor
	/// </summary>
	/// <param name="body">Body.</param>
	/// <param name="bodyObject">Body object.</param>
	public void SetFace(Kinect.Body body, GameObject bodyObject){
		Kinect.Joint headJoint = body.Joints[Kinect.JointType.Head];
		if (headJoint.TrackingState == TrackingState.Tracked)
		{
			/// <summary>
			/// get the color image coordinates for the head joint
			/// </summary>
			ColorSpacePoint ColorSpaceHead = 
				KinectSensor.GetDefault().CoordinateMapper.MapCameraPointToColorSpace(headJoint.Position);

			/// <summary>
			/// get the depth image coordinates for the head joint
			/// </summary>
			DepthSpacePoint depthPoint = 
				KinectSensor.GetDefault().CoordinateMapper.MapCameraPointToDepthSpace(headJoint.Position);

			/// <summary>
			/// use the x & y coordinates to locate the depth data
			/// </summary> 
			FrameDescription depthDesc = KinectSensor.GetDefault().DepthFrameSource.FrameDescription;

			int depthX = (int)Mathf.Floor(depthPoint.X + 0.5F);
			int depthY = (int)Mathf.Floor(depthPoint.Y + 0.5F);
			int depthIndex = (depthY * depthDesc.Width) + depthX;
		
			//ushort depth = _depthData[depthIndex];

			/// <remarks>
			/// SET FACE IS COUSING AN ERROR SOMETIMES 
			///  
			/// IndexOutOfRangeException: Array index is out of range.
			/// FaceSourceView.SetFace (Windows.Kinect.Body body, UnityEngine.GameObject bodyObject) (at Assets/KinectView/Scripts/msaw/FaceSourceView.cs:201)
			/// FaceSourceView.Update () (at Assets/KinectView/Scripts/msaw/FaceSourceView.cs:143)
			/// 
			/// this error occures when you move super close to the canvas so i guess the head gets out of frame on top
			/// or the kinect looses depth info when to close ...
			/// </remarks>
			ushort depth = _MultiManager.GetDepthData()[depthIndex];

			int imageSize = (int)(FaceFrameSize / CalculatePixelWidth(depthDesc, depth));
			int x = (int)(Mathf.Floor(ColorSpaceHead.X + 0.5F) - (imageSize / 2));
			int y = (int)(Mathf.Floor(ColorSpaceHead.Y + 0.5F) - (imageSize / 2));
		
//			_width = (float)imageSize;
//			_height = (float)imageSize;

			if (x  > 0 && y > 0 && imageSize > 0 && x < _MultiManager.ColorWidth - imageSize && y < _MultiManager.ColorHeight - imageSize) {
				Texture2D colorSource = _MultiManager.GetColorTexture();
				/// <remarks>
				/// ERROR
				/// 	Texture rectangle is out of bounds (867 + 263 > 1080)
				/// 	UnityEngine.Texture2D:GetPixels(Int32, Int32, Int32, Int32)
				/// 	FaceSourceView:SetFace(Body, GameObject) (at Assets/KinectView/Scripts/msaw/FaceSourceView.cs:224)
				/// 	FaceSourceView:Update() (at Assets/KinectView/Scripts/msaw/FaceSourceView.cs:145)
				/// 
				/// tried to fix it with better if condition but did not help
				/// && x <= _MultiManager.ColorWidth -(imageSize / 2) && y <= _MultiManager.ColorHeight -(imageSize / 2)
				/// 
				/// Texture rectangle is out of bounds (1859 + 72 > 1920)
				/// tried to fix it now by removing the division on the imageSize 
				/// from -(imageSize / 2) to -(imageSize)
				/// </remarks>
				/// 1920x1080 color frame
				/// 512x424 depth and infrared
				Color[] pix = colorSource.GetPixels(x, y, imageSize, imageSize);
				Texture2D destTex = new Texture2D(imageSize, imageSize);
				destTex.SetPixels(pix);
				destTex.Apply ();
				//gameObject.GetComponent<Renderer>().material.mainTexture = destTex;

				/// <summary>
				/// saving the destTex texture single frame to disk
				/// </summary>
				if (FaceImageNumber <= MaximumFaceImages){
					if (_MonitorState._Display == MonitorState.Show.Nothing){
						SaveTextureToFile(destTex, FaceImageNumber);
						FaceImageNumber += 1;
					}
				}else{
					//after images are saved tell the server to set the loading state
					_MonitorState._Display = MonitorState.Show.StartLoading;
					FaceImageNumber = 1;
					//_MonitorState._Display = MonitorState.Show.FaceAnimation;
				}
			}
		}
	}

	/// <summary>
	/// Saves the texture to file.
	/// </summary>
	/// <param name="texture">Texture.</param>
	/// <param name="imageNumber">Image number.</param>
	void SaveTextureToFile( Texture2D texture ,int imageNumber)
	{
		// to store image
		//print(Application.persistentDataPath);
		byte[] bytes = texture.EncodeToPNG ();
		//string path = System.IO.Path.Combine(Application.persistentDataPath, fileName + ".png");
		string path = System.IO.Path.Combine("Z:\\Faces\\face ("+RigSlot+")", "image_"+ imageNumber + ".png");
		File.WriteAllBytes (path, bytes);
		//print(path);
		}

	/// <summary>
	/// Calculates the width of the Depth Frame pixels.
	/// </summary>
	/// <remarks>
	/// not shure if its color pixels or depth!!!!!!!
	/// </remarks>
	/// <returns>The pixel width.</returns>
	/// <param name="description">Description.</param>
	/// <param name="depth">Depth.</param>
	private double CalculatePixelWidth(FrameDescription description, ushort depth)
	{
		// measure the size of the pixel
		float hFov = description.HorizontalFieldOfView / 2;
		float numPixels = description.Width / 2;
		
	/* soh-cah-TOA
     * 
     * TOA = tan(0) = O / A
     *   T = tan( (horizontal FOV / 2) in radians )
     *   O = (frame width / 2) in mm
     *   A = depth in mm
     *   
     *   O = A * T
     */
		
		double T = Mathf.Tan((Mathf.PI * 180) / hFov);
		double pixelWidth = T * depth;
		
		return pixelWidth / numPixels;
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
			lr.SetWidth(0.05f, 0.05f);
			
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
		return new Vector3(joint.Position.X * 25 , joint.Position.Y * 25, joint.Position.Z *25 -50-500);
	}
}
