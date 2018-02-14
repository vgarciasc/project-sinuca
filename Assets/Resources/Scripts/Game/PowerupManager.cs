using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {
	public static PowerupManager GetPowerupManager() {
		return (PowerupManager) HushPuppy.safeFindComponent("GameController", "PowerupManager");
	}

	public List<PowerupData> playerPowerups { get; private set; }

	[Header("List")]	
	[SerializeField]
	List<PowerupData> database;

	GameController gameController;

	void Start() {
		gameController = GameController.GetGameController();

		InitPowerups();
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.F)) {
			AddPowerup(0, database[0]);
		}
		if (Input.GetKeyDown(KeyCode.G)) {
			AddPowerup(0, database[1]);
		}
		if (Input.GetKeyDown(KeyCode.H)) {
			AddPowerup(0, database[2]);
		}
	}

	void InitPowerups() {
		playerPowerups = new List<PowerupData>();
		for (int i = 0; i < gameController.playerUI.Count; i++) {
			playerPowerups.Add(null);
			AddPowerup(i, null);
		}
	}

	public void AddPowerup(int playerID, PowerupData powerup) {
		playerPowerups[playerID] = powerup;
		gameController.playerUI[playerID].SetPowerup(powerup);
	}

	public void RemovePowerup(int playerID) {
		playerPowerups[playerID] = null;
		gameController.playerUI[playerID].SetPowerup(null);
	}

	public PowerupData GetRandomPowerup() {
		int dice = Random.Range(0, database.Count);
		return database[dice];
	}
}
