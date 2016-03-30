using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class QuadMapping : NetworkBehaviour
{
	public bool UseClientCamInstead = false;
	public bool ShowGUI = true;

	float lowerLeft_x 	= 0.0F;
	float lowerLeft_y 	= 0.0F;
	float lowerRight_x 	= 0.0F;
	float lowerRight_y 	= 0.0F;
	float upperRight_x 	= 0.0F;
	float upperRight_y 	= 0.0F;
	float upperLeft_x 	= 0.0F;
	float upperLeft_y 	= 0.0F;

	float Client_lowerLeft_x 	= 0.0F;
	float Client_lowerLeft_y 	= 0.0F;
	float Client_lowerRight_x 	= 1.0F;
	float Client_lowerRight_y 	= 0.0F;
	float Client_upperRight_x 	= 1.0F;
	float Client_upperRight_y 	= 1.0F;
	float Client_upperLeft_x 	= 0.0F;
	float Client_upperLeft_y 	= 1.0F;

	float Server_lowerLeft_x 	= 0.0F;
	float Server_lowerLeft_y 	= 0.0F;
	float Server_lowerRight_x 	= 1.0F;	
	float Server_lowerRight_y 	= 0.0F;	
	float Server_upperRight_x 	= 1.0F;	
	float Server_upperRight_y 	= 1.0F;	
	float Server_upperLeft_x 	= 0.0F;
	float Server_upperLeft_y 	= 1.0F;

	// arrow navi buttens
	float x_offset_left = 0;
	float x_offset_right = 100;
	float incement = 0.001F;
	bool isServer = true;

	public GameObject clientCam;
	public GameObject serverCam;

	// Use this for initialization
	void Start ()
	{
		Client_lowerLeft_x = clientCam.transform.GetComponent<StereovisionMasked>().lowerLeft_x;
		Client_lowerLeft_y = clientCam.transform.GetComponent<StereovisionMasked>().lowerLeft_y; 
		Client_lowerRight_x = clientCam.transform.GetComponent<StereovisionMasked>().lowerRight_x;
		Client_lowerRight_y = clientCam.transform.GetComponent<StereovisionMasked>().lowerRight_y;
		Client_upperRight_x = clientCam.transform.GetComponent<StereovisionMasked>().upperRight_x;
		Client_upperRight_y = clientCam.transform.GetComponent<StereovisionMasked>().upperRight_y;
		Client_upperLeft_x = clientCam.transform.GetComponent<StereovisionMasked>().upperLeft_x;
		Client_upperLeft_y = clientCam.transform.GetComponent<StereovisionMasked>().upperLeft_y;

		Server_lowerLeft_x = serverCam.transform.GetComponent<Stereovision>().lowerLeft_x;
		Server_lowerLeft_y = serverCam.transform.GetComponent<Stereovision>().lowerLeft_y; 
		Server_lowerRight_x = serverCam.transform.GetComponent<Stereovision>().lowerRight_x;
		Server_lowerRight_y = serverCam.transform.GetComponent<Stereovision>().lowerRight_y;
		Server_upperRight_x = serverCam.transform.GetComponent<Stereovision>().upperRight_x;
		Server_upperRight_y = serverCam.transform.GetComponent<Stereovision>().upperRight_y;
		Server_upperLeft_x = serverCam.transform.GetComponent<Stereovision>().upperLeft_x;
		Server_upperLeft_y = serverCam.transform.GetComponent<Stereovision>().upperLeft_y;

		if (GetComponent<NetworkIdentity>().isServer){
			print("Running as a server");
			if (UseClientCamInstead){
				clientCam.SetActive(true);
				serverCam.SetActive(false);
			}else{
			clientCam.SetActive(false);
			serverCam.SetActive(true);
			}
		}
		else if (GetComponent<NetworkIdentity>().isClient){
			print("Running as a client");
			clientCam.SetActive(true);
			serverCam.SetActive(false);
		}
		
	}

	void OnGUI()
	{
		if (ShowGUI){
		// togle between server cam and client cam
		if (GUI.Button(new Rect((Screen.width/2), 30, 50, 30), "toggle")){
			isServer = !isServer; 
		}
		if (isServer){
			GUI.Label(new Rect((Screen.width/2), 60, 80, 20), "ServerCam");
			//Debug.Log("is Server");
		} else {
			GUI.Label(new Rect((Screen.width/2), 60, 80, 20), "ClientCam");
			//Debug.Log("is Client");
		}

		/// <description>
		/// lower left corner -----------------------------------------------------------
		/// </description>
		if (GUI.Button(new Rect(0+x_offset_left, Screen.height-60, 50, 30), "left")){
			lowerLeft_x -= incement;
			Debug.Log("lowerLeft_x = " + lowerLeft_x);
			if (isServer){Server_lowerLeft_x -= incement;}else{Client_lowerLeft_x -= incement;}
		}
		if (GUI.Button(new Rect(100+x_offset_left, Screen.height-60, 50, 30), "right")){
			lowerLeft_x += incement;
			Debug.Log("lowerLeft_x = " + lowerLeft_x);
			if (isServer){Server_lowerLeft_x += incement;}else{Client_lowerLeft_x += incement;}
		}
		if (GUI.Button(new Rect(50+x_offset_left, Screen.height-30, 50, 30), "down")){
			lowerLeft_y -= incement;
			Debug.Log("lowerLeft_y = " + lowerLeft_y);
			if (isServer){Server_lowerLeft_y -= incement;}else{Client_lowerLeft_y -= incement;}
		}
		if (GUI.Button(new Rect(50+x_offset_left, Screen.height-90, 50, 30), "up")){
			lowerLeft_y += incement;
			Debug.Log("lowerLeft_y = " + lowerLeft_y);
			if (isServer){Server_lowerLeft_y += incement;}else{Client_lowerLeft_y += incement;}
		}

		/// <description>
		/// lower right corner
		/// </description>
		if (GUI.Button(new Rect((Screen.width-150)+0-x_offset_right, Screen.height-60, 50, 30), "left")){
			lowerRight_x -= incement;
			Debug.Log("lowerRight_x = " + lowerRight_x);
			if (isServer){Server_lowerRight_x -= incement;}else{Client_lowerRight_x -= incement;}
		}
		if (GUI.Button(new Rect((Screen.width-150)+100-x_offset_right, Screen.height-60, 50, 30), "right")){
			lowerRight_x += incement;
			Debug.Log("lowerRight_x = " + lowerRight_x);
			if (isServer){Server_lowerRight_x += incement;}else{Client_lowerRight_x += incement;}
		}
		if (GUI.Button(new Rect((Screen.width-150)+50-x_offset_right, Screen.height-30, 50, 30), "down")){
			lowerRight_y -= incement;
			Debug.Log("lowerRight_y = " + lowerRight_y);
			if (isServer){Server_lowerRight_y -= incement;}else{Client_lowerRight_y -= incement;}
		}
		if (GUI.Button(new Rect((Screen.width-150)+50-x_offset_right, Screen.height-90, 50, 30), "up")){
			lowerRight_y += incement;
			Debug.Log("lowerRight_y = " + lowerRight_y);
			if (isServer){Server_lowerRight_y += incement;}else{Client_lowerRight_y += incement;}
		}

		/// <description>
		/// upper right corner
		/// </description>
		if (GUI.Button(new Rect((Screen.width-150)+0-x_offset_right, 30, 50, 30), "left")){
			upperRight_x -= incement;
			Debug.Log("upperRight_x = " + upperRight_x);
			if (isServer){Server_upperRight_x -= incement;}else{Client_upperRight_x -= incement;}
		}
		if (GUI.Button(new Rect((Screen.width-150)+100-x_offset_right, 30, 50, 30), "right")){
			upperRight_x += incement;
			Debug.Log("upperRight_x = " + upperRight_x);
			if (isServer){Server_upperRight_x += incement;}else{Client_upperRight_x += incement;}
		}
		if (GUI.Button(new Rect((Screen.width-150)+50-x_offset_right, 60, 50, 30), "down")){
			upperRight_y -= incement;
			Debug.Log("upperRight_x = " + upperRight_y);
			if (isServer){Server_upperRight_y -= incement;}else{Client_upperRight_y -= incement;}
		}
		if (GUI.Button(new Rect((Screen.width-150)+50-x_offset_right, 0, 50, 30), "up")){
			upperRight_y += incement;
			Debug.Log("upperRight_x = " + upperRight_y);
			if (isServer){Server_upperRight_y += incement;}else{Client_upperRight_y += incement;}
		}

		/// <description>
		/// upper left corner
		/// </description>
		if (GUI.Button(new Rect(0+x_offset_left, 30, 50, 30), "left")){
			upperLeft_x -= incement;
			Debug.Log("upperLeft_x = " + upperLeft_x);
			if (isServer){Server_upperLeft_x -= incement;}else{Client_upperLeft_x -= incement;}
		}
		if (GUI.Button(new Rect(100+x_offset_left, 30, 50, 30), "right")){
			upperLeft_x += incement;
			Debug.Log("upperLeft_x = " + upperLeft_x);
			if (isServer){Server_upperLeft_x += incement;}else{Client_upperLeft_x += incement;}
		}
		if (GUI.Button(new Rect(50+x_offset_left, 60, 50, 30), "down")){
			upperLeft_y -= incement;
			Debug.Log("upperLeft_y = " + upperLeft_y);
			if (isServer){Server_upperLeft_y -= incement;}else{Client_upperLeft_y -= incement;}
		}
		if (GUI.Button(new Rect(50+x_offset_left, 0, 50, 30), "up")){
			upperLeft_y += incement;
			Debug.Log("upperLeft_y = " + upperLeft_y);
			if (isServer){Server_upperLeft_y += incement;}else{Client_upperLeft_y += incement;}
		}

		/// <description>
		/// numeric inputs ---------------------------------------------------
		/// </description>

		if (isServer == true){
			GUI.Label (new Rect(30, 180, 80, 20), "Lower Left");
			GUI.Label (new Rect(30, 200, 40, 20), string.Format ("{0:0.000}" ,Server_lowerLeft_x));
			GUI.Label (new Rect(70, 200, 40, 20), string.Format ("{0:0.000}" ,Server_lowerLeft_y));
			GUI.Label (new Rect(30, 220, 80, 20), "Lower Right");
	       	GUI.Label (new Rect(30, 240, 40, 20), string.Format ("{0:0.000}" ,Server_lowerRight_x));
	       	GUI.Label (new Rect(70, 240, 40, 20), string.Format ("{0:0.000}" ,Server_lowerRight_y));
			GUI.Label (new Rect(30, 260, 80, 20), "Upper Right");
	       	GUI.Label (new Rect(30, 280, 40, 20), string.Format ("{0:0.000}" ,Server_upperRight_x));
	       	GUI.Label (new Rect(70, 280, 40, 20), string.Format ("{0:0.000}" ,Server_upperRight_y));
			GUI.Label (new Rect(30, 300, 80, 20), "Upper Left");
	       	GUI.Label (new Rect(30, 320, 40, 20), string.Format ("{0:0.000}" ,Server_upperLeft_x));
	       	GUI.Label (new Rect(70, 320, 40, 20), string.Format ("{0:0.000}" ,Server_upperLeft_y));
		}
		if (isServer == false){
			GUI.Label (new Rect(30, 180, 80, 20), "Lower Left");
			GUI.Label (new Rect(30, 200, 40, 20), string.Format ("{0:0.000}" ,Client_lowerLeft_x));
			GUI.Label (new Rect(70, 200, 40, 20), string.Format ("{0:0.000}" ,Client_lowerLeft_y));
			GUI.Label (new Rect(30, 220, 80, 20), "Lower Right");
			GUI.Label (new Rect(30, 240, 40, 20), string.Format ("{0:0.000}" ,Client_lowerRight_x));
			GUI.Label (new Rect(70, 240, 40, 20), string.Format ("{0:0.000}" ,Client_lowerRight_y));
			GUI.Label (new Rect(30, 260, 80, 20), "Upper Right");
			GUI.Label (new Rect(30, 280, 40, 20), string.Format ("{0:0.000}" ,Client_upperRight_x));
			GUI.Label (new Rect(70, 280, 40, 20), string.Format ("{0:0.000}" ,Client_upperRight_y));
			GUI.Label (new Rect(30, 300, 80, 20), "Upper Left");
			GUI.Label (new Rect(30, 320, 40, 20), string.Format ("{0:0.000}" ,Client_upperLeft_x));
			GUI.Label (new Rect(70, 320, 40, 20), string.Format ("{0:0.000}" ,Client_upperLeft_y));
		}
		
		
		//		GUI.Label(new Rect(30, 180, 80, 20), "Lower Left");
//		S_lowerLeft_x = GUI.TextField(new Rect(30, 200, 40, 20), S_lowerLeft_x, 6);
//		S_lowerLeft_y = GUI.TextField(new Rect(70, 200, 40, 20), S_lowerLeft_y, 6);
//		GUI.Label(new Rect(30, 220, 80, 20), "Lower Right");
//		S_lowerRight_x = GUI.TextField(new Rect(30, 240, 40, 20), S_lowerRight_x, 6);
//		S_lowerRight_y = GUI.TextField(new Rect(70, 240, 40, 20), S_lowerRight_y, 6);
//		GUI.Label(new Rect(30, 260, 80, 20), "Upper Right");
//		S_upperRight_x = GUI.TextField (new Rect(30, 280, 40, 20), S_upperRight_x , 6);
//		S_upperRight_y = GUI.TextField (new Rect(70, 280, 40, 20), S_upperRight_y , 6);
//		GUI.Label(new Rect(30, 300, 80, 20), "Upper Left");
//		S_upperLeft_x = GUI.TextField (new Rect(30, 320, 40, 20), S_upperLeft_x , 6);
//		S_upperLeft_y = GUI.TextField (new Rect(70, 320, 40, 20), S_upperLeft_y , 6);
//		
//		if (S_lowerLeft_x != ""){
//			lowerLeft_x = float.Parse(S_lowerLeft_x);
//		}
//		if (S_lowerLeft_y != ""){
//			lowerLeft_y = float.Parse(S_lowerLeft_y);
//		}
//		if (S_lowerRight_x != ""){
//			lowerRight_x = float.Parse(S_lowerRight_x);
//		}
//		if (S_lowerRight_y != ""){
//			lowerRight_y = float.Parse(S_lowerRight_y);
//		}
//		if (S_upperRight_x != ""){
//			upperRight_x = float.Parse(S_upperRight_x);
//		}
//		if (S_upperRight_y != ""){
//			upperRight_y = float.Parse(S_upperRight_y);
//		}
//		if (S_upperLeft_y != ""){
//			upperLeft_x = float.Parse(S_upperLeft_x);
//		}
//		if (S_upperLeft_y != ""){
//			upperLeft_y = float.Parse(S_upperLeft_y);
//		}
		}
	}


	// Update is called once per frame
	void Update ()
	{
		if (GetComponent<NetworkIdentity>().isServer)
		{
			// client camera //string.Format ("{0:0.000}" ,Client_lowerLeft_x);
			if (isServer == false){
				clientCam.transform.GetComponent<StereovisionMasked>().lowerLeft_x = Client_lowerLeft_x; 
				clientCam.transform.GetComponent<StereovisionMasked>().lowerLeft_y = Client_lowerLeft_y; 
				clientCam.transform.GetComponent<StereovisionMasked>().lowerRight_x = Client_lowerRight_x; 
				clientCam.transform.GetComponent<StereovisionMasked>().lowerRight_y = Client_lowerRight_y; 
				clientCam.transform.GetComponent<StereovisionMasked>().upperRight_x = Client_upperRight_x; 
				clientCam.transform.GetComponent<StereovisionMasked>().upperRight_y = Client_upperRight_y;
				clientCam.transform.GetComponent<StereovisionMasked>().upperLeft_x = Client_upperLeft_x; 
				clientCam.transform.GetComponent<StereovisionMasked>().upperLeft_y = Client_upperLeft_y; 
			}
			// server camera
			if (isServer == true){
				//serverCam
				serverCam.transform.GetComponent<Stereovision>().lowerLeft_x = Server_lowerLeft_x; 
				serverCam.transform.GetComponent<Stereovision>().lowerLeft_y = Server_lowerLeft_y; 
				serverCam.transform.GetComponent<Stereovision>().lowerRight_x = Server_lowerRight_x; 
				serverCam.transform.GetComponent<Stereovision>().lowerRight_y = Server_lowerRight_y; 
				serverCam.transform.GetComponent<Stereovision>().upperRight_x = Server_upperRight_x; 
				serverCam.transform.GetComponent<Stereovision>().upperRight_y = Server_upperRight_y;
				serverCam.transform.GetComponent<Stereovision>().upperLeft_x = Server_upperLeft_x; 
				serverCam.transform.GetComponent<Stereovision>().upperLeft_y = Server_upperLeft_y; 
			}
		}
	}


	public string S_lowerLeft_x 	= "0.205"; //0.214 //0.0 * # # #
	public string S_lowerLeft_y 	= "0.015"; //0.006 //0.0 # * # #
	public string S_lowerRight_x 	= "0.745";//0.746 //1.0 # # * #
	public string S_lowerRight_y 	= "0.03"; //0.02 //0.0  # # # *
	public string S_upperRight_x 	= "0.98"; // //1.0		 - - - *
	public string S_upperRight_y 	= "0.98"; // //1.0		 - - - *
	public string S_upperLeft_x 	= "0.205"; // //1.0		 - * - -
	public string S_upperLeft_y 	= "0.97"; // //1.0		 - * - -
}








