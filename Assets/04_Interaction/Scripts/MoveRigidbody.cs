using UnityEngine;
using System.Collections;

public class MoveRigidbody : MonoBehaviour {
	public bool Movement = false;
	public GameObject Target;
	private Vector3 TargetPos;
	private Rigidbody rigidbody;
	private Vector3 velocity = Vector3.zero;
	public bool antiMovement = false;
	public float ScaleFactorX = 1.0f;
	// Use this for initialization
	void Start () {
		rigidbody = this.transform.GetComponent<Rigidbody>();
	}

	public float toVel = 2.5f;
	public float maxVel = 15.0f;
	public float maxForce = 40.0f;
	public float gain = 5f;

	void FixedUpdate(){
		if (!Movement){
			rigidbody.velocity = Vector3.zero;
		}else{
			if (Target != null){
				if (antiMovement){
					TargetPos = new Vector3 (Target.transform.position.x*-1f*ScaleFactorX,0,Target.transform.position.z);
				}else{
			TargetPos = new Vector3 (Target.transform.position.x,0,Target.transform.position.z);
				}
			Vector3 dist = TargetPos - transform.position;
			dist.y = 0; // ignore height differences
			// calc a target vel proportional to distance (clamped to maxVel)
			Vector3 tgtVel = Vector3.ClampMagnitude(toVel * dist, maxVel);
			// calculate the velocity error
			Vector3 error = tgtVel - rigidbody.velocity;
			// calc a force proportional to the error (clamped to maxForce)
			Vector3 force = Vector3.ClampMagnitude(gain * error, maxForce);
			rigidbody.AddForce(force);
			}
		}
	}
}
