using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;

	private Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate ()
	{
		float moveX = Input.GetAxis ("Horizontal");
		float moveY = Input.GetAxis ("Vertical");
		float moveUp = Input.GetAxis ("Fire1");
		float moveDown = Input.GetAxis ("Fire2");
		Vector3 movement = new Vector3 (moveX, 0.0f, moveY);
		if (moveUp!=0.0f)
			movement=new Vector3 (moveX, moveUp, moveY);
		else if (moveDown!=0.0f)
			movement=new Vector3 (moveX, -1.0f*moveDown, moveY);

		rb.AddForce (movement * speed);
	}
}