using UnityEngine;
using System.Collections;

class ColliderTrigger : MonoBehaviour {
	public int height;
	private CellState CellState;
	private int CellID;
	Transform Cell;
	public Material TestMaterial;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private static GameObject GridLayer {
		get { return PrefabRepository.Instance.GridLayer; }
	}
	void OnTriggerEnter (Collider col){
		Debug.Log("Collision:" + col.gameObject.name + " - ");
		if (GridLayer.transform.childCount > 0){
			if (Cell == null){
				Cell = this.transform;
				Cell.GetChild(0).GetComponent<Renderer>().material = TestMaterial;
				CellState = Cell.GetComponent<CellState>();
			}
			CellState.InstantTransition(true);
			
			
			//if (col != null){Debug.Log(col.gameObject.name);}
		}
	}
	
}
