using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueStick : MonoBehaviour {
	SpriteRenderer sr;
	PlayerBall playerBall;

	float currentAngle;

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

		Vector3 cuePos = new Vector3(
			Mathf.Cos(orientation? angle : -angle),
			Mathf.Sin(orientation? angle : -angle)
		).normalized * 3.5f * -1f + ballPos;

		var cueAngle = (orientation? angle : -angle);
		currentAngle = cueAngle;

		this.transform.position = cuePos;
		this.transform.rotation = Quaternion.Euler(0, 0, cueAngle * Mathf.Rad2Deg);
	}

	void HandleShot() {
		if (!Input.GetButtonDown("Fire1")) {
			return;
		}

		playerBall.Shoot(currentAngle, 10);
	}
}
