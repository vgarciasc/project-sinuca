using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	[SerializeField]
	Rigidbody2D rigidbody;
	[SerializeField]
	SpriteRenderer sr;
	[SerializeField]
	GameObject playerBall;	

	public delegate void BallDelegate(Ball ball);
	public event BallDelegate pocketBallEvent;

	PlayerData data;
	public int playerID { get; private set; }
	public bool isPlayer { get; private set; }

	public void SetPlayerID(int playerID) {
		this.playerID = playerID;
		SetPlayerData(PlayerDatabase.GetPlayerDatabase().GetPlayerDataByID(playerID));
	}

	void SetPlayerData(PlayerData data) {
		sr.color = data.color;
	}

	public void SetPlayerBall() {
		var obj = Instantiate(playerBall, this.transform, false);
		isPlayer = true;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Pocket") {
			Pocket();
		}
	}

	void Pocket() {
		if (pocketBallEvent != null) pocketBallEvent(this);
		this.gameObject.SetActive(false);
	}

	public bool IsMoving() {
		return rigidbody.velocity.magnitude > 0.05f;
	}
}
