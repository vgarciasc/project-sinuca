using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupEnum { NONE, HAND_OF_DESTRUCTION, HAND_OF_BLOCKING, HAND_OF_CONFUSION };

[CreateAssetMenu(menuName = "Powerup")]
public class Powerup : ScriptableObject {
	public PowerupEnum kind = PowerupEnum.NONE;
	public string name = "DEFAULT";
	public Sprite sprite;
}
