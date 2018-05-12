using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {

	public List<PlayerStatus> AllStatus = new List<PlayerStatus>();

	public List<string> effect_List = new List<string> ();

	void Start()
	{
		effect_List.Add ("Effect_None"); //0
		effect_List.Add ("Effect_Healing"); //1
 		effect_List.Add ("Effect_Poisonned"); //2
		effect_List.Add ("Effect_Spikey"); //3
		effect_List.Add ("Effect_Rooted"); //4 
		effect_List.Add ("Effect_Projectile"); //5
	}
}
