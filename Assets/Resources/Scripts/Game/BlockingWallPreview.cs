using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingWallPreview : MonoBehaviour {
	[Header("Mechanics")]
	[SerializeField]
	Color inactiveColor;
	[SerializeField]
	Color activeColor;

	[Header("References")]
	List<GameObject> inside = new List<GameObject>();
	[SerializeField]
	GameObject blockingWallPrefab;

	MeshRenderer meshRenderer;
	public bool valid { get; private set; }

	void Start() {
		meshRenderer = GetComponentInChildren<MeshRenderer>();
	}

	void Update() {
		valid = inside.Count == 0;

		HandlePosition();
		HandleColor();
	}

	void HandlePosition() {
		RaycastHit hit;
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(mouseRay, out hit, (1 << LayerMask.NameToLayer("MouseClip")))) {
			this.transform.position = Vector3.ProjectOnPlane(hit.point, Vector3.up);
			this.transform.position += new Vector3(0, 1.8f, 0);
		}
	}

	void HandleColor() {
		meshRenderer.material.color = valid ? inactiveColor : activeColor;
	}

	void OnTriggerEnter(Collider collider) {
		var obj = collider.gameObject;

		if (obj.layer != LayerMask.NameToLayer("Table")) {
			inside.Add(obj);
		}
	}

	void OnTriggerExit(Collider collider) {
		var obj = collider.gameObject;

		if (inside.Contains(obj)) {
			inside.Remove(obj);
		}
	}

	public void InstantiatePreview() {
		var obj = Instantiate(blockingWallPrefab, this.transform.position, Quaternion.identity);
		obj.transform.position -= new Vector3(0, 0.2f, 0);
		Destroy(this.gameObject);
	}
}
