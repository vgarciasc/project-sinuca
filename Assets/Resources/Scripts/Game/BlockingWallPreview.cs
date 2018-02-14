using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingWallPreview : MonoBehaviour {
	[SerializeField]
	Color inactiveColor;
	[SerializeField]
	Color activeColor;
	[SerializeField]
	List<GameObject> inside = new List<GameObject>();
	
	MeshRenderer meshRenderer;

	void Start() {
		meshRenderer = GetComponentInChildren<MeshRenderer>();
	}

	void Update() {
		HandlePosition();
		HandleColor();
	}

	void HandlePosition() {
		RaycastHit hit;
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(mouseRay, out hit, (1 << LayerMask.NameToLayer("MouseClip")))) {
			this.transform.position = Vector3.ProjectOnPlane(hit.point, Vector3.up);
			this.transform.position += new Vector3(0, 1.5f, 0);
		}
	}

	void HandleColor() {
		meshRenderer.material.color = inside.Count == 0 ? activeColor : inactiveColor;
	}

	void OnTriggerEnter(Collider collider) {
		var obj = collider.gameObject;

		inside.Add(obj);
	}

	void OnTriggerExit(Collider collider) {
		var obj = collider.gameObject;

		if (inside.Contains(obj)) {
			inside.Remove(obj);
		}
	}
}
