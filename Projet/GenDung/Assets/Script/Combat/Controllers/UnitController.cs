using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnitController : MonoBehaviour {

    private int tileX, tileY, tileAttackX, tileAttackY; // Position du Joueur en X, en Y ainsi que Position en X , en Y de la tile pour l'attaque.
    private List<Node> currentPath = null; // Liste des noeuds pour le PathFinding.
    private bool attacking = false; // Booléen vérifiant si l'on attaque ou pas.
    private int characterID, health, maxHealth, pm, pa, attackCost = 1, initiative, turnCount; // ID, PV, Max PV, PM, PA, coût d'une attaque, Portée Maximale, Portée Minimale, Compteur de Tours.
    public float remainingMovement = 99, remainingAction = 5; // Compte de déplacement restant (99 pour la phase de placement) , Compte de PA restant.
    private SpellObject[] playerSpells; // liste des sorts du personnage.

    private bool changePosition = false;

    private GameObject spellCanvasPrefab;
    private GameObject spellCanvas;

    void Start()
    {
		print ("unit well created");
        if(SceneManager.GetActiveScene().name != "Editor")
        {
            GameObject.Find("ImageFondPassYourTurn").GetComponent<Animator>().enabled = false;
            GameObject.Find("ImageFondPassYourTurn").GetComponent<Image>().enabled = false;
        }
    }

    void Update ()
    {
	    if(currentPath != null)
        {
            int currNode = 0;

            while (currNode < currentPath.Count-1)
            {
                Vector3 start = GridController.Instance.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);
                Vector3 end = GridController.Instance.TileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);
                currNode++;
            }
        }
        
        if (SceneManager.GetActiveScene().name != "Editor" && PreCombatController.Instance.CombatStarted && CombatController.Instance.Turn == CombatController.turnType.Player) // On vérifie que la scene n'est pas l'editeur et que le placement pré-combat a été réalisé.
        {
            AdvancePathing();
            transform.position = Vector3.Lerp(transform.position, GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime);
        }
        else if (SceneManager.GetActiveScene().name != "Editor" && !PreCombatController.Instance.CombatStarted) // On vérifie que la scene n'est pas l'editeur et que le placement pré-combat n'as pas été réalisé.
        {
            SetDefaultSpawn(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position);
            AdvancePathing();
        }
    }

    public void AdvancePathing() // Méthode de déplacement du personnage.
    {
        if (!PreCombatController.Instance.CombatStarted && this.characterID == PreCombatController.Instance.LocalIndex) // On vérifie si le placement pré-combat a été fait ou pas (ainsi, on téléporte le joueur sur la case cliquée pour le placement pré-combat). 
        {
            if(changePosition)
            transform.position = GridController.Instance.WorldPosTemp;
        }

        if (currentPath == null) // Cas où le chemin est null, on arrete l'éxécution de la méthode.
        {
            return;
        }

       if (remainingMovement <= 0) // on vérifie si le personnage a encore des PM.
        {
            //Debug.Log("Not enough movement point left, wait for the next turn");
            return;
        }

        remainingMovement -= GridController.Instance.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y); // on retire le coût du déplacement par case.

        tileX = currentPath[1].x;
        tileY = currentPath[1].y;

        GridController.Instance.Grid.Tiles[tileX, tileY].Type = Tile.TileType.Occupied;
        GridController.Instance.Grid.Tiles[currentPath[0].x, currentPath[0].y].Type = Tile.TileType.Floor;

        if (PreCombatController.Instance.CombatStarted)
            GameObject.Find("DontDestroyOnLoad").GetComponent<BuffIndicatorGestion>().GetBuffIndicator(0,3,Mathf.RoundToInt(24),0f);

        StartCoroutine(WaitBeforeNextMovement()); // Coroutine pour faire patienter le joueur et donné une meilleure impression de déplacement.

        currentPath.RemoveAt(0); // on retire la case précedente de la liste.

        if (currentPath.Count == 1) // on vérifie si il ne reste pas que la case de destination.
        {
            currentPath = null;
            CombatController.Instance.SetMovementRangeOnGrid();
        }
    }

    public void Attack(int s, int xPos, int yPos) // méthode d'attaque du personnage.
    {

        // ajouter self (spellType ==> spell 3 : buff) + verif pos identique à pos unit + ajouter indicateur buff pv 

        //attacking = false;

        if (remainingAction <= 0)
        {
            Debug.Log("No action points left, no action done");
            CombatController.Instance.ActualCombatState = CombatController.combatState.Movement;
            Debug.Log("Switching Back to Movement Mode");
            return;
        }

        if (remainingAction >= attackCost)
        {
            remainingAction -= attackCost;
            CombatController.Instance.SpellUsable(remainingAction);
            Debug.Log("Action points left : " + remainingAction);

            GameObject.Find("ImageFondPassYourTurn").GetComponent<Animator>().enabled = true;
            GameObject.Find("ImageFondPassYourTurn").GetComponent<Image>().enabled = true;
            //GameObject.Find("ButtonPassYourTurn").GetComponent<Image>().color = Color.white;
            //GameObject.Find("ButtonPassYourTurn").GetComponent<Button>().interactable = true;
            //GameObject.Find("ButtonPassYourTurn").GetComponent<Button>().onClick.AddListener(NextTurn);

            // Check Spell Type Loutre Manger Cachuétes.
            if (playerSpells[s].spellType == SpellObject.SpellType.Distance)
            {
                spellCanvasPrefab = playerSpells[s].spellPrefab;
                spellCanvas = Instantiate(spellCanvasPrefab);
                spellCanvas.transform.Find("Unit").transform.position = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + xPos + "_" + yPos).transform.position;
                PostCombatController.Instance.SpellCanvasInstantiated.Add(spellCanvas);
            }

            GameObject.Find("DontDestroyOnLoad").GetComponent<BuffIndicatorGestion>().GetBuffIndicator(0, 1, playerSpells[s].spellCost,0f);

            StartCoroutine(WaitForAttackCompletion(playerSpells[s].SpellCastAnimationTime, xPos, yPos, s));
        }
    }

    public void TakeDamage(int value)
    {
        health -= value;
        Debug.Log("Ouch!!!"); 
    }

    public bool CheckPA()
    {
        if (remainingAction >= attackCost)
            return true;
        else
            return false;
    }

    public void ResetMove()
    {
        remainingMovement = pm;
        GameObject.Find("DontDestroyOnLoad").GetComponent<BuffIndicatorGestion>().GetBuffIndicator(0, 2, pm,0f);
    }

    public void ResetAction()
    {
        remainingAction = pa;
        GameObject.Find("DontDestroyOnLoad").GetComponent<BuffIndicatorGestion>().GetBuffIndicator(0, 0, pm,1f);
    }

    public void SetDefaultSpawn(Vector3 pos)
    {
        this.transform.position = pos;
    }

    /* IEnumerator Methods*/
    public IEnumerator WaitForFoeEndTurn()
    {
        Debug.Log("Simulating Foe Turn");
        GameObject.Find("YourTurnPanel/Panel").GetComponent<Animator>().Play("yourturngo");
        GameObject.Find("TextYourTurn").GetComponent<Text>().text = "ENNEMI TURN";
        yield return new WaitForSeconds(0.2f);
        turnCount++;

        //ajout de l'animation de ton tour
        GameObject.Find("YourTurnPanel/Panel").GetComponent<Animator>().Play("yourturngo");
        GameObject.Find("TextYourTurn").GetComponent<Text>().text = "YOUR TURN";
        CombatController.Instance.NextEntityTurn();
        CombatController.Instance.SetMovementRangeOnGrid();

        Debug.Log("Begin Turn: " + turnCount);
    }

    public IEnumerator WaitBeforeNextMovement()
    {
        yield return new WaitForSecondsRealtime(1f);
        transform.position = Vector3.Lerp(transform.position, GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime);
    }

    public IEnumerator WaitForAttackCompletion(float t, int _x, int _y, int s)
    {
        CombatController.Instance.CleanActualSpellRange();
        // Play Attack Animation.
        this.transform.Find("Cube/Image").GetComponent<Animator>().Play(playerSpells[s].spellAnimator.ToString());
        yield return new WaitForSeconds(t);
        Debug.Log("Switching Back to Movement Mode");
        if(spellCanvas != null)
            spellCanvas.transform.Find("Unit").gameObject.SetActive(false);
        CombatController.Instance.ActualCombatState = CombatController.combatState.Movement;
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

    public SpellObject[] PlayerSpells
    {
        get
        {
            return playerSpells;
        }
        set
        {
            playerSpells = value;
        }
    }

    public int getTileAttackX()
    {
        return tileAttackX;
    }

    public void setTileAttackX(int _x)
    {
        tileAttackX = _x;
    }

    public int getTileAttackY()
    {
        return tileAttackY;
    }

    public void setTileAttackY(int _y)
    {
        tileAttackY = _y;
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

    public bool Attacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
        }
    }

    /**/
}
