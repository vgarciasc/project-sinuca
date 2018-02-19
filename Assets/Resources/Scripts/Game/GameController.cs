﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour {
	public static GameController GetGameController() {
		return (GameController) HushPuppy.safeFindComponent("GameController", "GameController");
	}
	
	[Header("Game Control")]
	[SerializeField]
	int maxScore = 6;
	[SerializeField]
	bool chainTurns = true;

	[Header("References")]
	[SerializeField]
	GameOverUI gameOver;	
	[SerializeField]
	List<Ball> balls = new List<Ball>();
	[SerializeField]
	List<Ball> playerStart = new List<Ball>();
	[SerializeField]
	PowerupSpawner powerupSpawner;
	public List<PlayerUI> playerUI;
	[SerializeField]
	CanvasGroup beginGameCanvasGroup;
	[SerializeField]
	CanvasGroup chainTurnCanvasGroup;

	List<PlayerBall> playerBalls = new List<PlayerBall>();
	List<int> playerScores = new List<int>();
	PlayerDatabase playerDatabase;

	int playerCount = 2;
	bool reassigningBalls;

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
			k.SetPowerup(null);
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

	void RemovePlayerBall(PlayerBall playerBall) {
		playerBall.ball.SetPlayerID(playerBall.ball.playerID);
		playerBall.Destroy();
	}
	#endregion

	List<int> currentlyPocketed = new List<int>();

	IEnumerator GameLoop() {
		beginGameCanvasGroup.alpha = 1f;
		yield return new WaitForSeconds(2f);
		beginGameCanvasGroup.DOFade(0f, 1f);

		while (GetWinnerID() == -1) {
			RandomSpawn();
			for (int i = 0; i < playerCount; i++) {
				do {
					currentlyPocketed = new List<int>();
					playerUI[i].ToggleTurn(true);
					playerBalls[i].ToggleTurn(true);
					
					yield return new WaitWhile(() => playerBalls[i].inTurn || reassigningBalls);
					yield return new WaitForSeconds(1f);
					yield return new WaitWhile(() => AnyBallMoving());

					playerBalls[i].ToggleTurn(false);
					playerUI[i].ToggleTurn(false);
					
					if (GetWinnerID() == -1 && ShouldGetAnotherTurn(i)) {
						chainTurnCanvasGroup.alpha = 1f;
						yield return new WaitForSeconds(0.25f);
						chainTurnCanvasGroup.DOFade(0f, 1f);
					}
				} while (ShouldGetAnotherTurn(i) && GetWinnerID() == -1);
			}
		}
		GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayAudioClip(Sfx.END_GAME);
		gameOver.Show(GetWinnerID());
	}

	bool ShouldGetAnotherTurn(int playerID) {
		if (!chainTurns) return false;
		if (currentlyPocketed.Count == 0) return false;
		
		GameObject.Find("AudioManager").GetComponent<AudioManager>().PlayAudioClip(Sfx.CHAIN);

		for (int i = 0; i < currentlyPocketed.Count; i++) {
			if (currentlyPocketed[i] == playerID) {
				return false;
			}
		}

		return true;
	}

	public void AddPlayerScore(int playerID, int amount) {
		playerScores[playerID] += amount;
	}

	void UpdatePlayerUI(int playerID) {
		playerUI[playerID].UpdateScore(playerScores[playerID]);
	}

	void ReassignPlayerBall(Ball originalBall, bool inTurn = false) {
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

			elected.GetComponentInChildren<PlayerBall>().ToggleTurn(inTurn);

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
		currentlyPocketed.Add(ball.playerID);
		for (int i = 0; i < playerCount; i++) {
			if (i != ball.playerID) {
				AddPlayerScore(i, 1);
				UpdatePlayerUI(i);
				if (ball.isPlayer) {
					ReassignPlayerBall(ball);
				}
				return;
			}
		}
	}

	public void RemoveAndReassignPlayers(int player_caster_ID) {
		reassigningBalls = true;

		for (int i = 0; i < playerCount; i++) {
			var ball = playerBalls[i].ball;
			RemovePlayerBall(playerBalls[i]);
			ReassignPlayerBall(ball);

			if (i == player_caster_ID) {
				playerBalls[i].ToggleTurn(true);
			}
		}

		reassigningBalls = false;
	}

	void RandomSpawn() {
		bool shouldSpawn = true;
		if (GameObject.FindGameObjectWithTag("GamePreferences") != null) {
			GamePreferencesDatabase gpd = GamePreferencesDatabase.GetGamePreferencesDatabase();
			shouldSpawn = (gpd.gamePreferences.powerUps);
		}

		if (shouldSpawn) {
			powerupSpawner.SpawnRandomPowerup();
		}
	}
}
