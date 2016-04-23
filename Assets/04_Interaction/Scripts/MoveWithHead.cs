using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MoveWithHead : NetworkBehaviour {
	public bool headmovement = false;
	public Vector3 offset;
	public GameObject Head;
	static Vector3 HeadPosition;
	[Tooltip("Factor to reduce the camera movement on X")]
	public float CamMovementXfactor = 0.73f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
			if (GetComponent<NetworkIdentity>().isServer)
			{
				if (isServer == true){
					//if (Head != null){
					//	Debug.LogError(Head.transform.name+"Head prefab missing.");

					//}else{
						HeadPosition = Head.transform.position;
					//}
					if (!headmovement){
						//this.transform.position = new Vector3(0,1.8f,-4);
						//idle
				}else{
//					if (5.0f-HeadPosition.z-offset.z <= 0){
//						CamMovementXfactor = 0;
//					}else{
//						CamMovementXfactor = (5.0f-HeadPosition.z-offset.z)/7.0f;
//				}

					this.transform.position = new Vector3((HeadPosition.x+offset.x)*CamMovementXfactor,offset.y,(HeadPosition.z-offset.z));
					}
				}
			if (isServer == false){
			}
		}
	}
}
