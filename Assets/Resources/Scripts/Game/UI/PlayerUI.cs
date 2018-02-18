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
	PlayerData data;

	public void Init(int playerID) {
		SetPlayerID(playerID);
		ToggleTurn(false, false);
	}

	public void SetPlayerID(int playerID) {
		this.playerID = playerID;

		foreach (var k in scoreBalls) {
			Color color = Color.red;

			var gpd = GameObject.FindGameObjectWithTag("GamePreferences");
			if (gpd != null) {
				color = GamePreferencesDatabase.GetGamePreferencesDatabase().GetColorByPlayerID(playerID);
			}

			k.SetPlayerColor(color);
		}
	}

	public void InitScore(int maxScore) {
		HushPuppy.destroyChildren(ballContainer);
		scoreBalls = new List<PlayerScoreBallUI>();

		for (int i = 0; i < maxScore; i++) {
			var obj = Instantiate(scoreBallPrefab, ballContainer, false);
			var scoreBall = obj.GetComponentInChildren<PlayerScoreBallUI>();
			scoreBalls.Add(scoreBall);
			scoreBall.Toggle(false, false);
		}
	}

	public void UpdateScore(int score) {
		for (int i = 0; i < scoreBalls.Count; i++) {
			scoreBalls[i].Toggle(i >= score, true);
		}
	}

	public void ToggleTurn(bool value, bool animation = true) {
		icon.ToggleTurn(value, animation);
	}

	public void SetPowerup(PowerupData powerup) {
		icon.SetPowerup(powerup);
	}
}
