// Set an off-center projection, where perspective's vanishing
// point is not necessarily in the center of the screen.
//
// left/right/top/bottom define near plane size, i.e.
// how offset are corners of camera's near plane.
// Tweak the values and you can see camera's frustum change.

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class OffsetCameraMatrix : MonoBehaviour {
	
	//if (parent.name == "leftEye"){  Debug.Log(parent.name);}
	
	public float left = -0.2F; 		//-0.4F
	public float right = 0.2F; 		//0.425F
	public float top = 0.2F;   		//0.4F
	public float bottom = -0.2F; 	//-0.2F
	[Tooltip("this value is used for the camera matrix instead of the natural clipping Camera plane")]
	public float nearPlane = 3.0F;
	void remote()
	{
		if (transform.name == "leftEyeBack") {
			left = transform.parent.GetComponent<StereovisionMasked>().outerOffaxis * -1.0f;
			right = transform.parent.GetComponent<StereovisionMasked>().innerOffaxis;
		} 
		else if (transform.name == "rightEyeBack") {
			left = transform.parent.GetComponent<StereovisionMasked>().innerOffaxis * -1.0f;
			right = transform.parent.GetComponent<StereovisionMasked>().outerOffaxis;
		}
		if (transform.name == "leftEyeBack" || transform.name == "rightEyeBack"){
		top = transform.parent.GetComponent<StereovisionMasked>().upperOffaxis;
		//top = transform.parent.GetComponent<Stereovision> ().innerOffaxis * 1.7f;
		bottom = transform.parent.GetComponent<StereovisionMasked>().lowerOffaxis *-1.0f;
		}
	}
	
	void LateUpdate() {
		remote ();
		Camera cam = GetComponent<Camera>();
		//@testing without the near plane connection in calcualtion
		//Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, cam.nearClipPlane, cam.farClipPlane);
		Matrix4x4 m = PerspectiveOffCenter(left, right, bottom, top, nearPlane, cam.farClipPlane);
		cam.projectionMatrix = m;
	}
	static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far) {
		float x = 2.0F * near / (right - left);
		float y = 2.0F * near / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0F * far * near) / (far - near);
		float e = -1.0F;
		Matrix4x4 m = new Matrix4x4();
		m[0, 0] = x; m[1, 0] = 0; m[2, 0] = 0; m[3, 0] = 0;
		m[0, 1] = 0; m[1, 1] = y; m[2, 1] = 0; m[3, 1] = 0;
		m[0, 2] = a; m[1, 2] = b; m[2, 2] = c; m[3, 2] = e;
		m[0, 3] = 0; m[1, 3] = 0; m[2, 3] = d; m[3, 3] = 0;
		return m;
	}
}