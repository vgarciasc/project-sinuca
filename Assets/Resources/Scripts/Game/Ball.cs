using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	[Header("Mechanics")]
	[SerializeField]
	float velocityCap;

	[Header("References")]
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

	Material originalMaterial;

	void Start() {
		originalMaterial = mr.material;
	}

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

		if (rb3d != null && GameObject.FindGameObjectWithTag("GamePreferences") != null) {
			GamePreferencesDatabase gpd = GamePreferencesDatabase.GetGamePreferencesDatabase();
			mr.material.EnableKeyword("_DETAIL_MULX2");
			mr.material.SetTexture("_DetailAlbedoMap", gpd.GetTextureByPlayerID(playerID));
			mr.material.SetColor("_DetailAlbedoMap", gpd.GetColorByPlayerID(playerID));
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

	void FixedUpdate() {
		if (rb3d.velocity.magnitude > velocityCap) {
			rb3d.velocity = rb3d.velocity.normalized * velocityCap;
		}
	}

	public bool IsMoving() {
		if (!this.gameObject.activeSelf) {
			return false;
		} else if (rb3d != null) {
			return rb3d.velocity.magnitude > 0.1f;
		}
		
		print("This shouldn't be happening");
		return false;
	}

	public void Boost(float intensity) {
		if (rb3d != null) {
			rb3d.velocity *= intensity;
		}
	}

	public void Teleport(Vector3 position, Vector3 direction) {
		this.transform.position = position;

		if (direction != Vector3.up) {
			RedirectVelocity(direction);
		}
	}

	public void RedirectVelocity(Vector3 direction) {
		float magnitude = rb3d.velocity.magnitude;
		rb3d.velocity = direction.normalized * magnitude;
	}
}
