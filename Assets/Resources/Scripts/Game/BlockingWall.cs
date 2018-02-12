﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockingWall : MonoBehaviour {

	bool active;
	List<GameObject> inside = new List<GameObject>();

	[Header("Colors")]
	[SerializeField]
	Color activeColor;
	[SerializeField]
	Color inactiveColor;

	[Header("References")]
	[SerializeField]
	Collider wallCollider;
	[SerializeField]
	MeshRenderer meshRenderer;

	[Header("Mechanics")]
	[SerializeField]
	[Range(0f, 2f)]
	float moveDistance = 0.6f;
	[SerializeField]
	float moveDuration = 0.25f;

	Vector3 upPosition;
	Vector3 downPosition;
	
	void Start() {
		upPosition = this.transform.localPosition;
		downPosition = this.transform.localPosition - Vector3.up * moveDistance;

		ToggleWall(false);
	}

	void OnTriggerEnter(Collider collider) {
		if (!active) {
			inside.Add(collider.gameObject);
		} else {
			ToggleWall(false);
		}
	}

	void OnTriggerExit(Collider collider) {
		var obj = collider.gameObject;

		if (!active && inside.Contains(obj)) {
			inside.Remove(obj);

			if (inside.Count == 0) {
				ToggleWall(true);
			}		
		}
	}

	void ToggleWall(bool value) {
		active = value;
		wallCollider.enabled = value;
		meshRenderer.material.DOColor(
			value ? activeColor : inactiveColor, 
			moveDuration);
		this.transform.DOLocalMove(
			value ? upPosition : downPosition,
			moveDuration);
	}
}