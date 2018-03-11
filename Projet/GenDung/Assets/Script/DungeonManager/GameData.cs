using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSaveData", menuName = "SaveGameSystem/GameSaveData", order = 1)]
public class GameData : ScriptableObject {

	public int DungeonIndexData = 1; // to know how much we have unlocked yet to load the map proprelly.

	public Character[] SavedCharacterList;
	public int SavedSizeOfTheTeam = 1;

    public int PlayerMoney = 0;

	public int totalAmountDungeons = 5;
	public bool[] dungeonUnlocked;
}
