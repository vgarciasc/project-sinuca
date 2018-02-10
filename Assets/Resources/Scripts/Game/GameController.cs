using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField]
	List<PlayerUI> playerUI = new List<PlayerUI>();
	[SerializeField]
	List<Ball> balls = new List<Ball>();
	[SerializeField]
	List<Ball> playerStart = new List<Ball>();
	
	List<PlayerBall> playerBalls = new List<PlayerBall>();
	List<int> playerScore = new List<int>();
	PlayerDatabase playerDatabase;

	int playerCount = 2;
	bool changeTurnFlag = false;

	void Start() {
		playerDatabase = PlayerDatabase.GetPlayerDatabase();

		InitPlayers();
		InitBalls();

		StartCoroutine(GameLoop());
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

		for (int i = 0; i < playerStart.Count; i++) {
			playerStart[i].SetPlayerID(i % 2);
			playerStart[i].pocketBallEvent += PocketBall;

			playerStart[i].SetPlayerBall();
			var playerBall = playerStart[i].GetComponentInChildren<PlayerBall>();
			playerBalls.Add(playerBall);
			playerBall.playerShotEvent += HandlePlayerShot;
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

		if (ball.isPlayer) {
			playerBalls.Clear();
			print("K");
		}
	}

	IEnumerator GameLoop() {
		while (true) {
			for (int i = 0; i < playerCount; i++) {
				//for each player, roll his turn

				for (int j = 0; j < playerCount; j++) {
					if (j != i) {
						//deactivates other players
						playerBalls[j].GetComponentInChildren<PlayerBall>().ToggleTurn(false);
					}
				}

				playerUI[i].ToggleTurn(true);
				playerBalls[i].GetComponentInChildren<PlayerBall>().ToggleTurn(true);
				yield return new WaitUntil(() => changeTurnFlag);
				playerUI[i].ToggleTurn(false);
				
				changeTurnFlag = false;
			}
		}
	}

	void HandlePlayerShot(Ball ball) {
		StartCoroutine(TurnOverWhenBallsStop());
	}

	IEnumerator TurnOverWhenBallsStop() {
		yield return new WaitWhile(() => AnyBallMoving());
		changeTurnFlag = true;
	}

	bool AnyBallMoving() {
		for (int i = 0; i < balls.Count; i++) {
			if (balls[i].IsMoving()) {
				return true;
			}
		}

		for (int i = 0; i < playerBalls.Count; i++) {
			if (playerBalls[i].ball.IsMoving()) {
				return true;
			}
		}

		return false;
	}
}
