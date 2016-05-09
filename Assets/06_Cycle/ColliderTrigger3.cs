
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

class ColliderTrigger3 : NetworkBehaviour {

	private Transform TriggerZone;

	private PlayerCameraController _PlayerCameraController;
	[Tooltip("Link the CycleRoomCameraController to this Object")]
	public GameObject CycleRoomCameraController;
	public ColliderPosition _SetColliderPosition;


	public enum ColliderPosition{
		Left,
		Right
	}
		
	// Use this for initialization
	void Start () {
		//_PlayerCameraController = transform.parent.GetComponent<PlayerCameraController>();
		if (CycleRoomCameraController != null){
		_PlayerCameraController = CycleRoomCameraController.GetComponent<PlayerCameraController>();
		TriggerZone = this.transform;
		}else{Debug.LogError("Link the CycleRoomCameraController to the "+ transform.name +" Object in the Inspector");}
	}

	void OnTriggerEnter (Collider Player){
		Debug.Log("Collision Enter: " + Player.name + " -> " + TriggerZone.name);
		_PlayerCameraController.CurrentCameraDirection = (PlayerCameraController.CameraDirections)_SetColliderPosition;//(PlayerCameraController.CameraDirection)_SetColliderPosition;

	}
	void OnTriggerExit (Collider Player){
		_PlayerCameraController.CurrentCameraDirection = PlayerCameraController.CameraDirections.Fixed;
		Debug.Log("Collision Leave: " + Player.name + " -> " + TriggerZone.name);
		//animate = false;	}
	}	
}



