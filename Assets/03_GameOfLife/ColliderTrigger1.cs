
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

class ColliderTrigger1 : NetworkBehaviour {
	Transform Cell;
	[SyncVar]
	public bool visible = true;
	public GameObject ShadowLine;
	
	
	// Use this for initialization
	void Start () {
		Cell = this.transform;
		Cell.GetComponentInChildren<MeshRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!visible){
			//Cell.GetChild(0).GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
		}
	}
	//private static GameObject GridLayer {
	//	get { return PrefabRepository.Instance.GridLayer; }
	//}
	void OnTriggerEnter (Collider col){
		Debug.Log("Collision:" + col.gameObject.name + " - " + Cell.gameObject.name);
		visible = false;
		//ShadowLine.GetComponent<ShadowLine>().CurrentCellPos = Cell.position;
		Cell.GetComponent<BoxCollider>().enabled = false;
		Cell.GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncTransform;
		Cell.GetComponentInChildren<MeshRenderer>().enabled = true;
		Cell.SetParent(col.transform);
		//if (GridLayer.transform.childCount > 0){
		//if (Cell == null){

		//Cell.GetChild(0).GetComponent<Renderer>().material = TestMaterial;
		//CellState = Cell.GetComponent<CellState>();

		//}
		//CellState.InstantTransition(true);
		
		
		//if (col != null){Debug.Log(col.gameObject.name);}
		//}
	}
	
}



