using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	public float
		panSpeed = 4.0f,
		zoomSpeed = 4.0f;
	private Vector3 mouseOrigin;
	private bool 
		isPanning,
		isZooming;
	private GameObject c;
	
	void Awake() {
		StartCoroutine("lookAtCenter");
		c = GameObject.Find("Center");
	}
	
	IEnumerator lookAtCenter() {
		yield return new WaitForFixedUpdate();
//		transform.LookAt(c.transform.position); 
	}
	
	void FixedUpdate() {
		if (Input.GetMouseButtonDown(0)) { mouseOrigin = Input.mousePosition; isPanning = true;
		} else if (Input.GetMouseButtonDown(1)) { mouseOrigin = Input.mousePosition; isZooming = true; }
		if (!Input.GetMouseButton(0)) isPanning = false;
		if (!Input.GetMouseButton(1)) isZooming = false;
		if (Input.GetKey("escape")) Application.Quit();
		
		if (isPanning) {        transform.Translate(-new Vector3(Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin).x * panSpeed, Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin).y * panSpeed, 0), Space.Self); transform.LookAt(c.transform.position);
		} else if (isZooming) { transform.Translate(Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin).y * zoomSpeed * transform.forward, Space.Self); transform.LookAt(c.transform.position); }
	}
}