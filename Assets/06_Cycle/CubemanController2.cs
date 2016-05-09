/// <summary>
/// Cubeman controller2.
/// is an atempt to increase the left and right movement for the circular room navigation
/// </summary>


using UnityEngine;
//using Windows.Kinect;

using System;
using System.Collections;

public class CubemanController2 : MonoBehaviour 
{

	public Vector3 data = Vector3.zero;

	[Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
	public int playerIndex = 0;

	[Tooltip("Whether the cubeman is allowed to move vertically or not.")]
	public bool verticalMovement = true;

	[Tooltip("Whether the cubeman is facing the player or not.")]
	public bool mirroredMovement = false;

	[Tooltip("Rate at which the cubeman will move through the scene.")]
	public Vector3 moveRate = new Vector3(1f,1f,1f);

	//TODO: @merlin: Cubeman, set movement speed multiplier to increase the movement speed
	[Tooltip("Rate Multiplier at which the cubeman will move through the scene.")]
	public Vector3 increasedMovement = new Vector3(2f,1f,5f);

	//TODO: @merlin: Cubeman, set scale multiplier to increase size
	[Tooltip("scale the cubeman to fit the shadow sillouet.")]
	private Vector3 increasedScaling = new Vector3(8f,10f,1f);

	// Initial position and rotation of the transform
	[Tooltip("Position this transform is offseted by")]
	public Vector3 offsetNodePos = new Vector3(0f,0.6f,4.5f);
	protected Quaternion offsetNodeRot;

	[Tooltip("set the distance from the center of the circular room")]
	public float CenterOffset = 78f;




	//public GameObject debugText;
	
	public GameObject Hip_Center;
	public GameObject Spine;
	public GameObject Neck;
	public GameObject Head;
	public GameObject Shoulder_Left;
	public GameObject Elbow_Left;
	public GameObject Wrist_Left;
	public GameObject Hand_Left;
	public GameObject Shoulder_Right;
	public GameObject Elbow_Right;
	public GameObject Wrist_Right;
	public GameObject Hand_Right;
	public GameObject Hip_Left;
	public GameObject Knee_Left;
	public GameObject Ankle_Left;
	public GameObject Foot_Left;
	public GameObject Hip_Right;
	public GameObject Knee_Right;
	public GameObject Ankle_Right;
	public GameObject Foot_Right;
	public GameObject Spine_Shoulder;
    public GameObject Hand_Tip_Left;
    public GameObject Thumb_Left;
    public GameObject Hand_Tip_Right;
    public GameObject Thumb_Right;
	
	public LineRenderer skeletonLine;
	public LineRenderer debugLine;

	private GameObject[] bones;

	private LineRenderer lineTLeft;
	private LineRenderer lineTRight;
	private LineRenderer lineFLeft;
	private LineRenderer lineFRight;

	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Vector3 initialPosOffset = Vector3.zero;
	private Int64 initialPosUserID = 0;




	void Start () 
	{

		//store bones in a list for easier access
		bones = new GameObject[] {
			Hip_Center,
            Spine,
            Neck,
            Head,
            Shoulder_Left,
            Elbow_Left,
            Wrist_Left,
            Hand_Left,
            Shoulder_Right,
            Elbow_Right,
            Wrist_Right,
            Hand_Right,
            Hip_Left,
            Knee_Left,
            Ankle_Left,
            Foot_Left,
            Hip_Right,
            Knee_Right,
            Ankle_Right,
            Foot_Right,
            Spine_Shoulder,
            Hand_Tip_Left,
            Thumb_Left,
            Hand_Tip_Right,
            Thumb_Right
		};
		

		

		//TODO: @merlin: Cubeman, set the initial statring position
		initialPosition = new Vector3(0,0,0f);//Vector3.zero;// transform.position;
		initialRotation = transform.rotation;
		//transform.rotation = Quaternion.identity;
	}
	
	Vector3 LastKnownPos = Vector3.zero;
	void Update () 
	{
		KinectManager manager = KinectManager.Instance;
		
		// get 1st player
		Int64 userID = manager ? manager.GetUserIdByIndex(playerIndex) : 0;
		if(userID >= 1){
			LastKnownPos = transform.position;
		}
		if(userID <= 0)
		{
			initialPosUserID = 0;
			initialPosOffset = Vector3.zero+new Vector3(0,0,CenterOffset);

			// reset the pointman position and rotation
			//if(transform.position != initialPosition)
			if(transform.position != LastKnownPos)
			{
				transform.position = LastKnownPos;//initialPosition+new Vector3(0f,0f,CenterOffset); //+ offsetNodePos;
			}
			
			if(transform.rotation != initialRotation)
			{
				//transform.rotation = initialRotation;
			}

			for(int i = 0; i < bones.Length; i++) 
			{
				bones[i].gameObject.SetActive(true);
				//TODO: @merlin ****** i destroied the cubemen bone positioning somehow
				//bones[i].transform.localPosition = Vector3.zero+new Vector3(0,0,-CenterOffset);
				//bones[i].transform.localRotation = Quaternion.identity;

			}

			return;
		}
		
		// set the position in space
		//TODO: @merlin: Cubeman, figure out a nice value for X movement
		Vector3 posPointMan = offsetNodePos-manager.GetUserPosition(userID);
//			Vector3.Scale (
//				Vector3.Scale(
//					offsetNodePos-manager.GetUserPosition(userID),
//		    		new Vector3(4f,1f,3f)
//				)  +new Vector3(0f,0f,9f),
//			increasedMovement
//			);
		data = posPointMan+new Vector3(0,0,CenterOffset);
		Vector3 posPointManMP = new Vector3(posPointMan.x*increasedMovement.x, posPointMan.y*increasedMovement.y, posPointMan.z*-increasedMovement.z);
		
		// store the initial position
		if(initialPosUserID != userID)
		{
			initialPosUserID = userID;
			//initialPosOffset = transform.position - (verticalMovement ? posPointMan * moveRate : new Vector3(posPointMan.x, 0, posPointMan.z) * moveRate);
			initialPosOffset = posPointMan+new Vector3(0,0,CenterOffset);
		}

		Vector3 relPosUser = (posPointMan - initialPosOffset);
		//relPosUser.z =!mirroredMovement ? -relPosUser.z : relPosUser.z;
		relPosUser.z = -relPosUser.z;

		transform.position = initialPosOffset + 
			(verticalMovement ? Vector3.Scale(new Vector3(relPosUser.x,relPosUser.y,-relPosUser.z+CenterOffset) , moveRate) : Vector3.Scale(new Vector3(relPosUser.x, 0, relPosUser.z+CenterOffset) , moveRate));
		
		// update the local positions of the bones
		for(int i = 0; i < bones.Length; i++) 
		{
			if(bones[i] != null)
			{
				//int joint = !mirroredMovement ? i : (int)KinectInterop.GetMirrorJoint((KinectInterop.JointType)i);
				int joint =  i;
				if(joint < 0)
					continue;
				if(manager.IsJointTracked(userID, joint))
				{
					bones[i].gameObject.SetActive(true);
					
					Vector3 posJoint = manager.GetJointPosition(userID, joint);
					//posJoint.z = !mirroredMovement ? -posJoint.z  : posJoint.z ;
					posJoint.z = -posJoint.z;

					//Quaternion rotJoint = manager.GetJointOrientation(userID, joint, !mirroredMovement);
					Quaternion rotJoint = manager.GetJointOrientation(userID, joint, true);
					rotJoint = initialRotation * rotJoint;

					posJoint -= posPointManMP; //Vector3.Scale(posPointManMP,new Vector3(1f,1f,3f))+new Vector3(0f,0f,9f);
					
//					if(mirroredMovement)
//					{
//						posJoint.x = -posJoint.x;
//						posJoint.z = -posJoint.z ;
//					}

					//TODO: @merlin: Cubeman, find the right scaling for shadow size
					bones[i].transform.localPosition = new Vector3(
						posJoint.x*increasedScaling.x-transform.position.x,																//bla
						(posJoint.y*increasedScaling.y), //*(6f-2.25f*(transform.position.z/increasedMovement.z+4f)/7f)),   //bla
						posJoint.z-CenterOffset//+posPointMan.z//posJoint.z+posPointMan.z/6+2f												//bla
						);
					bones[i].transform.rotation = rotJoint;


				}
				else
				{
					bones[i].gameObject.SetActive(false);
					

				}
			}	
		}
	}

}
