using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class crop : MonoBehaviour {
	public GameObject cam;
	Renderer rend;
	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {
		rend.material.SetVector("_ZeroParalax",cam.transform.position);
		rend.material.SetFloat("_x",cam.transform.position.x);
		rend.material.SetFloat("_y",cam.transform.position.y);
		rend.material.SetFloat("_z",cam.transform.position.z);
		//Debug.LogError(""
		//transform.GetComponent<Material>().GetVector("ZeroParalax")

			//);
	}
}
