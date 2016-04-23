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

	//TODO: set the multiplier
	[Tooltip("Rate Multiplier at which the cubeman will move through the scene.")]
	private Vector3 increasedMovement = new Vector3(3f,1f,4f);






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
	private LineRenderer[] lines;

	private LineRenderer lineTLeft;
	private LineRenderer lineTRight;
	private LineRenderer lineFLeft;
	private LineRenderer lineFRight;

	private Vector3 initialPosition;
	private Quaternion initialRotation;
	private Vector3 initialPosOffset = Vector3.zero;
	private Int64 initialPosUserID = 0;

	// Initial position and rotation of the transform
	[Tooltip("Position this transform is offseted by")]
	public Vector3 offsetNodePos = new Vector3(0,0,3f);
	protected Quaternion offsetNodeRot;


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
		
		// array holding the skeleton lines
		lines = new LineRenderer[bones.Length];
		
//		if(skeletonLine)
//		{
//			for(int i = 0; i < lines.Length; i++)
//			{
//				Debug.Log ("Line: " + i + " instantiate started.");
//
//				if((i == 22 || i == 24) && debugLine)
//					lines[i] = Instantiate(debugLine) as LineRenderer;
//				else
//					lines[i] = Instantiate(skeletonLine) as LineRenderer;
//
//				lines[i].transform.parent = transform;
//			}
//		}

		initialPosition = new Vector3(0,0,-6f);//Vector3.zero;// transform.position;
		initialRotation = transform.rotation;
		//transform.rotation = Quaternion.identity;
	}
	

	void Update () 
	{
		KinectManager manager = KinectManager.Instance;
		
		// get 1st player
		Int64 userID = manager ? manager.GetUserIdByIndex(playerIndex) : 0;
		
		if(userID <= 0)
		{
			initialPosUserID = 0;
			initialPosOffset = Vector3.zero;

			// reset the pointman position and rotation
			if(transform.position != initialPosition)
			{
				transform.position = Vector3.Scale(initialPosition,new Vector3(1f,1f,1f)); //+ offsetNodePos;
			}
			
			if(transform.rotation != initialRotation)
			{
				transform.rotation = initialRotation;
			}

			for(int i = 0; i < bones.Length; i++) 
			{
				bones[i].gameObject.SetActive(true);

				bones[i].transform.localPosition = Vector3.zero;
				bones[i].transform.localRotation = Quaternion.identity;
				
				if(lines[i] != null)
				{
					lines[i].gameObject.SetActive(false);
				}
			}

			return;
		}
		
		// set the position in space
		//TODO: figure out a nice value for X movement
		Vector3 posPointMan = Vector3.Scale (Vector3.Scale(offsetNodePos-manager.GetUserPosition(userID),new Vector3(4f,1f,3f))+new Vector3(0f,0f,9f),increasedMovement);
		data = posPointMan;
		Vector3 posPointManMP = new Vector3(posPointMan.x, posPointMan.y, !mirroredMovement ? posPointMan.z  : posPointMan.z );
		
		// store the initial position
		if(initialPosUserID != userID)
		{
			initialPosUserID = userID;
			//initialPosOffset = transform.position - (verticalMovement ? posPointMan * moveRate : new Vector3(posPointMan.x, 0, posPointMan.z) * moveRate);
			initialPosOffset = posPointMan;
		}

		Vector3 relPosUser = (posPointMan - initialPosOffset);
		relPosUser.z =!mirroredMovement ? -relPosUser.z : relPosUser.z;

		transform.position = initialPosOffset + 
			(verticalMovement ? Vector3.Scale(new Vector3(relPosUser.x,relPosUser.y,-relPosUser.z) , moveRate) : Vector3.Scale(new Vector3(relPosUser.x, 0, relPosUser.z) , moveRate));
		
		// update the local positions of the bones
		for(int i = 0; i < bones.Length; i++) 
		{
			if(bones[i] != null)
			{
				int joint = !mirroredMovement ? i : (int)KinectInterop.GetMirrorJoint((KinectInterop.JointType)i);
				if(joint < 0)
					continue;
				if(manager.IsJointTracked(userID, joint))
				{
					bones[i].gameObject.SetActive(true);
					
					Vector3 posJoint = manager.GetJointPosition(userID, joint);
					posJoint.z = !mirroredMovement ? -posJoint.z  : posJoint.z ;
					
					Quaternion rotJoint = manager.GetJointOrientation(userID, joint, !mirroredMovement);
					rotJoint = initialRotation * rotJoint;

					posJoint -= posPointManMP; //Vector3.Scale(posPointManMP,new Vector3(1f,1f,3f))+new Vector3(0f,0f,9f);
					
					if(mirroredMovement)
					{
						posJoint.x = -posJoint.x;
						posJoint.z = -posJoint.z ;
					}

					//TODO: find the right scaling for shadow size
					bones[i].transform.localPosition = new Vector3(
						posJoint.x*3f,
						(posJoint.y*(6f-2.25f*(transform.position.z/increasedMovement.z+4f)/7f)),
						posJoint.z+posPointMan.z/3*2+2f
						);
					bones[i].transform.rotation = rotJoint;
					
					if(lines[i] == null && skeletonLine != null) 
					{
						lines[i] = Instantiate((i == 22 || i == 24) && debugLine ? debugLine : skeletonLine) as LineRenderer;
						lines[i].transform.parent = transform;
					}

					if(lines[i] != null)
					{
						lines[i].gameObject.SetActive(true);
						Vector3 posJoint2 = bones[i].transform.position;
						
						Vector3 dirFromParent = manager.GetJointDirection(userID, joint, false, false);
						dirFromParent.z = !mirroredMovement ? -dirFromParent.z : dirFromParent.z;
						Vector3 posParent = posJoint2 - dirFromParent;
						
						//lines[i].SetVertexCount(2);
						lines[i].SetPosition(0, posParent);
						lines[i].SetPosition(1, posJoint2);
					}

				}
				else
				{
					bones[i].gameObject.SetActive(false);
					
					if(lines[i] != null)
					{
						lines[i].gameObject.SetActive(false);
					}
				}
			}	
		}
	}

}
