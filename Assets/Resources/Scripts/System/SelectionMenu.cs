using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectionMenu : MonoBehaviour {
	GamePreferencesDatabase gpd;

	[Header("Players")]
	public Slider[] color = new Slider[2];
	public Slider[] texture = new Slider[2];
	public Button[] ready = new Button[2];

	[Header("Table")]
	public Image image;
	public Slider type;
	public Toggle modifiers;
	public Toggle powerUps;

	[Header("Ready?")]
	public bool[] playerReady = {false,false};
	public GameObject[] balls = new GameObject[2];
	public Color[] readyColor = {Color.white, Color.yellow};

	private void Start(){
		gpd = GamePreferencesDatabase.GetGamePreferencesDatabase();
		SetSlidersMaxValues();

		SetColor(0);
		SetColor(1);

		SetTexture(0);
		SetTexture(1);

		SetTableImage();
	}
	private void Update(){
		balls[0].transform.Rotate(20*Time.deltaTime,20*Time.deltaTime,0);
		balls[1].transform.Rotate(20*Time.deltaTime,20*Time.deltaTime,0);
	}
	public void SetPlayerPreference(){
		gpd.gamePreferences.texture[0] = (int) texture[0].value;
		gpd.gamePreferences.texture[1] = (int) texture[1].value;
		gpd.gamePreferences.color[0] = (int) color[0].value;
		gpd.gamePreferences.color[1] = (int) color[1].value;

		gpd.gamePreferences.type = (int)type.value;
		gpd.gamePreferences.modifiers = modifiers.isOn;
		gpd.gamePreferences.powerUps = powerUps.isOn;

		DOTween.Init();
		gpd.transitionImg.raycastTarget = true;
		gpd.transitionImg.DOFade(1f,0.5f);
		transform.DOPlay();

		SceneManager.LoadScene("MainScene");
	}
	public void CheckIfReady(){
		if(playerReady[0] && playerReady[1]){
			SetPlayerPreference();
		}
	}
	public void SetReady(int playerId){
		playerReady[playerId] = !playerReady[playerId];
		CheckIfReady();
		if(playerReady[playerId]){
			ready[playerId].image.color = readyColor[1];
		}else{
			ready[playerId].image.color = readyColor[0];
		}
	}
	public void SetReadyFalse(int playerId){
		playerReady[playerId] = false;
		ready[playerId].image.color = readyColor[0];
	}
	public void SetTexture(int playerId){
		balls[playerId].GetComponent<MeshRenderer>().material.mainTexture = gpd.textures[ (int)texture[playerId].value];
		SetReadyFalse(playerId);
	}
	public void SetColor(int playerId){
		balls[playerId].GetComponent<MeshRenderer>().material.color = gpd.colors[ (int)color[playerId].value ];
		SetReadyFalse(playerId);
	}
	public void SetTableImage(){
		image.sprite = gpd.tableType[(int)type.value];
	}
	public void SetSlidersMaxValues(){
		color[0].maxValue = gpd.colors.Count - 1;
		color[1].maxValue = gpd.colors.Count - 1;

		texture[0].maxValue = gpd.textures.Count - 1;
		texture[1].maxValue = gpd.textures.Count - 1;

		type.maxValue = gpd.tableType.Count - 1;
	}
	public void ExitGame(){
		Application.Quit();
	}
}
