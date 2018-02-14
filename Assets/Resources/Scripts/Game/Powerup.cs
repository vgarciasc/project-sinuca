using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Powerup : MonoBehaviour {
	MeshRenderer meshRenderer;
	PowerupManager manager;
	PowerupData data;

	void Start() {
		manager = PowerupManager.GetPowerupManager();
		meshRenderer = GetComponentInChildren<MeshRenderer>();
	}

	public void SetData(PowerupData data) {
		this.data = data;
	}

	void OnTriggerEnter(Collider collider) {
		var obj = collider.gameObject;
		var ball = obj.GetComponentInChildren<PlayerBall>();

		if (ball != null) {
			DestroyMe(() => {
				manager.AddPowerup(ball.ball.playerID, data);
				Destroy(this.gameObject);
			});
		}
	}

	public void DestroyMe(TweenCallback a = null) {
		if (a == null) {
			a = (() => {Destroy(this.gameObject);});
		}

		meshRenderer.material.DOColor(Color.clear, 0.3f).OnComplete(a);
	}
}
