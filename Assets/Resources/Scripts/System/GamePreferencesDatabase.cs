using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GamePreferencesDatabase : MonoBehaviour {
	public static GamePreferencesDatabase GetGamePreferencesDatabase() {
		return (GamePreferencesDatabase) HushPuppy.safeFindComponent("GamePreferences", "GamePreferencesDatabase");
	}

	[Header("Fixed Values")]
	public List<Color> colors;
	public List<Texture> textures;
	public List<Sprite> tableType;
	public Image transitionImg;
	public GamePreferences gamePreferences;

	void Start() {
		DontDestroyOnLoad(this.gameObject);
	}

	public Color GetColorByPlayerID(int playerID) {
		return colors[gamePreferences.color[playerID]];
	}
}
