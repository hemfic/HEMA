using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	public float torque;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{
		float reset = Input.GetAxis ("Cancel");
		float moveX = Input.GetAxis ("Horizontal");
		float moveY = Input.GetAxis ("Vertical");
		float moveUp = Input.GetAxis ("Fire1");
		float moveDown = Input.GetAxis ("Fire2");
		float spinX = Input.GetAxis ("Jump");
		float spinY = Input.GetAxis ("Mouse X");
		float spinZ = Input.GetAxis ("Mouse Y");
		Vector3 movement = new Vector3 (moveX, 0.0f, moveY);
		Vector3 origin = new Vector3 (0.0f,0.0f,0.0f);
		if (moveUp!=0.0f)
			movement=new Vector3 (moveX, moveUp, moveY);
		else if (moveDown!=0.0f)
			movement=new Vector3 (moveX, -1.0f*moveDown, moveY);
		print (reset);
		if (reset != 0)
			rb.position = origin;

		rb.AddForce (movement * speed);
		rb.AddTorque (transform.up * torque * spinX);
		rb.AddTorque (transform.right * torque * spinY);
		rb.AddTorque (transform.forward * torque * spinZ);
	}
}