using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FaceTextureAnimation : NetworkBehaviour {
	[SyncVar]
	public bool PlayAnimation = false;
	[SyncVar]
	public int RigSlot;

	public int MaximumFaceImages = 120;
	private Texture2D[] FaceFramesArray = new Texture2D[120];

	//[SyncVar(hook="doLoadFaceImages")]
	private MonitorState _MonitorStates;

//	private float framesPerSecond = 40.0F;
	[SyncVar]
	private float indexF = 0.0F;
	public float indexFIncrease = 1.5F;
	private int MaximumFaceLoops = 5;
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		_MonitorStates = gameObject.GetComponent<MonitorState>();
		if (_MonitorStates == null)
		{
			return;
		}
		AnimateFaceFrame();
	}

	public void doLoadFaceImages(MonitorState.Show newDisplayState){
		print ("popo kakak " + _MonitorStates._Display + " "+ newDisplayState);
		if (newDisplayState == MonitorState.Show.StartLoading){
			StartCoroutine("ReadTextureFromFile",MaximumFaceImages);
			_MonitorStates._Display = MonitorState.Show.Loading;
		}
		if (newDisplayState == MonitorState.Show.FaceAnimation){
			PlayAnimation = true;
			//AnimateFaceFrame();
		}
		if (newDisplayState == MonitorState.Show.Stillframe){
			FaceFramesArray = null;
		}
	}


	//the images are loaded in the FaceFramesArray
	IEnumerator ReadTextureFromFile(int maximumFaceImages)
	{
		FaceFramesArray = new Texture2D[maximumFaceImages];
		// To get image
		for (int imageNumber = 1; imageNumber <= maximumFaceImages; imageNumber++){
			print (imageNumber);
			var path = System.IO.Path.Combine ("Z:\\Faces\\face ("+RigSlot+")", "image_"+ imageNumber + ".png");
			var bytesRead = System.IO.File.ReadAllBytes (path);
			Texture2D myTexture = new Texture2D (32, 32);
			myTexture.LoadImage (bytesRead);
			FaceFramesArray[imageNumber-1] = myTexture;
			//yield return new WaitForSeconds(.001f);
			yield return new WaitForSeconds(.111f);
		}
		_MonitorStates._Display = MonitorState.Show.LoadingDone;
		print ("laoding done");
		_MonitorStates._Display = MonitorState.Show.FaceAnimation;
	}



	private bool backwards = false;
	int loopFaceNumberOfTimes = 0;

	void AnimateFaceFrame() {
		if (!PlayAnimation){return;}
		// flip the texture
		//_Monitor.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(-1, -1));
		//int faceFramesIndex = (int)(Time.time * framesPerSecond);
		//kaka
		if (_MonitorStates._Display == MonitorState.Show.FaceAnimation){
			if (isServer){
				if (backwards) {
					indexF -= indexFIncrease;
				}
				if (indexF < 0.0F) {
					indexF = 0.0F;
					backwards = false;
				}
				if (!backwards) {
					indexF += indexFIncrease;
				}
				if (indexF > FaceFramesArray.Length-1) {
					indexF = FaceFramesArray.Length-1;
					backwards = true;
					if (_MonitorStates._Behavior == MonitorState.AI.Engage){
						loopFaceNumberOfTimes += 1;
					}
				}
				//When the loop played the amount of loopFaceNumberOfTimes back and forth 
				//the monitor will return to its Rig position and keep showcasing a face still frame
				if (loopFaceNumberOfTimes >= MaximumFaceLoops){
					_MonitorStates._Behavior = MonitorState.AI.Return;
					_MonitorStates._Display = MonitorState.Show.Stillframe;
					loopFaceNumberOfTimes = 0;
					PlayAnimation = false;
				}
			}
			int faceFramesIndex = ((int)indexF);// % FaceFramesArray.Length;
			//print (faceFramesIndex);
			if (PlayAnimation){
				gameObject.transform.GetChild(0).GetChild(6).GetComponent<Renderer>().material.mainTexture = FaceFramesArray[faceFramesIndex];
			}
		}
	}


}
