using UnityEngine;
using System.Collections;

public class random_mesh : MonoBehaviour {
	public Mesh[] Meshes;
	private int random;
	// Use this for initialization
	void Start () {
		random = Random.Range(0, 5);
		//GetComponent<MeshFilter>().mesh = Meshes[random];
		GetComponent<MeshFilter>().mesh = Meshes[0];
		//transform.Rotate(Random.Range(0, 360),Random.Range(0, 360),Random.Range(0, 360));
		//transform.Rotate(Random.Range(0, 360),0,0);
		//transform.Rotate(45,45,45);
	}
	// Update is called once per frame
	void Update () {


	}

}
