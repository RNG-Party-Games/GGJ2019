using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	bool snapped = true, resting = false;
	public Transform snapTransform, unSnapTransform;
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
		if(moveDirection == Vector3.zero) {
			resting = true;
		}
		if(resting && snapped && moveDirection != Vector3.zero) {
			UnSnap();
		}
		if(!snapped) {
			SelfController.Move(moveDirection);
			float step = 5.0f * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(transform.forward, moveDirection, step, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
	}

	public void Snap(Transform snapTransform, Transform unSnapTransform) {
		if(!snapped) {
			snapped = true;
			this.snapTransform = snapTransform;
			this.unSnapTransform = unSnapTransform;
			transform.position = snapTransform.position;
		}
	}

	public void UnSnap() {
		snapped = false;
		resting = false;
		transform.position = unSnapTransform.position;
	}

	public bool IsSnapped() {
		return snapped;
	}
}
