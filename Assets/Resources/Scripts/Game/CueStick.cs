using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueStick : MonoBehaviour {
	[SerializeField]
	[Range(0f, 100f)]
	float intensityModifier = 30f;
	
	LineRenderer lineRenderer;
	PlayerBall playerBall;

	float currentAngle;
	float currentIntensity; //from 0f to 1f

	bool aimingShot;
	bool shotAnimation;
	bool paused;

	void Start() {
		lineRenderer = this.GetComponentInChildren<LineRenderer>();
	}

	public void SetPlayerBall(PlayerBall playerBall) {
		this.playerBall = playerBall;
	}

	void Update() {
		if (!paused) {
			HandleAim();
			HandleShot();
		}
	}

	void HandleAim() {		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit hit;
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(mouseRay, out hit, (1 << LayerMask.NameToLayer("MouseClip")))) {
			mousePos = hit.point;
		}

		var ballPos = playerBall.transform.position;
		var aimVec = Vector3.ProjectOnPlane(mousePos - ballPos, Vector3.up);
		var axisVec = Vector3.right;

		Debug.DrawLine(ballPos, aimVec + ballPos, Color.green, Time.deltaTime);
		Debug.DrawLine(ballPos, axisVec + ballPos, Color.green, Time.deltaTime);

		float angle = (Vector3.Angle(aimVec, axisVec) * Mathf.Deg2Rad);
		bool orientation = aimVec.z * axisVec.x - aimVec.x * axisVec.z >= 0;
		float distance = (2.1f + currentIntensity * 0.8f);

		Vector3 cuePos = new Vector3(
			Mathf.Cos(orientation? angle : -angle),
			0,
			Mathf.Sin(orientation? angle : -angle)
		).normalized * distance * -1f;

		var cueAngle = (orientation? angle : -angle);
		currentAngle = cueAngle;

		this.transform.position = cuePos + ballPos;

		if (!shotAnimation) {
			this.transform.rotation = Quaternion.Euler(90, 0, cueAngle * Mathf.Rad2Deg);
		}

		var meshRenderer = this.GetComponentInChildren<MeshFilter>().mesh;
		meshRenderer.RecalculateBounds();
		meshRenderer.RecalculateNormals();

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
			playerBall.Shoot(currentAngle, originalIntensity * intensityModifier);
			GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayAudioClip(Sfx.HIT_BALL);
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
			currentIntensity -= Time.deltaTime * 15f;
		}
	}

	void SetPathPreview(Vector3 position, Vector3 direction) {
		Ray ray = new Ray(position, direction);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, (1 << LayerMask.NameToLayer("Wall")))) {
			Vector3[] vectors = new Vector3[3];
			vectors[0] = position;
			vectors[1] = (hit.point);
			// vectors[2] = (Vector2.Reflect(direction, hit.normal)).normalized * 5f + (Vector2) hit.point;
			lineRenderer.SetPositions(vectors);
		}
	}

	public void TogglePause(bool value) {
		paused = value;
	}
}
