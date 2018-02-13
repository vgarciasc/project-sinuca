using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(menuName = "GamePreferences")]
public class GamePreferences : ScriptableObject {
	[Header("Players Preferences")]
	public int[] color = {1,1};
	public int[] texture = {1,1};
	[Header("Table Preferences")]
	public int type = 1;
	public bool modifiers;
	public bool powerUps;
	
}
