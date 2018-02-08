using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueStick : MonoBehaviour {
	LineRenderer lineRenderer;
	PlayerBall playerBall;

	float currentAngle;
	float currentIntensity; //from 0f to 1f

	bool aimingShot;
	bool shotAnimation;

	void Start() {
		lineRenderer = this.GetComponentInChildren<LineRenderer>();
	}

	public void SetPlayerBall(PlayerBall playerBall) {
		this.playerBall = playerBall;
	}

	void Update() {
		HandleAim();
		HandleShot();
	}

	void HandleAim() {
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		var ballPos = playerBall.transform.position;
		var aimVec = mousePos - ballPos;
		var axisVec = Vector3.right;

		float angle = (Vector2.Angle(aimVec, axisVec) * Mathf.Deg2Rad);
		bool orientation = aimVec.x * axisVec.y - aimVec.y * axisVec.x < 0;
		float distance = (3.15f + currentIntensity * 0.8f);

		Vector3 cuePos = new Vector3(
			Mathf.Cos(orientation? angle : -angle),
			Mathf.Sin(orientation? angle : -angle)
		).normalized * distance * -1f + ballPos;

		var cueAngle = (orientation? angle : -angle);
		currentAngle = cueAngle;

		this.transform.position = cuePos;

		if (!shotAnimation) {
			this.transform.rotation = Quaternion.Euler(0, 0, cueAngle * Mathf.Rad2Deg);
		}

		SetPathPreview(ballPos, aimVec);
		HandleBackAndForth();
	}

	float originalIntensity;
	void HandleShot() {
		if (Input.GetButtonDown("Fire1") && !aimingShot) {
			aimingShot = true;
			currentIntensity = 0f;
		}
		else if (Input.GetButtonUp("Fire1") && aimingShot) {
			aimingShot = false;
			shotAnimation = true;
			originalIntensity = currentIntensity;
		}
		else if (shotAnimation && currentIntensity < 0f) {
			shotAnimation = false;
			playerBall.Shoot(currentAngle, originalIntensity * 10);
		}
	}

	bool movingUp;
	void HandleBackAndForth() {
		if (aimingShot) {
			currentIntensity += Time.deltaTime * 2f * (movingUp ? 1f : -1f);
			if (currentIntensity > 1f) movingUp = false;
			if (currentIntensity < 0f) movingUp = true;
		}
		else if (shotAnimation) {
			currentIntensity -= Time.deltaTime * 8f;
		}
	}

	void SetPathPreview(Vector3 position, Vector3 direction) {
		RaycastHit2D hit = Physics2D.Raycast(position, direction, Mathf.Infinity, (1 << LayerMask.NameToLayer("Wall")));
		
		Vector3[] vectors = new Vector3[3];
		vectors[0] = position;
		vectors[1] = (hit.point);
		// vectors[2] = (Vector2.Reflect(direction, hit.normal)).normalized * 5f + (Vector2) hit.point;
		lineRenderer.SetPositions(vectors);
	}
}
