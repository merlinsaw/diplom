using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MoveWithHead : NetworkBehaviour {
	public bool headmovement = false;
	public Vector3 offset;
	public GameObject Head;
	static Vector3 HeadPosition;

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
						this.transform.position = new Vector3(0,1.8f,-4);
					}else{
						this.transform.position = new Vector3(HeadPosition.x+offset.x,offset.y,HeadPosition.z-offset.z);
					}
				}
			if (isServer == false){
			}
		}
	}
}
