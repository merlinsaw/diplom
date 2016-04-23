
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

class ColliderTrigger3 : NetworkBehaviour {

	Transform TriggerZone;

	public GameObject Room;
	public GameObject RoomFront;
	public GameObject RoomEmission;
	private int direction;
	public bool animate = false;
	public CameraDirection _CameraDirection;
	public enum CameraDirection{
		Left,
		Right
	}

	Vector3 angle ;
	public int speed = 5;
		
	// Use this for initialization
	void Start () {
		TriggerZone = this.transform;
		switch (_CameraDirection) {
			case CameraDirection.Left:
				//rotate the Room clockwise
				direction = -1;
				break;
			case CameraDirection.Right:
				//rotate the Room counter clockwise
				direction = 1;
				break;
			default:
				Debug.LogError ("_CameraDirection is not set to left or right");
				break;
		}
	}
	
	// Update is called once per frame
	void RotateRoom (int dir) {
		angle.y += Time.deltaTime * speed * dir;
		if (Room != null){
			Room.transform.eulerAngles = angle;
		}
		if (RoomFront != null){
			RoomFront.transform.eulerAngles = angle;
		}
		if (RoomEmission != null){
			RoomEmission.transform.eulerAngles = angle;
		}
	}
//	void OnTriggerEnter (Collider Player){
//
//	}
	void Update(){
		if (Room != null){
			if (animate){
				RotateRoom(direction);
			}
		}else{
			Debug.LogError("The Room Prefab is not assined");
		}
	}

	void OnTriggerEnter (Collider Player){
		Debug.Log("Collision Enter: " + Player.gameObject.name + " -> " + TriggerZone.gameObject.name);
		angle = Room.transform.eulerAngles ;
		animate = true;
	}
	void OnTriggerExit (Collider Player){
		Debug.Log("Collision Leave: " + Player.gameObject.name + " -> " + TriggerZone.gameObject.name);
		animate = false;
	}
	
}



