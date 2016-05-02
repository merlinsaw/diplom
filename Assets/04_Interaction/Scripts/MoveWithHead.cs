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
				HeadPosition = Head.transform.localPosition;//position;
						

					if (!headmovement){
					// do nothing
					}else{
					//this.transform.position = new Vector3((HeadPosition.x+offset.x)*CamMovementXfactor,offset.y,(HeadPosition.z-offset.z));
					this.transform.localPosition = new Vector3((HeadPosition.x+offset.x)*CamMovementXfactor,offset.y,(HeadPosition.z-offset.z));
					}
				}
			if (isServer == false){
			}
		}
	}
}
