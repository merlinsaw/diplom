using UnityEngine;
using System.Collections;

public class move_WASD : MonoBehaviour {

	//#pragma strict
	public bool enableKeys             	= true; 

	public float speed = 1.0f;
	
	void Start() {

	}

	void Update() { 
		if (enableKeys){
			if (Input.GetKey(KeyCode.E))
				transform.GetComponent<MoveRigidbody>().Movement = false;
			if (Input.GetKey(KeyCode.Q))
				transform.GetComponent<MoveRigidbody>().Movement = true;
			if (Input.GetKey(KeyCode.W))
				transform.position += Vector3.forward * speed * Time.deltaTime;
			if (Input.GetKey(KeyCode.S))
				transform.position += Vector3.back * speed * Time.deltaTime;
			if (Input.GetKey(KeyCode.A))
				transform.position += Vector3.left * speed * Time.deltaTime;
			if (Input.GetKey(KeyCode.D))
				transform.position += Vector3.right * speed * Time.deltaTime;
			}
	}
}
