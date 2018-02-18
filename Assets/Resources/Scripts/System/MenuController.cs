using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuController : MonoBehaviour {

	[SerializeField] private List<Transform> cameraPositions;
	[SerializeField] private List<GameObject> tutorialPanels;

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
	public void TutorialPanel(){
		StartCoroutine( TutorialPanelAnimation() );
	}
	private IEnumerator TutorialPanelAnimation(){
		float unit = tutorialPanels[0].transform.localScale.y;
		foreach(GameObject go in tutorialPanels){
			go.transform.localScale = new Vector3(0,unit,unit);
		}
		yield return new WaitForSeconds(0.3f);
		foreach(GameObject go in tutorialPanels){
			go.transform.DOScaleX(unit,0.3f);
			yield return new WaitForSeconds(0.3f);
		}
	}
}
public enum CameraState{
	MAIN_MENU,SELECTION_MENU
}
