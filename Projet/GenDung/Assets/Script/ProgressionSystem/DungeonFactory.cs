using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory : MonoBehaviour {

	public Pdungeon CreateDungeon(int dungeonType){
		
		if (dungeonType.Equals (0)){
            return new PDungeonForest();
		} else if (dungeonType.Equals (1)) {
			return new PDungeonDesert ();
		} else
			return null;
	}
}
