using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class PlayerScoreBallUI : MonoBehaviour {

	[SerializeField]
	Image image;

	float fadeOut = 0.3f;
	float fadeIn = 1f;

	public void Toggle(bool value, bool animation = false) {
		if (animation && !value) {
			StartCoroutine(ScoreAnimation());
		} else {
			image.color = HushPuppy.getColorWithOpacity(
				image.color,
				value ? fadeIn : fadeOut);
		}
	}

	public void SetPlayerColor(Color color) {
		image.color = color + new Color(0.3f, 0.3f, 0.3f);
	}

	IEnumerator ScoreAnimation() {
		image.DOFade(fadeOut, 0.2f);
		transform.DOLocalMoveY(transform.localPosition.y + 5f, 0.2f);
		yield return new WaitForSeconds(0.2f);
		transform.DOLocalMoveY(transform.localPosition.y - 5f, 0.2f);
		yield return new WaitForSeconds(0.2f);
	}
}
