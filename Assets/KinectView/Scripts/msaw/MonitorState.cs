using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MonitorState : NetworkBehaviour {
	[SyncVar]
	public AI 		_Behavior;
	[SyncVar(hook="loadFaceImages")]
	public Show 	_Display;
	[SyncVar]
	public Track 	_Tracking;
	[SyncVar]
	public Move 	_Movement;
	[SyncVar]
	public Location _Locating;

	FaceTextureAnimation _FaceTextureAnimation;

	void loadFaceImages(Show newState){
		print ("loadFaceImages");
		_FaceTextureAnimation = gameObject.GetComponent<FaceTextureAnimation>();
		_FaceTextureAnimation.doLoadFaceImages(newState);
	}

	public enum AI{
		SetToSleep,
		Sleeping,
		Awake,
		Idle,
		Engage,
		Mimic,
		Return,
		Showcase
	}

	public enum Show{
		Nothing,
		StartLoading,
		Loading,
		LoadingDone,
		FaceAnimation,
		Stillframe
	}

	public enum Track{
		Nothing,
		Searching,
		Body
	}

	public enum Move{
		StayInRig,
		Idle,
		Follow,
		ReturnToRig
	}

	public enum Location{
		InBack,
	 	InFront

	}

}
