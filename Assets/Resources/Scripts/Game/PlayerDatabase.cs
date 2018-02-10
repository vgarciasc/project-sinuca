using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDatabase : MonoBehaviour {
	public static PlayerDatabase GetPlayerDatabase() {
		return (PlayerDatabase) HushPuppy.safeFindComponent("PlayerDatabase", "PlayerDatabase");
	}
	
	[SerializeField]
	List<PlayerData> database = new List<PlayerData>();

	public PlayerData GetPlayerDataByID(int playerID) {
		return database[playerID];
	}
}
