using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour {
	[SerializeField]
	GameObject cueStickPrefab;

	public delegate void PlayerBallDelegate(Ball ball);
	public event PlayerBallDelegate playerShotEvent;
	public Ball ball { get; private set; }
	
	CueStick cueStick;
	Rigidbody2D rigidbody;

	bool inTurn;
	bool hasShot;
	bool isMoving;

	void Start() {
		ball = this.GetComponentInParent<Ball>();
		rigidbody = GetComponentInParent<Rigidbody2D>();
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
	}

	void HandleTurnOver() {
		if (inTurn && hasShot && !isMoving) {
			if (playerShotEvent != null) {
				playerShotEvent(ball);
			}			

			inTurn = false;
		}
	}

	public void ToggleTurn(bool value) {
		inTurn = value;
		ToggleCueStick(value);
	}

	public void Shoot(float angle, float intensity) {
		Vector2 direction = 
			new Vector2(
				Mathf.Cos(angle), 
				Mathf.Sin(angle)).normalized 
			* intensity
			* Mathf.Rad2Deg;

		rigidbody.AddForce(direction);
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
}
