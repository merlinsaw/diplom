using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	Vector3 angle ;
	public int speed = 100;
	// Use this for initialization
	void Start () {
		angle = transform.eulerAngles ;
	}
	
	// Update is called once per frame
	void Update () {
		angle.y += Time.deltaTime * speed ;
		transform.eulerAngles = angle ;
	}
}
