using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour {

	[Header("References")]
	[SerializeField]
	Transform container;
	[SerializeField]
	GameObject powerupPrefab;

	List<Transform> positions = new List<Transform>();
	Powerup currentPowerup;
	PowerupManager manager;
	int lastPosition = -1;
	
	void Start() {
		manager = PowerupManager.GetPowerupManager();

		PopulatePositions();
	}

	void PopulatePositions() {
		foreach (Transform t in container) {
			positions.Add(t);
		}
	}

	Vector3 GetRandomPosition() {
		PopulatePositions();
		int dice = -1;

		do {
			dice = Random.Range(0, positions.Count);
		} while (dice == lastPosition);

		lastPosition = dice;
		return positions[dice].position;
	}

	public void SpawnRandomPowerup() {
		manager = PowerupManager.GetPowerupManager();
		SpawnPowerup(manager.GetRandomPowerup());
	}

	public void SpawnPowerup(PowerupData data) {
		CleanPowerups();
		Vector3 position = GetRandomPosition();
		GameObject obj = Instantiate(powerupPrefab, position, Quaternion.identity);
		currentPowerup = obj.GetComponentInChildren<Powerup>();
		currentPowerup.SetData(data);
	}

	void CleanPowerups() {
		if (currentPowerup != null) {
			currentPowerup.DestroyMe();
		}
	}
}
