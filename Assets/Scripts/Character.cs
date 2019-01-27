using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour {

	bool snapped = false, resting = false, deathStarted = false, isEliminated = false;
	public int player;
	public Transform snapTransform, unSnapTransform;
	Kotatsu currentKotatsu;
	CharacterController SelfController;
	List<Interactor> interactors;
	Interactor closest;
	int points;
	float heat = 1.0f;
	public HeatBar heatBar;
	float originalY;
	Vector3 originalPosition;
	public TextMeshProUGUI pointsText;
	// Use this for initialization

	void Awake() {		
		originalY = transform.position.y;
		originalPosition = transform.position;
	}
	void Start () {
		SelfController = GetComponent<CharacterController>();
		interactors = new List<Interactor>();
		pointsText.text = "0 pts";
	}
	
	// Update is called once per frame
	void Update () {
		if(!IsDead()) {
			Vector3 moveDirection = new Vector3();
			moveDirection.x = Input.GetAxis("Horizontal" + player) * .5f;
			moveDirection.z = Input.GetAxis("Vertical" + player) * .5f;
			//Debug.Log(moveDirection);
			if(!resting && snapped && ControllerAtRest(moveDirection)) {
				resting = true;
			}
			if(resting && snapped && !ControllerAtRest(moveDirection)) {
				UnSnap();
			}
			if(!snapped) {
				SelfController.Move(moveDirection);
				float step = 5.0f * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, moveDirection, step, 0.0f);
				transform.rotation = Quaternion.LookRotation(newDir);
			}

			if(closest != null && Input.GetButtonDown("Interact" + player)) {
				Collect(closest);
			}
		}
		if(!snapped) {
			transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
		}
	}

	public void Snap(Transform snapTransform, Transform unSnapTransform, Kotatsu currentKotatsu) {
		if(!snapped) {
			this.currentKotatsu = currentKotatsu;
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
		currentKotatsu.LeaveKotatsu();
	}

	public bool IsSnapped() {
		return snapped;
	}

	bool ControllerAtRest(Vector3 moveDirection) {
		return moveDirection.x == 0 && moveDirection.z == 0;
	}

	public void AddInteractor(Interactor newInteractor) {
		interactors.Add(newInteractor);
		FindClosestInteractor();
	}

	public void RemoveInteractor(Interactor newInteractor) {
		interactors.Remove(newInteractor);
		newInteractor.DeSelect();
		FindClosestInteractor();
	}

	void FindClosestInteractor() {
		if(interactors.Count == 0) {
			closest = null;
		}
		else {
			closest = interactors[0];
			float distance = Vector3.Distance(transform.position, closest.transform.position);
			foreach(Interactor i in interactors) {
				float newDistance = Vector3.Distance(transform.position, i.transform.position);
				if(newDistance < distance) {
					closest = i;
				}
			}
			closest.Select();
		}
	}

	void Collect(Interactor i) {
		i.Collect();
		points += i.GetValue();
		pointsText.text = points + " pts";
		interactors.Remove(i);
		closest = null;
		FindClosestInteractor();
	}

	public int GetPlayerNumber() {
		return player;
	}

	public void Warm(float value) {
		heat+= value;
		AdjustHeat();
	}

	void AdjustHeat() {
		if(heat > 1.0f) heat = 1.0f;
		else if(heat < 0.0f) heat = 0.0f;
		heatBar.SetHeat(heat);
	}

	public bool IsDead() {
		return heat < 0.05;
	}

	public void StartDeath() {
		deathStarted = true;
	}

	public bool DeathStarted() {
		return deathStarted;
	}

	public void Eliminate() {
		isEliminated = true;
	}

	public bool IsEliminated() {
		return isEliminated;
	}

	public void Reset() {
		heat = 1.0f;
		transform.position = originalPosition;
		closest = null;
		interactors = new List<Interactor>();
		points = 0;		
		pointsText.text = points + " pts";
	}

	public int GetPoints() {
		return points;
	}

	public void Kill() {
		heat = 0.0f;
		UnSnap();
	}
}
