// Set an off-center projection, where perspective's vanishing
// point is not necessarily in the center of the screen.
//
// left/right/top/bottom define near plane size, i.e.
// how offset are corners of camera's near plane.
// Tweak the values and you can see camera's frustum change.

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CopyCameraMatrix : MonoBehaviour {
	
	//if (parent.name == "leftEye"){  Debug.Log(parent.name);}
	public GameObject CopyMatrixFrom; 
	public float left ;	//-0.4F
	public float right ;		//0.425F
	public float top ;		//0.4F
	public float bottom ;	//-0.2F
	public float nearPlane ;
	void remote()
	{
		transform.position = CopyMatrixFrom.transform.position;
		this.GetComponent<Camera>().nearClipPlane = CopyMatrixFrom.transform.GetComponent<Camera>().nearClipPlane;
		this.GetComponent<Camera>().farClipPlane = CopyMatrixFrom.transform.GetComponent<Camera>().farClipPlane;
		left = CopyMatrixFrom.transform.GetComponent<Example>().left; 		//-0.4F
		right = CopyMatrixFrom.transform.GetComponent<Example>().right; 		//0.425F
		top = CopyMatrixFrom.transform.GetComponent<Example>().top;   		//0.4F
		bottom = CopyMatrixFrom.transform.GetComponent<Example>().bottom; 	//-0.2F
		nearPlane = CopyMatrixFrom.transform.GetComponent<Example>().nearClipPlane;
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