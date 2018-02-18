using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerIconUI : MonoBehaviour {
	[SerializeField]
	Image glowBackground;
	[SerializeField]
	Image powerupImage;
	[SerializeField]
	TextMeshProUGUI powerupName;

	public void ToggleTurn(bool value, bool animation) {
		if (!animation) {
			glowBackground.DOFade(value ? 1f : 0f, 0f);
			return;
		}

		if (value) {
			GenericAnimationFunctions.FadeIn(glowBackground, 0.5f);
		} else {
			GenericAnimationFunctions.FadeOut(glowBackground, 0.5f);
		}
	}

	public void SetPowerup(PowerupData powerup) {
		if (powerup == null) {
			powerupImage.enabled = false;
			powerupName.enabled = false;
			return;
		}
		
		powerupImage.enabled = true;
		powerupImage.sprite = powerup.sprite;
		powerupName.enabled = true;
		powerupName.text = powerup.name;
	}
}
