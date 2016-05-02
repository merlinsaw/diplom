
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

class PlayerCameraController : NetworkBehaviour {

	Transform TriggerZone;
	
	public GameObject StereoCameraRig;
	public GameObject RoomBackground;
	public GameObject RoomForeground;
	public GameObject RoomEmission;
	public GameObject Player;
	public GameObject StereoCameraRigParent;
	public Collider LeftColliderZone;
	public Collider RightColliderZone;
	private int direction;
	public bool RotateRoom = true;
	public bool RotateCam = true;
	public bool RotatePlayer = true;
	public CameraDirections _CameraDirection;
	public enum CameraDirections{
		Left,
		Right,
		Fixed
	}
	public CameraDirections CurrentCameraDirection{
		get{ 
			return _CameraDirection; 
		}
		set{ 
			_CameraDirection = value;
			Debug.Log("Enum just got changed to: " + _CameraDirection +" <-----------------");
			//TODO: @merlin: change this Enumerator code to an event handler
			if (!RotateCam) return;
			switch(CurrentCameraDirection){
			case CameraDirections.Left:
				SetBlendedEulerAngles(new Vector3(0,360-MaxCamAngle,0));
				StartCoroutine(ContinuousRotation(turningRate));
				break;
			case CameraDirections.Right:
				SetBlendedEulerAngles(new Vector3(0,MaxCamAngle,0));
				StartCoroutine(ContinuousRotation(turningRate));

				break;
			case CameraDirections.Fixed:
				SetBlendedEulerAngles(new Vector3(0,0,0));
				StartCoroutine(ContinuousRotation(backTurningRate));
				break;
			default:
				break;
			}

		}
	}
   

	Vector3 roomAngle ;
	Vector3 camAngle;
	Vector3 playerAngle;
	[Tooltip("Traveling speed of the turning Room.")]
	public float RoomSpeed = 3f;
	[Tooltip("Traveling speed of the turning Player.")]
	public float PlayerSpeed = 3f;
	[Tooltip("Maximum degrees, when facing the curved Room.")]
	public float MaxCamAngle = 17f;
	[Tooltip("Maximum turn rate in degrees per second, when facing the curved Room.")]
	public float turningRate = 30f;
	[Tooltip("Maximum turn rate in degrees per second, when returning to the static room.")]
	public float backTurningRate = 30f;
	// Rotation we should blend towards.
	private Quaternion _targetRotation = Quaternion.identity;
		
	// Use this for initialization
	void Start () {
		TriggerZone = this.transform;
		roomAngle.y = RoomBackground.transform.eulerAngles.y;
		playerAngle.y = Player.transform.eulerAngles.y;
	}
	
	// Update is called once per frame
	void RotateTheRoom (int dir) {
		if (dir != 0){
			roomAngle.y += Time.deltaTime * RoomSpeed * dir;
			if (RoomBackground != null){
				RoomBackground.transform.eulerAngles = roomAngle;
			}
			if (RoomForeground != null){
				RoomForeground.transform.eulerAngles = roomAngle;
			}
			if (RoomEmission != null){
				RoomEmission.transform.eulerAngles = roomAngle;
			}
		}
	}
	// Update is called once per frame
	void RotateThePlayer (int dir) {
		if (dir != 0){
			playerAngle.y += Time.deltaTime * PlayerSpeed * dir;
			if (Player != null){
				Player.transform.eulerAngles = playerAngle;
				transform.eulerAngles = playerAngle;
				StereoCameraRigParent.transform.eulerAngles = playerAngle;
			}
		}
	}

	// Call this when you want to turn the camera smoothly.
	public void SetBlendedEulerAngles(Vector3 angles)
	{
		_targetRotation = Quaternion.Euler(angles);
	}
	
	// Turn the camera towards our target rotation.
	IEnumerator ContinuousRotation (float _turningRate)
	{
		while(StereoCameraRig.transform.rotation != _targetRotation){
			StereoCameraRig.transform.rotation = Quaternion.RotateTowards(StereoCameraRig.transform.rotation, _targetRotation, _turningRate * Time.deltaTime);
			yield return new WaitForSeconds (0.01f);
		}
	}

	void Update(){
		if (RoomBackground != null){
			if (RotateRoom){
				RotateTheRoom(CurrentCameraDirection == CameraDirections.Left ? -1 : CurrentCameraDirection ==  CameraDirections.Right ? 1 : 0);
			}
		}else{
			Debug.LogError("The Room Prefab is not assined");
		}
		if (RotatePlayer){
			RotateThePlayer(CurrentCameraDirection == CameraDirections.Left ? 1 : CurrentCameraDirection ==  CameraDirections.Right ? -1 : 0);
		}
	}
}


	