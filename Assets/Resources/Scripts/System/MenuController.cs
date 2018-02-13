using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuController : MonoBehaviour {

	[SerializeField] private List<Transform> cameraPositions;

	public void ChangeCameraPosition(int i){
		StartCoroutine( InterpolateToTransform(cameraPositions[i]) );
	}
	private IEnumerator InterpolateToTransform(Transform t){
		//Transita para a posicao t.position e a rotacao t.rotation e em escala t.scale
		DOTween.Init();
		transform.DOMove(t.localPosition,1f);
		transform.DOPlay();
		yield return new WaitForSeconds(1f);
	}
}
public enum CameraState{
	MAIN_MENU,SELECTION_MENU
}
