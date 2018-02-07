using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class GenericAnimationFunctions : MonoBehaviour {
	public bool fixRotation;
	public bool rotate;
	public float rotateTime;
	public bool upDownIdle;
	public float upDownIdleDistance;
	public float upDownIdleDelay;
	public bool randomColor;
	public bool crudeAnimation;
	public float crudeAnimationDelay = 0.1f;
	public List<Sprite> crudeAnimationSprites;
	
	Quaternion initialRotation;

	void Update() {
		if (fixRotation) {
			transform.rotation = initialRotation;
		}
	}

	public void DeactivateMyself() {
		this.gameObject.SetActive(false);
	}

	void OnEnable() {
		if (fixRotation) {
			initialRotation = transform.rotation;
		}

		if (rotate) {
			this.transform.DORotate(new Vector3(0, 0, 360), rotateTime, RotateMode.LocalAxisAdd)
				.SetEase(Ease.Linear)
				.SetLoops(-1);
		}

		if (upDownIdle) {
			StartCoroutine(IdleMovement());
		}

		if (randomColor) {
			this.GetComponentInChildren<SpriteRenderer>().color = 
				new Color(
					Random.Range(0, 1f),
					Random.Range(0, 1f),
					Random.Range(0, 1f));
		}

		if (crudeAnimation) {
			StartCoroutine(CrudeAnimationLoop());
		}
	}

	IEnumerator IdleMovement() {
		while (true) {
			if (upDownIdleDelay == 0f) {
				this.transform.DOLocalMoveY(this.transform.localPosition.y + upDownIdleDistance, 0.15f);
				yield return new WaitForSeconds(0.15f);
				this.transform.DOLocalMoveY(this.transform.localPosition.y - upDownIdleDistance, 0.15f);
				yield return new WaitForSeconds(0.2f);
			} else {
				this.transform.DOLocalMoveY(this.transform.localPosition.y + upDownIdleDistance, upDownIdleDelay);
				yield return new WaitForSeconds(upDownIdleDelay);
				this.transform.DOLocalMoveY(this.transform.localPosition.y - upDownIdleDistance, upDownIdleDelay);
				yield return new WaitForSeconds(upDownIdleDelay);
			}
		}
	}

	IEnumerator CrudeAnimationLoop() {
		SpriteRenderer sr = this.GetComponentInChildren<SpriteRenderer>();
		Image img = this.GetComponentInChildren<Image>();

		while (true) {
			foreach (Sprite s in crudeAnimationSprites) {
				if (sr != null) {
					sr.sprite = s;
				} else {
					img.sprite = s;
				}

				yield return new WaitForSeconds(crudeAnimationDelay);
			}
		}
	}
}
