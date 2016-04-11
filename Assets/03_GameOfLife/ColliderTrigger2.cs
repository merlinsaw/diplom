using UnityEngine;
using System.Collections;

 class ColliderTrigger2 : MonoBehaviour {
	public int height;
	static int count = 20;
	private CellState CellState;
	private int CellID;
	Transform Cell;
	public Material TestMaterial;

	public CellPosition _CellPosition;

	public enum CellPosition{
		Left,
		Right
	}
	// Use this for initialization
	void Start () {
		if (_CellPosition == CellPosition.Left){
			CellID = (int)(count * 0.25 * 3);
		}
		if (_CellPosition == CellPosition.Right){
			CellID = (int)(count * 0.25);
		
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
	private static GameObject GridLayer {
		get { return PrefabRepository.Instance.GridLayer; }
	}
	void OnTriggerEnter (Collider col){
		Debug.Log("Collision:" + col.gameObject.name + " - ");
		if (GridLayer.transform.childCount >= count){
		if (Cell == null){
			Cell = GridLayer.transform.GetChild((height+count*CellID));
			Cell.GetChild(0).GetComponent<Renderer>().material = TestMaterial;
			CellState = Cell.GetComponent<CellState>();
		}
		CellState.InstantTransition(true);


		//if (col != null){Debug.Log(col.gameObject.name);}
		}
	}

}
