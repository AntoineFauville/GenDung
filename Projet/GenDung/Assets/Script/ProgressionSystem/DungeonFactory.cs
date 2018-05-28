using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory : MonoBehaviour {

	public Pdungeon CreateDungeon(int dungeonType)
	{
	    switch (dungeonType)
	    {
	        case 0:
	            return new PDungeonForest();
	        case 1:
	            return new PDungeonDesert ();
	        default:
	            return null;
	    }
	}
}
