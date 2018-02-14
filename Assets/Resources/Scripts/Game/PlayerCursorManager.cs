using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursorManager : MonoBehaviour {
	public enum State { NONE, CUE, POWERUP }

	[SerializeField]
	GameObject cursorPrefab;

	Powerup currentPowerup = Powerup.HAND_OF_BLOCKING;
	State currentState = State.NONE;
	CueStick cueStick;
	PlayerCursor cursor;

	void Update() {
		if (Input.GetButtonDown("Fire1")) {
			OnLeftClick();
		}
		if (Input.GetButtonDown("Fire2")) {
			OnRightClick();
		}
	}

	public void SetCueStick(CueStick cueStick) {
		this.cueStick = cueStick;
		currentState = State.CUE;
	}

	void OnLeftClick() {
		switch (currentState) {
			case State.POWERUP:
				if (cursor.selected && currentPowerup != Powerup.NONE) {
					cursor.UsePowerup(currentPowerup);
					currentPowerup = Powerup.NONE;
				}
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
				cueStick.TogglePause(true);
				InitCursor();
				break;
			case State.POWERUP:
				currentState = State.CUE;
				cueStick.TogglePause(false);
				Destroy(cursor);
				break;
		}
	}

	void InitCursor() {
		var obj = Instantiate(cursorPrefab, 
			GameObject.FindGameObjectWithTag("CursorContainer").transform, 
			false);
		cursor = obj.GetComponentInChildren<PlayerCursor>();
		cursor.Init(currentPowerup);
	}
}
