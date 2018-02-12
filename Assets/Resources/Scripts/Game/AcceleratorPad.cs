using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratorPad : MonoBehaviour {

	[Header("Mechanics")]
	[SerializeField]
	[Range(0f, 10f)]
	float intensity = 5f;

	void OnTriggerEnter(Collider collider) {
		var obj = collider.gameObject;
		if (obj.tag == "Ball") {
			var ball = obj.GetComponentInChildren<Ball>();
			ball.Boost(intensity);
		}
	}
}
