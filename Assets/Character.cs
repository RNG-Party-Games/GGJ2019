using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	CharacterController SelfController;
	// Use this for initialization
	void Start () {
		SelfController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 moveDirection = new Vector3();
		Vector3 rotateDirection = transform.rotation.eulerAngles;
		moveDirection.x = Input.GetAxis("Horizontal") * .5f;
		moveDirection.z = Input.GetAxis("Vertical") * .5f;
		SelfController.Move(moveDirection);
        float step = 5.0f * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, moveDirection, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
	}
}
