using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AcceleratorPad : MonoBehaviour, SelectableObstacle {

	[Header("References")]
	[SerializeField]
	MeshRenderer meshRenderer;

	[Header("Mechanics")]
	[SerializeField]
	[Range(0f, 10f)]
	float intensity = 5f;

	Color originalColor;

	void Start() {
		originalColor = meshRenderer.material.color;
	}

	void OnTriggerEnter(Collider collider) {
		var obj = collider.gameObject;
		if (obj.tag == "Ball") {
			var ball = obj.GetComponentInChildren<Ball>();
			ball.Boost(intensity);
		}
	}

	public void ToggleSelection(bool value) {
		if (meshRenderer == null) {
			return;
		}
		
		Color newColor = value ? originalColor + new Color(0.3f, 0.3f, 0.3f) : originalColor;
		meshRenderer.material.DOColor(newColor, 0.3f);
	}

	public void RemoveObstacle() {
		meshRenderer.material.DOColor(Color.clear, 0.3f).OnComplete(() => {
			Destroy(this.gameObject);
		});
	}
}
