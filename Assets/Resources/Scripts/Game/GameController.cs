using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField]
	List<PlayerUI> playerUI = new List<PlayerUI>();
	[SerializeField]
	List<Ball> balls = new List<Ball>();

	List<int> playerScore = new List<int>();
	PlayerDatabase playerDatabase;

	int playerCount = 2;

	void Start() {
		playerDatabase = PlayerDatabase.GetPlayerDatabase();

		InitPlayers();
		InitBalls();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			playerUI[1].ToggleTurn(true);
		}
	}

	void InitPlayers() {
		foreach (var k in playerUI) {
			k.InitScore(5);
			k.Init(playerUI.IndexOf(k));
			playerScore.Add(0);
		}

		playerUI[0].ToggleTurn(true);
	}

	void InitBalls() {
		for (int i = 0; i < balls.Count; i++) {
			balls[i].SetPlayerID(i % 2);
			balls[i].pocketBallEvent += PocketBall;
		}
	}

	public void AddPlayerScore(int playerID, int amount) {
		playerScore[playerID] += amount;
	}

	void UpdatePlayerUI(int playerID) {
		playerUI[playerID].UpdateScore(playerScore[playerID]);
	}

	void PocketBall(Ball ball) {
		AddPlayerScore(ball.playerID, 1);
		UpdatePlayerUI(ball.playerID);
	}
}
