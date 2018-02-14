using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDatabase : MonoBehaviour {
	public static PlayerDatabase GetPlayerDatabase() {
		return (PlayerDatabase) HushPuppy.safeFindComponent("PlayerDatabase", "PlayerDatabase");
	}
	
	[SerializeField]
	List<PlayerData> database = new List<PlayerData>();

	GamePreferencesDatabase gpd;

	void Start() {
		if (GameObject.FindGameObjectWithTag("GamePreferences") == null) return;

		gpd = GamePreferencesDatabase.GetGamePreferencesDatabase();

		database[0].color = gpd.colors[gpd.gamePreferences.color[0]];
		database[0].material.color = database[0].color;
		database[0].playerMaterial.color = database[0].color;
		database[0].playerMaterial.SetTexture("asdf", gpd.textures[gpd.gamePreferences.texture[0]]);

		database[1].color = gpd.colors[gpd.gamePreferences.color[1]];
		database[1].material.color = database[1].color;
		database[1].playerMaterial.color = database[1].color;
		database[1].playerMaterial.SetTexture("asdf", gpd.textures[gpd.gamePreferences.texture[1]]);
	}

	public PlayerData GetPlayerDataByID(int playerID) {
		return database[playerID];
	}
}
