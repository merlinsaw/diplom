using UnityEngine;
using System.Collections;

public class OffsetTexture : MonoBehaviour {

	public float scrollSpeed_x = 0.025F;
	public float scrollSpeed_y = 0.015F;
	public float initialOffest_x = 0.45F;
	public float initialOffest_y = 0.57F;
	public float initialPosition_x = 0.00F;
	public float initialPosition_y = 11.2F;
	public Renderer rend;
	public GameObject mirror;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		//mirror.transform.position.x
		float offset_x = initialOffest_x + mirror.transform.position.x * scrollSpeed_x; //Time.time * scrollSpeed;
		float offset_y = initialOffest_y + (mirror.transform.position.y - initialPosition_y) * scrollSpeed_y;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offset_x, offset_y));
	}
}
