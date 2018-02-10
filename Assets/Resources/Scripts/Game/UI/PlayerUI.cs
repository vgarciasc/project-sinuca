using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	GameObject scoreBallPrefab;
	[SerializeField]
	Transform ballContainer;
	[SerializeField]
	PlayerIconUI icon;

	List<PlayerScoreBallUI> scoreBalls;
	int playerID = -1;

	public void Init(int playerID) {
		SetPlayerID(playerID);
		ToggleTurn(false, false);
	}

	public void SetPlayerID(int playerID) {
		this.playerID = playerID;
	}

	public void SetMaxScore(int maxScore) {
		HushPuppy.destroyChildren(ballContainer);
		scoreBalls = new List<PlayerScoreBallUI>();

		for (int i = 0; i < maxScore; i++) {
			var obj = Instantiate(scoreBallPrefab, ballContainer, false);
			var scoreBall = obj.GetComponentInChildren<PlayerScoreBallUI>();
			scoreBalls.Add(scoreBall);
			scoreBall.Toggle(false);
		}
	}

	public void UpdateScore(int score) {
		for (int i = 0; i < scoreBalls.Count; i++) {
			scoreBalls[i].Toggle(i >= score);
		}
	}

	public void ToggleTurn(bool value, bool animation = true) {
		icon.ToggleTurn(value, animation);
	}
}
