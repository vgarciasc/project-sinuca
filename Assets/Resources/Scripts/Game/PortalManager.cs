using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

	List<Portal> portals = new List<Portal>();

	void Start() {
		PopulateExits();
	}

	void PopulateExits() {
		foreach (Transform t in transform) {
			Portal portal = t.GetComponent<Portal>();
			if (portal != null) {
				portals.Add(portal);
			}
		}
	}

	public Portal GetExit(Portal portal) {
		foreach (Portal p in portals) {
			if (p != portal) {
				return p;
			}
		}

		print("This shouldn't be happening.");
		return null;
	}
}
