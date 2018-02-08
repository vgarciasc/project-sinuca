using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : MonoBehaviour {
	[SerializeField]
	GameObject cueStickPrefab;

	Rigidbody2D rigidbody;
	
	Ball ball;
	CueStick cueStick;

	void Start() {
		ball = this.GetComponentInParent<Ball>();
		rigidbody = GetComponentInParent<Rigidbody2D>();

		InitCueStick();
	}

	void InitCueStick() {
		var obj = Instantiate(
			cueStickPrefab, 
			this.transform.position, 
			this.transform.rotation);
		obj.transform.SetParent(HushPuppy.safeFind("World").transform);
		var cs = obj.GetComponentInChildren<CueStick>();
		cs.SetPlayerBall(this);

		cueStick = cs;
	}

	void Update() {
		if (cueStick != null) {
			cueStick.gameObject.SetActive(rigidbody.velocity.magnitude < 0.1f);
		}
	}

	public void Shoot(float angle, float intensity) {
		Vector2 direction = 
			new Vector2(
				Mathf.Cos(angle), 
				Mathf.Sin(angle)).normalized 
			* intensity
			* Mathf.Rad2Deg;

		rigidbody.AddForce(direction);
	}
}
