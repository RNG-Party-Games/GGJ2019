using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kotatsu : MonoBehaviour {
	
	
	public Transform snapTransform, unSnapTransform;
	public Character startingPlayer;
	bool inUse = false;
	// Use this for initialization
	void Start () {
		if(startingPlayer.gameObject.activeInHierarchy) {
			startingPlayer.Snap(snapTransform, unSnapTransform, this);
			inUse = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Player" && !inUse) {
			inUse = true;
			Character player = collider.GetComponent<Character>();
			if(!player.IsSnapped()) {
				Debug.Log("Snapping");
				player.Snap(snapTransform, unSnapTransform, this);
			}
		}
	}

	public void LeaveKotatsu() {
		inUse = false;
	}

	public void Reset() {		
		if(startingPlayer.gameObject.activeInHierarchy) {
			startingPlayer.Snap(snapTransform, unSnapTransform, this);
			inUse = true;
		}
	}
}
