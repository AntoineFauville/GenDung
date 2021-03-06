﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExploUnitController : MonoBehaviour {

    private int tileX, tileY; // Position du Joueur en X, en Y.
    private List<Node> currentPath = null; // Liste des noeuds pour le PathFinding.
    private int characterID, health, maxHealth, pm, pa, attackCost = 1, initiative, turnCount; // ID, PV, Max PV, PM, PA, coût d'une attaque, Portée Maximale, Portée Minimale, Compteur de Tours.
    private float remainingMovement = 1;

    private Explo_Range unitRange;
    private Explo_Room_FightController fightRoom;
	private Explo_Room_ExitController exitRoom;
	private Explo_Room_TreasureController treasorRoom;
    private bool alreadyClicked = false;

	//private bool updatePosCube;

    void Start()
    {
        unitRange = Resources.Load("ScriptableObject/ExplorationRange_01") as Explo_Range;

		if (SceneManager.GetActiveScene().name != "ExploEditor") // On vérifie que la scene n'est pas l'editeur et que le placement pré-combat a été réalisé.
		{
        fightRoom = GameObject.Find("ExploGridPrefab").GetComponent<Explo_Room_FightController>();
		exitRoom = GameObject.Find("EndExploScripts").GetComponent<Explo_Room_ExitController>();
		treasorRoom = GameObject.Find("ExploGridPrefab").GetComponent<Explo_Room_TreasureController>();
		}
    }

    void Update()
    {
        if (currentPath != null)
        {
            int currNode = 0;

            while (currNode < currentPath.Count - 1)
            {
                Vector3 start = Explo_GridController.Instance.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);
                Vector3 end = Explo_GridController.Instance.TileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);
                currNode++;
            }
        }

        if (SceneManager.GetActiveScene().name == "ExploEditor") // On vérifie que la scene n'est pas l'editeur et que le placement pré-combat n'as pas été réalisé.
        {
            SetDefaultSpawn(GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position);
            AdvancePathing();
        }
        else if (SceneManager.GetActiveScene().name != "ExploEditor") // On vérifie que la scene n'est pas l'editeur et que le placement pré-combat a été réalisé.
        {
            AdvancePathing();
            transform.position = Vector3.Lerp(transform.position, GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime);
        }

		//if (updatePosCube) {
			//GameObject.Find ("ExploUnit(Clone)/Cube").transform.position = Vector3.Lerp (GameObject.Find ("ExploUnit(Clone)/Cube").transform.position, GameObject.Find ("ExploUnit(Clone)/Unit").transform.position, 0.2f);
		//}


    }

    public void AdvancePathing() // Méthode de déplacement du personnage.
    {
        if (currentPath == null) // Cas où le chemin est null, on arrete l'éxécution de la méthode.
        {
            return;
        }

        if (remainingMovement <= 0)
            return;

        remainingMovement -= Explo_GridController.Instance.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);

        tileX = currentPath[1].x;
        tileY = currentPath[1].y;

        //Debug.Log(Explo_GridController.Instance.Grid.ExploTiles[tileX, tileY].Type); // Display Tile Type for Debug Purpose.

        DiscoverDungeon();

        StartCoroutine(WaitBeforeNextMovement()); // Coroutine pour faire patienter le joueur et donné une meilleure impression de déplacement.

        currentPath.RemoveAt(0); // on retire la case précedente de la liste.

        if (currentPath.Count == 1) // on vérifie si il ne reste pas que la case de destination.
        {
            currentPath = null;
        }

        StartCoroutine(WaitBeforeNextTile());
    }

    public void SetDefaultSpawn(Vector3 pos)
    {
        this.transform.position = pos;
    }

    public void FirstDiscoverDungeon()
    {
        Explo_GridController.Instance.Grid.ExploTiles[Mathf.RoundToInt(Explo_GridController.Instance.EntranceTile.x), Mathf.RoundToInt(Explo_GridController.Instance.EntranceTile.y)].State = Explo_Tile.Explo_TileState.Discovered;
        GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + Explo_GridController.Instance.EntranceTile.x + "_" + Explo_GridController.Instance.EntranceTile.y).GetComponent<ExploTileController>().UpdateTileUI();
        DiscoverDungeon();
    }

    public void DiscoverDungeon()
    {
        StupeflipTile();

		//UpdatePositionCube ();

		StartCoroutine ("WaitForFlipAnim");

		for (int i = 0; i < unitRange.exploTileRange.Count; i++)
        {
            if(Explo_GridController.Instance.Grid.ExploTiles[Mathf.RoundToInt(this.tileX + unitRange.exploTileRange[i].x), Mathf.RoundToInt(this.tileY + unitRange.exploTileRange[i].y)].State == Explo_Tile.Explo_TileState.Undiscovered)
            {
                Explo_GridController.Instance.Grid.ExploTiles[Mathf.RoundToInt(this.tileX + unitRange.exploTileRange[i].x), Mathf.RoundToInt(this.tileY + unitRange.exploTileRange[i].y)].State = Explo_Tile.Explo_TileState.ToBeOrNotToBeDiscovered;
                GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + (this.tileX + unitRange.exploTileRange[i].x) + "_" + (this.tileY + unitRange.exploTileRange[i].y)).GetComponent<ExploTileController>().UpdateTileUI();

				Animator TyleAnim;
				TyleAnim = GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + tileX + "_" + tileY).transform.GetComponent<Animator> ();
				TyleAnim.Play ("flipTile");
            }
        }
    }

    public void StupeflipTile()
    {
        Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].State = Explo_Tile.Explo_TileState.Discovered;
        GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + this.tileX + "_" + this.tileY).GetComponent<ExploTileController>().UpdateTileUI();

		if (Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].Type != Explo_Tile.Explo_TileType.Wall)
		{
			switch (Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].Type)
			{
			case Explo_Tile.Explo_TileType.Fight:
				if (GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles [this.tileX, this.tileY].x + "_" + Explo_GridController.Instance.Grid.ExploTiles [this.tileX, this.tileY].y).GetComponent<ExploTileController> ().isAlreadyDiscovered == false) {

					GameObject.Find("DontDestroyOnLoad").GetComponent<Explo_DataController>().roomImOn = GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].x + "_" + Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].y).GetComponent<ExploTileController> ().indexLocalOfFightRoom;
					GameObject.Find("DontDestroyOnLoad").GetComponent<Explo_DataController>().LaunchFightFreshStart ();
				}
				break;
			}
		}
    }

    public void ResetMovement()
    {
        remainingMovement = 1;
        currentPath = null;
    }

	//public void UpdatePositionCube ()
	//{
	//	StartCoroutine ("WaitUpdateCube");
	//}

    /* IEnumerator Methods*/

    public IEnumerator WaitBeforeNextMovement()
    {
        yield return new WaitForSecondsRealtime(1f);
        transform.position = Vector3.Lerp(transform.position, GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime); // 5f for lerp is also a good speed for movement
    }

    public IEnumerator WaitBeforeNextTile()
    {
        yield return new WaitForSecondsRealtime(0.3f); // 0.3f is perfect for waiting between movement.

        if (Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].Type != Explo_Tile.Explo_TileType.Empty && !GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<ExploTileController>().isAlreadyDiscovered)
        {
            remainingMovement = 0;
        }
        else
            remainingMovement = 1;
    }

    //public IEnumerator WaitUpdateCube()
    //{
    //    if not discovered

    //    yield return new WaitForSecondsRealtime(0.5f);
    //    updatePosCube = true;
    //    yield return new WaitForSecondsRealtime(1f);
    //    updatePosCube = false;
    //}

    public IEnumerator WaitForFlipAnim()
	{
		yield return new WaitForSeconds(1f); 

		if (Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].Type != Explo_Tile.Explo_TileType.Wall)
		{
			switch (Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].Type)
			{
			case Explo_Tile.Explo_TileType.Fight:
				Debug.Log ("Character Entered a Fight Room");

				if (GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].x + "_" + Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].y).GetComponent<ExploTileController> ().isAlreadyDiscovered == false) 
				{
					yield return new WaitForSeconds(0.75f); 
                    if (!alreadyClicked)
                        {
                            alreadyClicked = true;
					        fightRoom.LinkToRoom ();
                        }
                        yield return new WaitForSeconds(1f);
                        alreadyClicked = false;

					GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles [this.tileX, this.tileY].x + "_" + Explo_GridController.Instance.Grid.ExploTiles [this.tileX, this.tileY].y).GetComponent<ExploTileController> ().isAlreadyDiscovered = true;
				}


				break;
			case Explo_Tile.Explo_TileType.Treasure:
				Debug.Log("Character Entered a Treasure Room");

				if (GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].x + "_" + Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].y).GetComponent<ExploTileController> ().isAlreadyDiscovered == false) 
				{
					yield return new WaitForSeconds(1f); // 0.3f is perfect for waiting between movement.
					treasorRoom.LinkToRoom (this.tileX, this.tileY);
				}

				break;

			case Explo_Tile.Explo_TileType.Exit:
				Debug.Log ("Character Entered an Exit Room");

				exitRoom.LinkToRoom ();

				break;
			}
		}
	}

    /* */

    /* Accessors Methods*/

    public int ID
    {
        get
        {
            return characterID;
        }
        set
        {
            characterID = value;
        }
    }

    public int TileX
    {
        get
        {
            return tileX;
        }
        set
        {
            tileX = value;
        }
    }

    public int TileY
    {
        get
        {
            return tileY;
        }
        set
        {
            tileY = value;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    public int PM
    {
        get
        {
            return pm;
        }
        set
        {
            pm = value;
        }
    }

    public int PA
    {
        get
        {
            return pa;
        }
        set
        {
            pa = value;
        }
    }

    public int Initiative
    {
        get
        {
            return initiative;
        }
        set
        {
            initiative = value;
        }
    }
    public List<Node> CurrentPath
    {
        get
        {
            return currentPath;
        }
        set
        {
            currentPath = value;
        }
    }

    /**/
}
