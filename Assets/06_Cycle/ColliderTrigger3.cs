
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

class ColliderTrigger3 : NetworkBehaviour {

	private Transform TriggerZone;

	private PlayerCameraController _PlayerCameraController;
	public ColliderPosition _ColliderPosition;

	public enum ColliderPosition{
		Left,
		Right
	}
		
	// Use this for initialization
	void Start () {
		_PlayerCameraController = transform.parent.GetComponent<PlayerCameraController>();
		TriggerZone = this.transform;
	}

	void OnTriggerEnter (Collider Player){
		Debug.Log("Collision Enter: " + Player.name + " -> " + TriggerZone.name);
		_PlayerCameraController.CurrentCameraDirection = (PlayerCameraController.CameraDirections)_ColliderPosition;//(PlayerCameraController.CameraDirection)_ColliderPosition;

	}
	void OnTriggerExit (Collider Player){
		_PlayerCameraController.CurrentCameraDirection = PlayerCameraController.CameraDirections.Fixed;
		Debug.Log("Collision Leave: " + Player.name + " -> " + TriggerZone.name);
		//animate = false;	}
	}	
}



