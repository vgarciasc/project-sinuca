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

		Vector3 originalScale = this.transform.localScale;
		this.transform.localScale = Vector3.zero;
		this.transform.DOScale(originalScale, 0.25f).SetEase(Ease.InOutBack);
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

		this.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBack).OnComplete(a);
	}
}
