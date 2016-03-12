using UnityEngine;
using System.Collections;

public class RecordingGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	float x_offset_left = 0;
	float x_offset_right = 100;
	void OnGUI(){
		if (GUI.Button(new Rect((Screen.width-150)+0-x_offset_right, Screen.height-60, 150, 30), "Record")){
			//this.transform.GetComponent<"KinectRecorderPlayer">() StartRecording() = true;
		}
	}
}
