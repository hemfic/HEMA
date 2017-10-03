using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour {
	public float speed;
	public float torque;
	public Vector3 origin;
	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
		origin = rb.position;
	}

	void FixedUpdate () {
		float reset = Input.GetAxis ("Cancel");

		//if (moveUp!=0.0f)
		if (reset != 0)
			rb.position = origin;
	}
}
