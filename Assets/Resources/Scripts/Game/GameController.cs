using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField]
	List<PlayerUI> playerUI = new List<PlayerUI>();

	List<int> playerScore = new List<int>();

	void Start() {
		foreach (var k in playerUI) {
			k.SetMaxScore(5);
			k.UpdateScore(0);
			k.Init(playerUI.IndexOf(k));
			playerScore.Add(0);
		}

		playerUI[0].ToggleTurn(true);
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			playerUI[1].ToggleTurn(true);
		}
	}

	public void AddPlayerScore(int playerID, int amount) {
		playerScore[playerID] += amount;
		UpdatePlayerUI(playerID);
	}

	void UpdatePlayerUI(int playerID) {
		playerUI[playerID].UpdateScore(playerScore[playerID]);
	}
}
