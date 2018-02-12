using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	[SerializeField]
	GameOverUI gameOver;	
	[SerializeField]
	List<PlayerUI> playerUI = new List<PlayerUI>();
	[SerializeField]
	List<Ball> balls = new List<Ball>();
	[SerializeField]
	List<Ball> playerStart = new List<Ball>();
	
	List<PlayerBall> playerBalls = new List<PlayerBall>();
	List<int> playerScores = new List<int>();
	PlayerDatabase playerDatabase;

	int playerCount = 2;
	int maxScore = 5;
	bool changeTurnFlag = false;

	#region Initialization
	void Start() {
		playerDatabase = PlayerDatabase.GetPlayerDatabase();

		InitPlayers();
		InitBalls();

		StartCoroutine(GameLoop());
	}

	void InitPlayers() {
		foreach (var k in playerUI) {
			k.InitScore(maxScore);
			k.Init(playerUI.IndexOf(k));
			playerScores.Add(0);
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
			var pball = SetPlayerBall(playerStart[i]);
			playerBalls.Add(pball);
		}
	}

	PlayerBall SetPlayerBall(Ball ball) {
		ball.SetPlayerBall();
		var playerBall = ball.GetComponentInChildren<PlayerBall>();
		return playerBall;
	}
	#endregion

	IEnumerator GameLoop() {
		while (GetWinnerID() == -1) {
			for (int i = 0; i < playerCount; i++) {
				playerUI[i].ToggleTurn(true);
				playerBalls[i].ToggleTurn(true);
				yield return new WaitWhile(() => playerBalls[i].inTurn);
				yield return new WaitWhile(() => AnyBallMoving());
				playerBalls[i].ToggleTurn(false);
				playerUI[i].ToggleTurn(false);
				
				changeTurnFlag = false;

				if (GetWinnerID() != -1) break;
			}
		}

		gameOver.Show(GetWinnerID());
	}

	public void AddPlayerScore(int playerID, int amount) {
		playerScores[playerID] += amount;
	}

	void UpdatePlayerUI(int playerID) {
		playerUI[playerID].UpdateScore(playerScores[playerID]);
	}

	void ReassignPlayerBall(Ball originalBall) {
		List<Ball> candidateBalls = new List<Ball>();
		for (int i = 0; i < balls.Count; i++) {
			if (balls[i].gameObject.activeSelf 
			&& balls[i].playerID == originalBall.playerID
			&& !balls[i].isPlayer) {
				candidateBalls.Add(balls[i]);
			}
		}

		if (candidateBalls.Count > 0) {
			int dice = Random.Range(0, candidateBalls.Count);
			Ball elected = candidateBalls[dice];
			SetPlayerBall(elected);

			for (int i = 0; i < playerBalls.Count; i++) {
				if (playerBalls[i].ball != null && playerBalls[i].ball.playerID == originalBall.playerID) {
					playerBalls[i] = elected.GetComponentInChildren<PlayerBall>();
				}
			}
		}
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

	int GetWinnerID() {
		int winner = -1;
		bool tie = false;

		for (int i = 0; i < playerScores.Count; i++) {
			if (playerScores[i] >= maxScore) {
				if (winner != -1) {
					tie = true;
					break;
				}
				else {
					winner = i;
				}
			}
		}

		if (tie) {
			return -2;
		}

		if (winner != -1) {
			return winner;
		}

		return -1;
	}

	void PocketBall(Ball ball) {
		if (ball.isPlayer) {
			for (int i = 0; i < playerCount; i++) {
				if (i != ball.playerID) {
					AddPlayerScore(i, 1);
					UpdatePlayerUI(i);
				}
			}

			ReassignPlayerBall(ball);
			return;
		}

		AddPlayerScore(ball.playerID, 1);
		UpdatePlayerUI(ball.playerID);
	}
}
