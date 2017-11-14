using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitController : MonoBehaviour {

    private int tileX, tileY, tileAttackX, tileAttackY; // Position du Joueur en X, en Y ainsi que Position en X , en Y de la tile pour l'attaque.
    private List<Node> currentPath = null; // Liste des noeuds pour le PathFinding.
    private bool attacking = false; // Booléen vérifiant si l'on attaque ou pas.
    private int characterID, health, maxHealth, pm, pa, attackCost = 1, rangeMax = 2, rangeMin = 1, turnCount; // ID, PV, Max PV, PM, PA, coût d'une attaque, Portée Maximale, Portée Minimale, Compteur de Tours.
    private float remainingMovement = 99, remainingAction = 5; // Compte de déplacement restant (99 pour la phase de placement) , Compte de PA restant.
    private SpellObject[] playerSpells; // liste des sorts du personnage.

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

        remainingMovement -= DungeonController.Instance.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y); // on retire le coût du déplacement par la case.

        tileX = currentPath[1].x;
        tileY = currentPath[1].y;
        
        StartCoroutine(WaitBeforeNextMovement()); // Coroutine pour faire patienter le joueur et donné une meilleure impression de déplacement.

        currentPath.RemoveAt(0); // on retire la case précedente de la liste.

        if (currentPath.Count == 1) // on vérifie si il ne reste pas que la case de destination.
        {
            currentPath = null;
        }
    }

    public void Attack() // méthode d'attaque du personnage.
    {
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
            Debug.Log("Action points left : " + remainingAction);
            StartCoroutine(WaitForAttackCompletion());
        }
    }

    public void NextTurn() // Penser à déplacer cette méthode dans le CombatController
    {
        while(currentPath!= null && remainingMovement > 0)
        {
            AdvancePathing();
        }

        ResetMove();
        ResetAction();

        Debug.Log("End of Turn: " + turnCount);

        // Wait a time for simulationg Foe Action Here.
        StartCoroutine(WaitForFoeEndTurn());
    }

    public void ResetMove()
    {
        remainingMovement = pm;
    }

    public void ResetAction()
    {
        remainingAction = pa;
    }

    public void SetDefaultSpawn(Vector3 pos)
    {
        this.transform.position = pos;
    }

    /* IEnumerator Methods*/
    public IEnumerator WaitForFoeEndTurn()
    {
        Debug.Log("Simulating Foe Turn");
        yield return new WaitForSeconds(3f);
        turnCount++;
        Debug.Log("Begin Turn: " + turnCount);
    }

    public IEnumerator WaitBeforeNextMovement()
    {
        yield return new WaitForSecondsRealtime(3f);
        transform.position = Vector3.Lerp(transform.position, GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime);
    }

    public IEnumerator WaitForAttackCompletion()
    {
        yield return new WaitForSeconds(3.5f);
        Debug.Log("Switching Back to Movement Mode");
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
