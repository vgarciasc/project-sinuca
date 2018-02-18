using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursorManager : MonoBehaviour {
	public enum State { NONE, CUE, POWERUP }

	[Header("References")]
	[SerializeField]
	GameObject cursorPrefab;
	[SerializeField]
	PlayerBall player;

	State currentState = State.NONE;
	CueStick cueStick;
	PlayerCursor cursor;
	PowerupManager powerupManager;
	GameController gameController;

	void Start() {
		powerupManager = PowerupManager.GetPowerupManager();
		gameController = GameController.GetGameController();
	}

	void Update() {
		if (player.inTurn) {
			if (Input.GetButtonDown("Fire1")) {
				OnLeftClick();
			}
			if (Input.GetButtonDown("Fire2")) {
				OnRightClick();
			}
		}
	}

	public void SetCueStick(CueStick cueStick) {
		this.cueStick = cueStick;
		currentState = State.CUE;
	}

	PowerupData GetCurrentPowerup() {
		return powerupManager.playerPowerups[player.ball.playerID];
	}

	void OnLeftClick() {
		var currentPowerup = GetCurrentPowerup();

		switch (currentState) {
			case State.POWERUP:
				UsePowerup(currentPowerup);
				break;
		}
	}

	void OnRightClick() {
		switch (currentState) {
			case State.NONE:
				currentState = State.CUE;
				break;
			case State.CUE:
				currentState = State.POWERUP;
				EnablePowerupMode();
				break;
			case State.POWERUP:
				currentState = State.CUE;
				DisablePowerupMode();
				break;
		}
	}

	void UsePowerup(PowerupData powerup) {
		if (powerup == null) return;

		switch (powerup.kind) {
			case PowerupEnum.HAND_OF_BLOCKING:
				if (cursor.blockingWallPreview.valid) {
					cursor.blockingWallPreview.InstantiatePreview();
					powerupManager.RemovePowerup(player.ball.playerID);
					OnRightClick();
				}
				return;
			case PowerupEnum.HAND_OF_DESTRUCTION:
				if (cursor.selected != null) {
					cursor.selected.RemoveObstacle();
					powerupManager.RemovePowerup(player.ball.playerID);
					OnRightClick();
				}
				return;
			case PowerupEnum.HAND_OF_CONFUSION:
				OnRightClick();
				powerupManager.RemovePowerup(player.ball.playerID);
				gameController.RemoveAndReassignPlayers(player.ball.playerID);
				return;
		}
	}

	void InitCursor() {
		var currentPowerup = GetCurrentPowerup();

		var obj = Instantiate(cursorPrefab, 
			GameObject.FindGameObjectWithTag("CursorContainer").transform, 
			false);
		cursor = obj.GetComponentInChildren<PlayerCursor>();
		cursor.Init(currentPowerup);
	}

	void EnablePowerupMode() {
		if (cueStick != null) {
			cueStick.TogglePause(true);
		}

		InitCursor();
	}

	void DisablePowerupMode() {
		if (cueStick != null) {
			cueStick.TogglePause(false);
		}

		cursor.DestroyMe();
	}

	public void EndTurn() {
		if (currentState == State.POWERUP) {
			OnRightClick();
		}
	}
}
