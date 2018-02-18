using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameOverUI : MonoBehaviour {
	[SerializeField]
	CanvasGroup canvas;
	[SerializeField]
	Image banner;
	[SerializeField]
	TextMeshProUGUI victoryText;		
	
	public void Show(int playerID) {
		if (playerID == -2) { //tie
			banner.color = Color.grey;
			victoryText.text = "Empate!";
		}
		else {
			if (GameObject.FindGameObjectWithTag("GamePreferences") != null) {
				Color color = GamePreferencesDatabase.GetGamePreferencesDatabase().GetColorByPlayerID(playerID);
				banner.color = color;
			}
			
			victoryText.text = "Jogador #" + "<color=#BABABA>" + (playerID + 1) + "</color> venceu!";
		}

		canvas.gameObject.SetActive(true);
		canvas.DOFade(1f, 1f);
		StartCoroutine(ResetGame());
	}

	IEnumerator ResetGame() {
		var obj = HushPuppy.safeFind("GamePreferences");
		if (obj != null) Destroy(obj);
		
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene("MainMenu");
	}
}
