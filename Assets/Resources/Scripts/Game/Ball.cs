using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	[SerializeField]
	Rigidbody2D rb2d;
	[SerializeField]
	Rigidbody rb3d;
	[SerializeField]
	SpriteRenderer sr;
	[SerializeField]
	MeshRenderer mr;
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
		this.data = data;

		if (sr != null) {
			sr.color = data.color;
		} else if (mr != null) {
			mr.material = data.material;
		}
	}

	public void SetPlayerBall() {
		var obj = Instantiate(playerBall, this.transform, false);
		isPlayer = true;

		if (rb3d != null) {
			mr.material = data.playerMaterial;
		}
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.gameObject.tag == "Pocket") {
			Pocket();
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.tag == "Pocket") {
			Pocket();
		}
	}

	void Pocket() {
		if (pocketBallEvent != null) pocketBallEvent(this);
		this.gameObject.SetActive(false);
	}

	public bool IsMoving() {
		if (rb2d != null) {
			return rb2d.velocity.magnitude > 0.05f;
		} else if (rb3d != null) {
			return rb3d.velocity.magnitude > 0.1f;
		} 
		
		print("This shouldn't be happening");
		return false;
	}
}
