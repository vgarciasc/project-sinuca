using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerScoreBallUI : MonoBehaviour {

	[SerializeField]
	Image image;

	public void Toggle(bool value) {
		image.color = HushPuppy.getColorWithOpacity(
			image.color,
			value ? 1f : 0.5f);
	}
}
