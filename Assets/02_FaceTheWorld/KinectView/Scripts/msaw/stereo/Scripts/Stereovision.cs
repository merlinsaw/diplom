/// <summary>
/// Camera for the server
/// </summary>


using UnityEngine;
using System.Collections;
//using System.Object;
//[ExecuteInEditMode]
class S3DV : System.Object
{
	public static float eyeDistance = 0.5F;// 	= 0.35F; 
	public static float focalDistance;// 	= 6.5F; 
}
[ExecuteInEditMode]
// should be ranamed stereo vision server when claening up the code
public class Stereovision : MonoBehaviour {
	public Material anaglyphMat; 

	public bool enableKeys             	= true; 
		
	public KeyCode downEyeDistance		= KeyCode.O; 
	public KeyCode upEyeDistance		= KeyCode.P; 
	public KeyCode downFocalDistance	= KeyCode.K; 
	public KeyCode upFocalDistance		= KeyCode.L;
	
	public float zvalue = 0.1F;
	public float eyeDistance_s;
	public float focalDistance_s;
	public float innerOffaxis = 0.4f;
	public float outerOffaxis = 0.564214f;
	public float upperOffaxis = 0.31066f;
	public float lowerOffaxis = 0.31066f;
	public float toeIn = 0;

	public GameObject leftEye; 
	public GameObject rightEye;

	float textureResolution = 2.0f;


	RenderTexture leftEyeRT;    
	RenderTexture rightEyeRT;

	Camera camera;



	// Use this for initialization
	void Start () {

		S3DV.eyeDistance = eyeDistance_s;
		//focalDistance_s = S3DV.focalDistance;

		camera = GetComponent<Camera>();

		//leftEye = new GameObject ("leftEye"); 
		//rightEye = new GameObject ("rightEye");

		//leftEye.AddComponent<Camera>();
		//rightEye.AddComponent<Camera>();

		//leftEye.GetComponent<Camera>().CopyFrom (camera);
		//rightEye.GetComponent<Camera>().CopyFrom (camera);

		leftEyeRT = new RenderTexture ((int)(Screen.width*textureResolution), (int)(Screen.height*textureResolution), 24); 
		rightEyeRT = new RenderTexture ((int)(Screen.width*textureResolution), (int)(Screen.height*textureResolution), 24); 

		leftEye.GetComponent<Camera>().targetTexture = leftEyeRT; 
		rightEye.GetComponent<Camera>().targetTexture = rightEyeRT; 
		// the cameras are reversed for cross eye positions in the foreground
		anaglyphMat.SetTexture ("_LeftTex", leftEyeRT); 
		anaglyphMat.SetTexture ("_RightTex", rightEyeRT);

		leftEye.transform.position = transform.position + transform.TransformDirection(-S3DV.eyeDistance, 0, 0); 
		rightEye.transform.position = transform.position + transform.TransformDirection(S3DV.eyeDistance, 0, 0); 

		leftEye.transform.parent = transform;
		rightEye.transform.parent = transform;


		//camera.cullingMask = 0; 
		//camera.backgroundColor = new Color (0,0,0,0);
		//camera.clearFlags = CameraClearFlags.Nothing;
	
	}

	void Stop () { 
	} 

	void UpdateView() {
		leftEye.GetComponent<Camera>().depth = camera.depth -2; 
		rightEye.GetComponent<Camera>().depth = camera.depth -1; 

		leftEye.transform.position = transform.position + transform.TransformDirection(-S3DV.eyeDistance, 0, 0); 
		rightEye.transform.position = transform.position + transform.TransformDirection(S3DV.eyeDistance, 0, 0);
	
		//leftEye.transform.rotation = transform.rotation; 
		//rightEye.transform.rotation = transform.rotation; 


		leftEye.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DV.focalDistance)); 
		rightEye.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DV.focalDistance)); 
		
		leftEye.transform.parent = transform; 
		rightEye.transform.parent = transform; 

	}

	/// <summary>
	/// Key controls for the stereo vision settings	
	/// </summary>
	void LateUpdate() { 
		UpdateView ();

		if (enableKeys) { 
			// o and p 
			float eyeDistanceAdjust = 0.01F; 
			if (Input.GetKeyDown(upEyeDistance)) { 
				S3DV.eyeDistance += eyeDistanceAdjust;
				eyeDistance_s 	= S3DV.eyeDistance;
			} else if (Input.GetKeyDown(downEyeDistance)) { 
				S3DV.eyeDistance -= eyeDistanceAdjust; 
				eyeDistance_s 	= S3DV.eyeDistance;
			} 
			
			// k and l 
			float focalDistanceAdjust = 0.5F; 
			if (Input.GetKeyDown(upFocalDistance)) { 
				//Debug.Log("focal up"); 
				S3DV.focalDistance += focalDistanceAdjust;
				focalDistance_s = S3DV.focalDistance;
			} else if (Input.GetKeyDown(downFocalDistance)) { 
				S3DV.focalDistance -= focalDistanceAdjust;
				focalDistance_s = S3DV.focalDistance;
			} 
			S3DV.eyeDistance = eyeDistance_s;
			S3DV.focalDistance = focalDistance_s;
		}
	}
	// Update is called once per frame
	void Update () {


	}
	//public void OnPostRender() {
	void OnRenderImage (RenderTexture source, RenderTexture destination) { 
		RenderTexture.active = destination; 
		GL.PushMatrix(); 
		GL.LoadOrtho();
		for(int i = 0; i < anaglyphMat.passCount; i++) { 
			anaglyphMat.SetPass(i);
			DrawQuad_2 ();
		}
		GL.PopMatrix(); 
	}
	//this Quad is only for backup it's the default not manipulated one
//	private void DrawQuad() { 
//		GL.Begin (GL.QUADS); 
//		GL.TexCoord2( 0.0F, 0.0F ); GL.Vertex3( 0.0F, 0.0F, zvalue ); 
//		GL.TexCoord2( 1.0F, 0.0F ); GL.Vertex3( 1.0F, 0.0F, zvalue ); 
//		GL.TexCoord2( 1.0F, 1.0F ); GL.Vertex3( 1.0F, 1.0F, zvalue ); 
//		GL.TexCoord2( 0.0F, 1.0F ); GL.Vertex3( 0.0F, 1.0F, zvalue ); 
//		GL.End(); 
//	} 

	// mapping corners for the projection Quad
	public float lowerLeft_x;
	public float lowerLeft_y;
	public float lowerRight_x;
	public float lowerRight_y;
	public float upperRight_x;
	public float upperRight_y;
	public float upperLeft_x;
	public float upperLeft_y;


	//Experimental new Quad for UV mapping tests
	private void DrawQuad_2() { 
		GL.Begin (GL.QUADS); 
		//lower left
		GL.TexCoord2( 0.0F, 0.0F ); GL.Vertex3( lowerLeft_x, lowerLeft_y, zvalue ); 
		//lower right
		GL.TexCoord2( 1.0F, 0.0F ); GL.Vertex3( lowerRight_x, lowerRight_y, zvalue ); 
		//upper right
		GL.TexCoord2( 1.0F, 1.0F ); GL.Vertex3( upperRight_x, upperRight_y, zvalue );
		//upper left
		GL.TexCoord2( 0.0F, 1.0F ); GL.Vertex3( upperLeft_x, upperLeft_y, zvalue ); 
		GL.End(); 
	} 
}
