using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class PlayerIconUI : MonoBehaviour {
	[SerializeField]
	Image glowBackground;
	[SerializeField]
	Image powerupImage;

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

	public void SetPowerup(Powerup powerup) {
		if (powerup == null) {
			powerupImage.enabled = false;
			return;
		}
		
		powerupImage.enabled = true;
		powerupImage.sprite = powerup.sprite;
	}
}
