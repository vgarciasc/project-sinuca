using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

	[Header("Mechanics")]
	[SerializeField]
	bool isEntry = true;
	[SerializeField]
	bool directional;
	[SerializeField]
	[Range(0f, 360f)]
	float rotationSpeed;

	public float angle { get; private set; }

	PortalManager manager;
	static List<Ball> inCooldown = new List<Ball>();

	void Start() {
		manager = this.GetComponentInParent<PortalManager>();
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
}
