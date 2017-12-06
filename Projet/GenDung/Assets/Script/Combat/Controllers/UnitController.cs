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

    private GameObject spellCanvasPrefab;
    private GameObject spellCanvas;

    void Start()
    {
        if(SceneManager.GetActiveScene().name != "Editor")
        {
            GameObject.Find("ImageFondPassYourTurn").GetComponent<Animator>().enabled = false;
            GameObject.Find("ImageFondPassYourTurn").GetComponent<Image>().enabled = false;
            GameObject.Find("ButtonPassYourTurn").GetComponent<Image>().color = Color.grey;
            GameObject.Find("ButtonPassYourTurn").GetComponent<Button>().interactable = false;
        }
    }

    void Update ()
    {
	    if(currentPath != null)
        {
            int currNode = 0;

            while (currNode < currentPath.Count-1)
            {
                Vector3 start = DungeonController.Instance.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);
                Vector3 end = DungeonController.Instance.TileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);

                //Debug.DrawLine(start, end, Color.red);
                currNode++;
            }
        }
        
        if (SceneManager.GetActiveScene().name != "Editor" && CombatController.Instance.CombatStarted) // On vérifie que la scene n'est pas l'editeur et que le placement pré-combat a été réalisé.
        {
            AdvancePathing();
            transform.position = Vector3.Lerp(transform.position, GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime);
        }
        else if (SceneManager.GetActiveScene().name != "Editor" && !CombatController.Instance.CombatStarted) // On vérifie que la scene n'est pas l'editeur et que le placement pré-combat n'as pas été réalisé.
        {
            SetDefaultSpawn(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position);
            AdvancePathing();
        }
    }

    public void AdvancePathing() // Méthode de déplacement du personnage.
    {
        if (!CombatController.Instance.CombatStarted) // On vérifie si le placement pré-combat a été fait ou pas (ainsi, on téléporte le joueur sur la case cliquée pour le placement pré-combat). 
        {
            transform.position = DungeonController.Instance.WorldPosTemp;
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

        remainingMovement -= DungeonController.Instance.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y); // on retire le coût du déplacement par case.

        tileX = currentPath[1].x;
        tileY = currentPath[1].y;

        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).GetComponent<TileController>().Occupied = true;
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + currentPath[0].x + "_" + currentPath[0].y).GetComponent<TileController>().Occupied = false;

        if (CombatController.Instance.CombatStarted)
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
            CombatController.Instance.AttackMode = false;
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
            GameObject.Find("ButtonPassYourTurn").GetComponent<Image>().color = Color.white;
            GameObject.Find("ButtonPassYourTurn").GetComponent<Button>().interactable = true;
            GameObject.Find("ButtonPassYourTurn").GetComponent<Button>().onClick.AddListener(NextTurn);

            // Check Spell Type Loutre Manger Cachuétes.
            if (playerSpells[s].spellType == SpellObject.SpellType.Distance)
            {
                spellCanvasPrefab = playerSpells[s].spellPrefab;
                spellCanvas = Instantiate(spellCanvasPrefab);
                spellCanvas.transform.Find("Unit").transform.position = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + xPos + "_" + yPos).transform.position;
                CombatController.Instance.SpellCanvasInstantiated.Add(spellCanvas);
            }

            GameObject.Find("DontDestroyOnLoad").GetComponent<BuffIndicatorGestion>().GetBuffIndicator(0, 1, playerSpells[s].spellCost,0f);

            StartCoroutine(WaitForAttackCompletion(playerSpells[s].SpellCastAnimationTime, xPos, yPos, s));
        }
    }

    public bool CheckPA()
    {
        if (remainingAction >= attackCost)
            return true;
        else
            return false;
    }

    public void Test()
    {
        CombatController.Instance.SpellUsable(0);
    }

    public void NextTurn() // Penser à déplacer cette méthode dans le CombatController
    {
        while(currentPath!= null && remainingMovement > 0)
        {
            AdvancePathing();
        }

        GameObject.Find("ImageFondPassYourTurn").GetComponent<Animator>().enabled = false;
        GameObject.Find("ImageFondPassYourTurn").GetComponent<Image>().enabled = false;
        GameObject.Find("ButtonPassYourTurn").GetComponent<Image>().color = Color.grey;
        GameObject.Find("ButtonPassYourTurn").GetComponent<Button>().interactable = false;

        ResetMove();
        ResetAction();
        

        Debug.Log("End of Turn: " + turnCount);

        // Wait a time for simulationg Foe Action Here.
        StartCoroutine(WaitForFoeEndTurn());
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
        yield return new WaitForSeconds(3f);
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
        // Play Attack Animation.
        this.transform.Find("Cube/Image").GetComponent<Animator>().Play(playerSpells[s].spellAnimator.ToString());
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + _x + "_" + _y).GetComponent<TileController>().RemoveRange();
        yield return new WaitForSeconds(t);
        GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + _x + "_" + _y).GetComponent<TileController>().RemoveRange();
        Debug.Log("Switching Back to Movement Mode");
        if(spellCanvas != null)
            spellCanvas.transform.Find("Unit").gameObject.SetActive(false);
        CombatController.Instance.AttackMode = false;
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
