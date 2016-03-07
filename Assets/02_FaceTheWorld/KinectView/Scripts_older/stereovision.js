var anaglyphMat; 

var leftEyeRT;    
var rightEyeRT; 

var leftEye; 
var rightEye; 

var enableKeys             : boolean    = true; 

var downEyeDistance       : KeyCode    = KeyCode.O; 
var upEyeDistance          : KeyCode    = KeyCode.P; 
var downFocalDistance       : KeyCode    = KeyCode.K; 
var upFocalDistance       : KeyCode    = KeyCode.L; 

var zvalue               : float      = 0.0; // original: 1.0 

class S3DV extends System.Object { 
   static var eyeDistance = 0.035; 
   static var focalDistance = 6.5; 
}; 

function Start () { 
   leftEye = new GameObject ("leftEye", Camera); 
   rightEye = new GameObject ("rightEye", Camera); 
    
   leftEye.camera.CopyFrom (GetComponent.<Camera>()); 
   rightEye.camera.CopyFrom (GetComponent.<Camera>()); 
    
  // leftEyeRT = new RenderTexture (Screen.width, Screen.height, 24); 
  // rightEyeRT = new RenderTexture (Screen.width, Screen.height, 24);
   leftEyeRT = new RenderTexture (Screen.width, Screen.height, 24); 
   rightEyeRT = new RenderTexture (Screen.width, Screen.height, 24); 
       
    
   anaglyphMat = new Material 
   (    
      "Shader \"Hidden/Anaglyph\"" + 
      "{" + 
      "    Properties" + 
      "    {" + 
      "        _Color (\"Main Color, Alpha\", Color) = (1,1,1,1)" + 
      "        _LeftTex (\"Left (RGB)\", RECT) = \"white\" {}" + 
      "        _RightTex (\"Right (RGB)\", RECT) = \"white\" {}" + 
      "    }" + 
      "    Category" + 
      "    {" + 
      "        ZWrite Off" + 
      "        ZTest Always" + 
      "        Lighting On" + 
      "        Tags {Queue=Transparent}" + 
      "        SubShader" + 
      "        {" + 
      "            Pass" + 
      "            {" + 
      "               ColorMask R" + 
      "               Cull Off" + 
      "               Material" + 
      "               {" + 
      "                   Emission [_Color]" + 
      "               }" + 
      "" +              
      "              SetTexture [_LeftTex]" + 
      "               {" + 
      "                   Combine texture * primary, texture + primary" + 
      "               }" + 
      "           }" + 
      "" +            
      "           Pass" + 
      "            {" + 
      "               ColorMask GB" + 
      "               Cull Off" + 
      "               Material" + 
      "               {" + 
      "                   Emission [_Color]" + 
      "               }" + 
      "" +            
      "               SetTexture [_RightTex]" + 
      "               {" + 
      "                   Combine texture * primary, texture + primary" + 
      "               }" + 
      "           }" + 
      "       }" + 
      "    }" + 
      "}" 
   ); 
    
   leftEye.camera.targetTexture = leftEyeRT; 
   rightEye.camera.targetTexture = rightEyeRT; 
      
   anaglyphMat.SetTexture ("_LeftTex", leftEyeRT); 
   anaglyphMat.SetTexture ("_RightTex", rightEyeRT); 
      
   leftEye.camera.depth = GetComponent.<Camera>().depth -2; 
   rightEye.camera.depth = GetComponent.<Camera>().depth -1; 
    
   leftEye.transform.position = transform.position + transform.TransformDirection(-S3DV.eyeDistance, 0, 0); 
   rightEye.transform.position = transform.position + transform.TransformDirection(S3DV.eyeDistance, 0, 0); 
    
   leftEye.transform.rotation = transform.rotation; 
   rightEye.transform.rotation = transform.rotation; 
    
   leftEye.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DV.focalDistance)); 
   rightEye.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DV.focalDistance)); 
    
   leftEye.transform.parent = transform; 
   rightEye.transform.parent = transform; 
    
   GetComponent.<Camera>().cullingMask = 0; 
   GetComponent.<Camera>().backgroundColor = Color (0,0,0,0); 
   //camera.Render(); 
   GetComponent.<Camera>().clearFlags = CameraClearFlags.Nothing; 
   //camera.enabled = false; 
} 

function Stop () { 
} 

function UpdateView() { 
   leftEye.camera.depth = GetComponent.<Camera>().depth -2; 
   rightEye.camera.depth = GetComponent.<Camera>().depth -1; 
    
   leftEye.transform.position = transform.position + transform.TransformDirection(-S3DV.eyeDistance, 0, 0); 
   rightEye.transform.position = transform.position + transform.TransformDirection(S3DV.eyeDistance, 0, 0); 
    
   leftEye.transform.rotation = transform.rotation; 
   rightEye.transform.rotation = transform.rotation; 
    
   leftEye.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DV.focalDistance)); 
   rightEye.transform.LookAt (transform.position + (transform.TransformDirection (Vector3.forward) * S3DV.focalDistance)); 
    
   leftEye.transform.parent = transform; 
   rightEye.transform.parent = transform; 
} 

function LateUpdate() { 
   UpdateView(); 
    
   if (enableKeys) { 
      // o and p 
      var eyeDistanceAdjust : float = 0.01; 
      if (Input.GetKeyDown(upEyeDistance)) { 
         S3DV.eyeDistance += eyeDistanceAdjust; 
      } else if (Input.GetKeyDown(downEyeDistance)) { 
         S3DV.eyeDistance -= eyeDistanceAdjust; 
      } 
       
      // k and l 
      var focalDistanceAdjust : float = 0.5; 
      if (Input.GetKeyDown(upFocalDistance)) { 
         //Debug.Log("focal up"); 
         S3DV.focalDistance += focalDistanceAdjust; 
      } else if (Input.GetKeyDown(downFocalDistance)) { 
         S3DV.focalDistance -= focalDistanceAdjust; 
      } 
   } 
} 

function OnRenderImage (source:RenderTexture, destination:RenderTexture) { 
   RenderTexture.active = destination; 
   GL.PushMatrix(); 
   GL.LoadOrtho(); 
   for(var i:int = 0; i < anaglyphMat.passCount; i++) { 
      anaglyphMat.SetPass(i); 
      DrawQuad(); 
   } 
   GL.PopMatrix(); 
} 
  
private function DrawQuad() { 
   GL.Begin (GL.QUADS);       
      GL.TexCoord2( 0.0, 0.0 ); GL.Vertex3( 0.0, 0.0, zvalue ); 
      GL.TexCoord2( 1.0, 0.0 ); GL.Vertex3( 1.0, 0.0, zvalue ); 
      GL.TexCoord2( 1.0, 1.0 ); GL.Vertex3( 1.0, 1.0, zvalue ); 
      GL.TexCoord2( 0.0, 1.0 ); GL.Vertex3( 0.0, 1.0, zvalue ); 
   GL.End(); 
} 