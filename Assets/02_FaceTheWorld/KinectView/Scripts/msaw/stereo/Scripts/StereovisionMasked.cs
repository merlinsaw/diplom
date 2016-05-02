/// <summary>
/// Camera for the Client
/// </summary>


using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//using System.Object;
//[ExecuteInEditMode]
class S3DVM : System.Object
{
	public static float eyeDistance;// 	= 0.35F; 
	public static float focalDistance;// 	= 6.5F; 
}
[ExecuteInEditMode]
public class StereovisionMasked : NetworkBehaviour {
	//public Material anaglyphMat; 
	public Material anaglyphMatMasked;
	
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
	public GameObject leftEyeMask; 
	public GameObject rightEyeMask;
	
	float textureResolution = 2.0f;
	
	
	RenderTexture leftEyeRT;    
	RenderTexture rightEyeRT;
	RenderTexture leftEyeMaskRT;
	RenderTexture rightEyeMaskRT;
	
	Camera camera;
	
	
	
	// Use this for initialization
	void Start () {
		
		//eyeDistance_s = S3DVM.eyeDistance;
		//focalDistance_s = S3DVM.focalDistance;
		
		camera = GetComponent<Camera>();
		
		//leftEye = new GameObject ("leftEye"); 
		//rightEye = new GameObject ("rightEye");
		
		//leftEye.AddComponent<Camera>();
		//rightEye.AddComponent<Camera>();
		
		//leftEye.GetComponent<Camera>().CopyFrom (camera);
		//rightEye.GetComponent<Camera>().CopyFrom (camera);
		
		leftEyeRT = new RenderTexture ((int)(Screen.width*textureResolution), (int)(Screen.height*textureResolution), 24); 
		rightEyeRT = new RenderTexture ((int)(Screen.width*textureResolution), (int)(Screen.height*textureResolution), 24); 
		leftEyeMaskRT = new RenderTexture ((int)(Screen.width*textureResolution), (int)(Screen.height*textureResolution), 24); 
		rightEyeMaskRT = new RenderTexture ((int)(Screen.width*textureResolution), (int)(Screen.height*textureResolution), 24); 
		
		leftEye.GetComponent<Camera>().targetTexture = leftEyeRT; 
		rightEye.GetComponent<Camera>().targetTexture = rightEyeRT; 
		leftEyeMask.GetComponent<Camera>().targetTexture = leftEyeMaskRT; 
		rightEyeMask.GetComponent<Camera>().targetTexture = rightEyeMaskRT;
		
		anaglyphMatMasked.SetTexture ("_LeftTex", leftEyeRT); 
		anaglyphMatMasked.SetTexture ("_RightTex", rightEyeRT);
		// needs to be figured out what to use best as a black mask
		anaglyphMatMasked.SetTexture ("_LeftMask", leftEyeMaskRT);
		anaglyphMatMasked.SetTexture ("_RightMask", rightEyeMaskRT);
		
		leftEye.transform.position = transform.position + transform.TransformDirection(-S3DVM.eyeDistance, 0, 0); 
		rightEye.transform.position = transform.position + transform.TransformDirection(S3DVM.eyeDistance, 0, 0); 
		// currently the eyedistance is set to 0 it could be ment to be the same as the front pair of cameras
		//leftEyeMask.transform.position = transform.position + transform.TransformDirection(-1.27F, 0, 0); 
		//rightEyeMask.transform.position = transform.position + transform.TransformDirection(1.27F, 0, 0); 
		
		leftEye.transform.parent = transform;
		rightEye.transform.parent = transform;
		//leftEyeMask.transform.parent = transform;
		//rightEyeMask.transform.parent = transform;
		
		
		//camera.cullingMask = 0; 
		//camera.backgroundColor = new Color (0,0,0,0);
		//camera.clearFlags = CameraClearFlags.Nothing;
		
	}
	
	void Stop () { 
	} 
	
	void UpdateView() {
		leftEye.GetComponent<Camera>().depth = camera.depth -4; 
		rightEye.GetComponent<Camera>().depth = camera.depth -3;
		leftEyeMask.GetComponent<Camera>().depth = camera.depth -2; 
		rightEyeMask.GetComponent<Camera>().depth = camera.depth -1;
		
		leftEye.transform.position = transform.position + transform.TransformDirection(-S3DVM.eyeDistance, 0, 0); 
		rightEye.transform.position = transform.position + transform.TransformDirection(S3DVM.eyeDistance, 0, 0);
		
		//leftEye.transform.rotation = transform.rotation; 
		//rightEye.transform.rotation = transform.rotation; 
		
		
		//leftEye.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DVM.focalDistance)); 
		//rightEye.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DVM.focalDistance)); 
		
		leftEye.transform.parent = transform; 
		rightEye.transform.parent = transform; 
		
	}
	
	
	void LateUpdate() { 
		UpdateView ();
		
		if (enableKeys) { 
			// o and p 
			float eyeDistanceAdjust = 0.01F; 
			if (Input.GetKeyDown(upEyeDistance)) { 
				S3DVM.eyeDistance += eyeDistanceAdjust;
				eyeDistance_s 	= S3DVM.eyeDistance;
			} else if (Input.GetKeyDown(downEyeDistance)) { 
				S3DVM.eyeDistance -= eyeDistanceAdjust; 
				eyeDistance_s 	= S3DVM.eyeDistance;
			} 
			
			// k and l 
			float focalDistanceAdjust = 0.5F; 
			if (Input.GetKeyDown(upFocalDistance)) { 
				//Debug.Log("focal up"); 
				S3DVM.focalDistance += focalDistanceAdjust;
				focalDistance_s = S3DVM.focalDistance;
			} else if (Input.GetKeyDown(downFocalDistance)) { 
				S3DVM.focalDistance -= focalDistanceAdjust;
				focalDistance_s = S3DVM.focalDistance;
			} 
			S3DVM.eyeDistance = eyeDistance_s;
			S3DVM.focalDistance = focalDistance_s;
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
		for(int i = 0; i < anaglyphMatMasked.passCount; i++) { 
			anaglyphMatMasked.SetPass(i);
			DrawQuad_2 ();
		}
		GL.PopMatrix(); 
	}
	
	private void DrawQuad() { 
		GL.Begin (GL.QUADS); 
		GL.TexCoord2( 0.0F, 0.0F ); GL.Vertex3( 0.0F, 0.0F, zvalue ); 
		GL.TexCoord2( 1.0F, 0.0F ); GL.Vertex3( 1.0F, 0.0F, zvalue ); 
		GL.TexCoord2( 1.0F, 1.0F ); GL.Vertex3( 1.0F, 1.0F, zvalue ); 
		GL.TexCoord2( 0.0F, 1.0F ); GL.Vertex3( 0.0F, 1.0F, zvalue ); 
		GL.End(); 
	} 
	// the Quad_2 can be manipulated via the gui
	[SyncVar]
	public float lowerLeft_x;
	[SyncVar]
	public float lowerLeft_y;
	[SyncVar]
	public float lowerRight_x;
	[SyncVar]
	public float lowerRight_y;
	[SyncVar]
	public float upperRight_x;
	[SyncVar]
	public float upperRight_y;
	[SyncVar]
	public float upperLeft_x;
	[SyncVar]
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
