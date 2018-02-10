using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	[SerializeField]
	Rigidbody2D rigidbody;
	[SerializeField]
	SpriteRenderer sr;

	public delegate void BallDelegate(Ball ball);
	public event BallDelegate pocketBallEvent;

	PlayerData data;
	public int playerID { get; private set; }

	public void SetPlayerID(int playerID) {
		this.playerID = playerID;
		SetPlayerData(PlayerDatabase.GetPlayerDatabase().GetPlayerDataByID(playerID));
	}

	void SetPlayerData(PlayerData data) {
		sr.color = data.color;
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
}
