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
			PlayerData data = PlayerDatabase.GetPlayerDatabase().GetPlayerDataByID(playerID);
			banner.color = data.color;
			victoryText.text = "Jogador #" + "<color=#BABABA>" + (playerID + 1) + "</color> venceu!";
		}

		canvas.gameObject.SetActive(true);
		canvas.DOFade(1f, 1f);
		StartCoroutine(ResetGame());
	}

	IEnumerator ResetGame() {
		yield return new WaitForSeconds(2f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
