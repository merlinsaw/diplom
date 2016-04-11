using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ShadowLine : NetworkBehaviour {
	static float offScene = -1000;
	static Vector3 EmptyPos = new Vector3(offScene,offScene,offScene);
	public Vector3 CurrentCellPos = EmptyPos;
	private Vector3 LastCellPos = EmptyPos;

	//private LineRenderer[] lines = new LineRenderer[8];
	public LineRenderer CellLine;
	private int LineIdx = 1;

	// Use this for initialization
	void Start () {
		CellLine = this.transform.GetComponent<LineRenderer>();
		CellLine.SetVertexCount(8);
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentCellPos == EmptyPos){
			Debug.Log("CurrentCellPos not set");
		}else{
			// we have the point first time
			if (LastCellPos == EmptyPos){
				// fist point do nothing but store the value in last point
				Debug.Log("first point" + CurrentCellPos);
				LastCellPos = CurrentCellPos;
				CellLine.SetPosition(0, LastCellPos);
			}
			if (LastCellPos != CurrentCellPos && LastCellPos != EmptyPos){
				// we have the point again
				Debug.Log("current: " + CurrentCellPos + " last : " + LastCellPos);
				// if defferent and not empty than draw line
				// lines[0] = Instantiate(CellLine) as LineRenderer;
				//lines[0] = NetworkServer.Spawn(CellLine)as LineRenderer;
				//lines[0].transform.SetParent(this.transform);
				//lines[i].transform.parent = transform;
				//lines[i].SetVertexCount(2);
				//lines[0].SetPosition(0, LastCellPos);
				//lines[0].SetPosition(1, CurrentCellPos);
				CellLine.SetPosition(LineIdx, CurrentCellPos);
				LineIdx++;
				// alfter line drawn set current point to last point
				LastCellPos = CurrentCellPos;
				// and set current point to empty
			}






//			if (LastCellPos != CurrentCellPos){
//				Debug.Log("first point" + CurrentCellPos);
//				LastCellPos = CurrentCellPos;
//			}else{
//				// do something
//				Debug.Log("current: " + CurrentCellPos + " last : " + LastCellPos);
//			}
	
		}
	}
}

// 

