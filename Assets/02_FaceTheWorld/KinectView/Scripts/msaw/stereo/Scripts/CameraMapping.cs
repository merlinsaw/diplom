/// <summary>
/// Camera for the Client
/// </summary>


using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//using System.Object;
//[ExecuteInEditMode]

[ExecuteInEditMode]
public class CameraMapping : NetworkBehaviour {
	//public Material anaglyphMat; 
	public Material MergedCameras;

	public float zvalue = 0.1F;

	public GameObject FrontCam; 
	public GameObject BackCam;
	
	float textureResolution = 2.0f;
	
	RenderTexture FrontRT;    
	RenderTexture BackRT;


	
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
		
		FrontRT = new RenderTexture ((int)(Screen.width*textureResolution), (int)(Screen.height*textureResolution), 24); 
		BackRT = new RenderTexture ((int)(Screen.width*textureResolution), (int)(Screen.height*textureResolution), 24); 
	
		
		FrontCam.GetComponent<Camera>().targetTexture = FrontRT; 
		BackCam.GetComponent<Camera>().targetTexture = BackRT; 

		
		MergedCameras.SetTexture ("_FrontTex", FrontRT); 
		MergedCameras.SetTexture ("_BackTex", BackRT);


		
	}
	
	void Stop () { 
	} 
	
	void UpdateView() {
		FrontCam.GetComponent<Camera>().depth = camera.depth -2; 
		BackCam.GetComponent<Camera>().depth = camera.depth -1;

		
	}
	
	
	void LateUpdate() { 
		UpdateView ();

	}
	// Update is called once per frame
	void Update () {
		
		
	}
	//public void OnPostRender() {
	void OnRenderImage (RenderTexture source, RenderTexture destination) { 
		RenderTexture.active = destination; 
		GL.PushMatrix(); 
		GL.LoadOrtho();
		for(int i = 0; i < MergedCameras.passCount; i++) { 
			MergedCameras.SetPass(i);
			DrawQuad();
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
}
