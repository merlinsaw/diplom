using UnityEngine;
using System.Collections;
public class ShakeCamera : MonoBehaviour {
	private Quaternion r;
	public float decay = .012f, intensity = .12f, i;
	
	void FixedUpdate () {
		if (i > 0) {
			transform.rotation = new Quaternion(
				r.x, 
				r.y + Random.Range(-intensity, intensity) * .2f, 
				r.z, 
				r.w);
			i -= decay;
		}
	}
	
	public void Shake() {
		r = transform.rotation;
		i = intensity;
	}
}