using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour {
	[Header("References")]
	[SerializeField]
	GameObject cueStickPrefab;
	
	[Header("Mechanics")]
	[SerializeField]
	float forceModifier;

	Rigidbody2D rb2d;
	Rigidbody rb3d;

	public Ball ball { get; private set; }
	public bool inTurn { get; private set; }
	
	CueStick cueStick;
	PlayerCursorManager cursor;

	bool hasShot;
	bool isMoving;

	void Start() {
		ball = this.GetComponentInParent<Ball>();
		rb2d = GetComponentInParent<Rigidbody2D>();
		rb3d = GetComponentInParent<Rigidbody>();
	}

	void Update() {
		isMoving = ball.IsMoving();

		HandleTurnOver();
	}

	void InitCueStick() {
		hasShot = false;

		var obj = Instantiate(
			cueStickPrefab, 
			this.transform.position, 
			this.transform.rotation);
		obj.transform.SetParent(HushPuppy.safeFind("World").transform);
		var cs = obj.GetComponentInChildren<CueStick>();
		cs.SetPlayerBall(this);

		cueStick = cs;
		
		cursor = GetComponent<PlayerCursorManager>();
		cursor.SetCueStick(cs);
	}

	void HandleTurnOver() {
		if (inTurn && hasShot && !isMoving) {
			inTurn = false;
			cursor.EndTurn();
		}
	}

	public void ToggleTurn(bool value) {
		inTurn = value;
		ToggleCueStick(value);
	}

	public void Shoot(float angle, float intensity) {
		if (rb2d != null) {
			Vector2 direction = 
				new Vector2(
					Mathf.Cos(angle), 
					Mathf.Sin(angle)).normalized 
				* intensity
				* forceModifier
				* Mathf.Rad2Deg;
			rb2d.AddForce(direction);
		}
		else if (rb3d != null) {
			Vector3 direction = 
				new Vector3(
					Mathf.Cos(angle),
					0f,
					Mathf.Sin(angle)).normalized 
				* intensity / 20f
				* forceModifier
				* Mathf.Rad2Deg;
			rb3d.AddForce(direction);
		}

		hasShot = true;
		ToggleCueStick(false);
	}

	void ToggleCueStick(bool value) {
		if (value) {
			InitCueStick();
		} else {
			if (cueStick != null) {
				Destroy(cueStick.gameObject);
			}
		}
	}

	public void Destroy() {
		ToggleCueStick(false);
		Destroy(this.gameObject);
	}
}
