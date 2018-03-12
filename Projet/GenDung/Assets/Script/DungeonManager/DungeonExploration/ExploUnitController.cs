using System.Collections;
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
    private Explo_FightRoom fightRoom;
	private Explo_ExitRoom exitRoom;
	private Explo_TresorRoom treasorRoom;


    void Start()
    {
        unitRange = Resources.Load("ScriptableObject/ExplorationRange_01") as Explo_Range;
        fightRoom = GameObject.Find("ExploGridPrefab").GetComponent<Explo_FightRoom>();
		exitRoom = GameObject.Find("EndExploScripts").GetComponent<Explo_ExitRoom>();
		treasorRoom = GameObject.Find("ExploScriptsTreasure").GetComponent<Explo_TresorRoom>();
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

        for (int i = 0; i < unitRange.exploTileRange.Count; i++)
        {
            if(Explo_GridController.Instance.Grid.ExploTiles[Mathf.RoundToInt(this.tileX + unitRange.exploTileRange[i].x), Mathf.RoundToInt(this.tileY + unitRange.exploTileRange[i].y)].State == Explo_Tile.Explo_TileState.Undiscovered)
            {
                Explo_GridController.Instance.Grid.ExploTiles[Mathf.RoundToInt(this.tileX + unitRange.exploTileRange[i].x), Mathf.RoundToInt(this.tileY + unitRange.exploTileRange[i].y)].State = Explo_Tile.Explo_TileState.ToBeOrNotToBeDiscovered;
                GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + (this.tileX + unitRange.exploTileRange[i].x) + "_" + (this.tileY + unitRange.exploTileRange[i].y)).GetComponent<ExploTileController>().UpdateTileUI();
            }
        }
    }

    public void StupeflipTile()
    {
        Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].State = Explo_Tile.Explo_TileState.Discovered;
        GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + this.tileX + "_" + this.tileY).GetComponent<ExploTileController>().UpdateTileUI();

		StartCoroutine ("WaitForFlipAnim");
		Animator TyleAnim;
		TyleAnim = GameObject.Find ("ExploGridCanvas").transform.Find ("PanelGrid/Tile_" + tileX + "_" + tileY).transform.GetComponent<Animator> ();
		TyleAnim.Play ("flipTile");
    }

    /* IEnumerator Methods*/

    public IEnumerator WaitBeforeNextMovement()
    {
        yield return new WaitForSecondsRealtime(1f);
        transform.position = Vector3.Lerp(transform.position, GameObject.Find("ExploGridCanvas").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime); // 5f for lerp is also a good speed for movement
    }

    public IEnumerator WaitBeforeNextTile()
    {
        yield return new WaitForSecondsRealtime(0.3f); // 0.3f is perfect for waiting between movement.
        remainingMovement = 1;
    }

	public IEnumerator WaitForFlipAnim()
	{
		yield return new WaitForSecondsRealtime(0.3f); // 0.3f is perfect for waiting between movement.

		if (Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].Type != Explo_Tile.Explo_TileType.Wall)
		{
			switch (Explo_GridController.Instance.Grid.ExploTiles[this.tileX, this.tileY].Type)
			{
			case Explo_Tile.Explo_TileType.Fight:
				Debug.Log("Character Entered a Treasure Room");


				fightRoom.LinkToRoom();


				break;
			case Explo_Tile.Explo_TileType.Treasure:
				Debug.Log("Character Entered a Treasure Room");

				treasorRoom.LinkToRoom ();

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
