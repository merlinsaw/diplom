using UnityEngine;
using System.Collections;

public class HoverAnimation : MonoBehaviour {
	public float amplitude = 0.02F;
	public float speed = 1F;
	Vector3 tempPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// hovering animation		
		tempPos.y = amplitude * Mathf.Sin(speed * Time.time);
		this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y+tempPos.y,this.transform.position.z);
	}
}
	

