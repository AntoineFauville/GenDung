using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSaveData", menuName = "SaveGameSystem/GameSaveData", order = 1)]
public class GameData : ScriptableObject {

	public int DungeonIndexData = 1;

	public Character[] SavedCharacterList;
	public int SavedSizeOfTheTeam = 1;

    public int PlayerMoney = 0;

}
