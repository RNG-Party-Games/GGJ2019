using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kotatsu : MonoBehaviour {
	
	
	public Transform unSnapTransform;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		Debug.Log(collider.name);
		if(collider.tag == "Player") {
			Character player = collider.GetComponent<Character>();
			if(!player.IsSnapped()) {
				Debug.Log("Snapping");
				player.Snap(transform, unSnapTransform);
			}
		}
	}
}
