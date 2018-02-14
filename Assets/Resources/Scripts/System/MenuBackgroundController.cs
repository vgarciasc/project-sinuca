using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackgroundController : MonoBehaviour {

	public List<GameObject> balls;
	public float timeBetweenShoots = 1f;

	//audioclip

	private void Start(){
		StartCoroutine( BallShooter() );
	}
	private IEnumerator BallShooter(){
		while(true){
			Vector3 dir = 100f*(Vector3.up*Random.Range(-1f,1f) + Vector3.right*Random.Range(-1f,1f));
			balls[ Random.Range(0,balls.Count) ].GetComponent<Rigidbody>().AddForce(dir);
			//play audioclip
			yield return new WaitForSeconds(timeBetweenShoots);
		}
	}
}
