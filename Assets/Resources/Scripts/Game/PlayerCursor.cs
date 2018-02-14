using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Powerup { NONE, HAND_OF_DESTRUCTION, HAND_OF_BLOCKING };
public class PlayerCursor : MonoBehaviour {
	public GameObject selected { get; private set; }

	[SerializeField]
	GameObject blockingWallPreviewPrefab;

	Powerup powerup;
	BlockingWallPreview blockingWallPreview;

	public void Init(Powerup powerup) {
		this.powerup = powerup;
		if (powerup == Powerup.HAND_OF_BLOCKING) {
			Instantiate(blockingWallPreviewPrefab, this.transform.position, Quaternion.identity);
		}
	}

	void Update () {
		HandlePosition();
		HandleSelection();
	}

	void HandlePosition() {
		this.transform.position = Input.mousePosition;
	}

	void HandleSelection() {
		RaycastHit hit;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(mouseRay, out hit, LayerMask.GetMask("BlockingWall"))) {
			if (hit.collider != null && hit.collider.gameObject.tag == "Obstacle") {
				if (selected == null) {
					selected = hit.collider.gameObject;
					selected.GetComponentInChildren<BlockingWall>().ToggleSelect(true);
				}

				selected = hit.collider.gameObject;
			} else {
				if (selected != null && selected.GetComponentInChildren<BlockingWall>() != null) {
					selected.GetComponentInChildren<BlockingWall>().ToggleSelect(false);
					selected = null;
				}
			}
		}
	}

	public void UsePowerup(Powerup power) {
		switch (power) {
			case Powerup.HAND_OF_DESTRUCTION:
				if (selected != null) {
					selected.GetComponentInChildren<Destructable>().DeactivateMe();
				}
				break;
			case Powerup.HAND_OF_BLOCKING:
				if (selected != null) {
					selected.GetComponentInChildren<Destructable>().DeactivateMe();
				}
				break;
		}
	}
}
