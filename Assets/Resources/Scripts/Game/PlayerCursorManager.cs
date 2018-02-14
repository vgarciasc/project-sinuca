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

	void Start() {
		powerupManager = PowerupManager.GetPowerupManager();
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

	Powerup GetCurrentPowerup() {
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

	void UsePowerup(Powerup powerup) {
		if (powerup == null) return;

		switch (powerup.kind) {
			case PowerupEnum.HAND_OF_BLOCKING:
				if (cursor.blockingWallPreview.valid) {
					cursor.blockingWallPreview.InstantiatePreview();
					powerupManager.RemovePowerup(player.ball.playerID);
				}
				return;
			case PowerupEnum.HAND_OF_DESTRUCTION:
				if (cursor.selected != null) {
					cursor.selected.RemoveObstacle();
					powerupManager.RemovePowerup(player.ball.playerID);
				}
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

		cursor.Destroy();
	}
}
