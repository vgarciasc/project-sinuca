using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour {
	[SerializeField]
	GameObject blockingWallPreviewPrefab;

	PowerupData powerup;
	public BlockingWallPreview blockingWallPreview { get; private set; }
	public SelectableObstacle selected { get; private set; }

	public void Init(PowerupData powerup) {
		this.powerup = powerup;
		if (powerup != null && powerup.kind == PowerupEnum.HAND_OF_BLOCKING) {
			var obj = Instantiate(blockingWallPreviewPrefab, this.transform.position, Quaternion.identity);
			blockingWallPreview = obj.GetComponentInChildren<BlockingWallPreview>();
		}
	}

	void Update () {
		HandlePosition();

		if (powerup != null && powerup.kind == PowerupEnum.HAND_OF_DESTRUCTION) {
			HandleSelection();
		}
	}

	void HandlePosition() {
		this.transform.position = Input.mousePosition;
	}

	void HandleSelection() {
		RaycastHit hit;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(mouseRay, out hit)) {
			var obj = hit.collider.gameObject;
			var selectable = obj.GetComponentInChildren<SelectableObstacle>();

			if (selectable != null) {
				if (selected == null) {
					selectable.ToggleSelection(true);
				}
				selected = selectable;
			}
			else {
				if (selected != null) {
					selected.ToggleSelection(false);
					selected = null;
				}
			}
		}
	}

	public void Destroy() {
		if (blockingWallPreview) {
			Destroy(blockingWallPreview);
		}

		Destroy(this.gameObject);
	}
}
