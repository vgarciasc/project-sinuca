using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public AudioSource myAS;
	public List<AudioClip> audioClips;

	public void PlayAudioClip(Sfx sfx){
		myAS.clip = audioClips[sfx.GetHashCode()];
		myAS.Play();
	}
}
public enum Sfx{
	HIT_BALL,CHAIN,END_GAME,GET_POWERUP,INTO_POCKET,PORTAL,WALL_SWITCH
}
