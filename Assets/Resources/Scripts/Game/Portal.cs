using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Portal : MonoBehaviour, SelectableObstacle {

	[Header("References")]
	public MeshRenderer meshRenderer;

	[Header("Mechanics")]
	[SerializeField]
	bool isEntry = true;
	[SerializeField]
	bool directional;
	[SerializeField]
	[Range(0f, 360f)]
	float rotationSpeed;

	public float angle { get; private set; }
	static List<Ball> inCooldown = new List<Ball>();

	PortalManager manager;
	Color originalColor;

	void Start() {
		DeactivateIfPreference();
		manager = this.GetComponentInParent<PortalManager>();
		originalColor = meshRenderer.material.color;
	}

	void Update() {
		if (directional) {
			angle += (Time.deltaTime * rotationSpeed);
			angle %= 360f;
			this.transform.rotation = Quaternion.Euler(0f, angle, 0f);
		}
	}

	void OnTriggerEnter(Collider collider) {
		var obj = collider.gameObject;

		if (isEntry && obj.tag == "Ball") {
			var ball = obj.GetComponentInChildren<Ball>();
			if (!inCooldown.Contains(ball)) {
				Teleport(ball);
			}
		}
	}

	void Teleport(Ball ball) {
		var exit = manager.GetExit(this);
		Vector3 direction = Vector3.up;

		if (exit.directional) {
			var angle = -exit.angle * Mathf.Deg2Rad;
			direction = new Vector3(
				Mathf.Cos(angle),
				0,
				Mathf.Sin(angle)
			);
			direction = Vector3.ProjectOnPlane(direction, Vector3.up);
		}

		ball.Teleport(exit.transform.position, direction);
		StartCoroutine(HandleCooldown(ball));
	}

	IEnumerator HandleCooldown(Ball ball) {
		inCooldown.Add(ball);
		yield return new WaitForSeconds(1f);
		if (inCooldown.Contains(ball)) {
			inCooldown.Remove(ball);
		}
	}

	bool selected;
	bool beingRemoved;

	public void ToggleSelection(bool value) {
		if (meshRenderer == null) return;
		if (value == selected) return;

		selected = value;
		
		manager.GetExit(this).ToggleSelection(value);
		Color newColor = value ? originalColor + new Color(0.3f, 0.3f, 0.3f) : originalColor;
		meshRenderer.material.DOColor(newColor, 0.3f);
	}

	public void RemoveObstacle() {
		if (beingRemoved) return;
		
		beingRemoved = true;
		manager.GetExit(this).RemoveObstacle();
		meshRenderer.material.DOColor(Color.clear, 0.3f).OnComplete(() => {
			Destroy(this.gameObject);
		});
	}

	void DeactivateIfPreference() {
		if (GameObject.FindGameObjectWithTag("GamePreferences") != null) {
			GamePreferencesDatabase gpd = GamePreferencesDatabase.GetGamePreferencesDatabase();
			if (!gpd.gamePreferences.modifiers) {
				Destroy(this.gameObject);
			}
		}
	}
}
