﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitController : MonoBehaviour {

    private int tileX, tileY, tileAttackX, tileAttackY; // Position du Joueur en X, en Y ainsi que Position en X , en Y de la tile pour l'attaque.
    private List<Node> currentPath = null; // Liste des noeuds pour le PathFinding.
    private bool attacking = false; // Booléen vérifiant si l'on attaque ou pas.
    private int health, pm, pa, attackCost = 1, rangeMax = 2, rangeMin = 1, turnCount = -1; // PV, PM, PA, coût d'une attaque, Portée Maximale, Portée Minimale, Compteur de Tours.
    private float remainingMovement = 99, remainingAction = 5; // Compte de déplacement restant (99 pour la phase de placement) , Compte de PA restant.

	void Update ()
    {
	    if(currentPath != null)
        {
            int currNode = 0;

            while (currNode < currentPath.Count-1)
            {
                Vector3 start = DungeonController.Instance.TileCoordToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);
                Vector3 end = DungeonController.Instance.TileCoordToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode].y) + new Vector3(0, 0, -1f);

                Debug.DrawLine(start, end, Color.red);
                //Debug.Log("Line has been Drawn");
                currNode++;
            }
        }

        if (attacking)
        {
            Debug.Log("Launching Attack, Please Wait");
            Attack();
        }
        
        if (SceneManager.GetActiveScene().name != "Editor" && CombatController.Instance.CombatStarted)
        {
            AdvancePathing();
            transform.position = Vector3.Lerp(transform.position, GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime);
        }
        else if (SceneManager.GetActiveScene().name != "Editor" && !CombatController.Instance.CombatStarted)
        {
            SetDefaultSpawn(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position);
            AdvancePathing();
        }
    }

    public void AdvancePathing()
    {
        if (!CombatController.Instance.CombatStarted)
        {
            //transform.position = GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position;
            transform.position = DungeonController.Instance.WorldPosTemp;
        }

        if (currentPath == null)
        {
            return;
        }

       if (remainingMovement <= 0)
        {
            //Debug.Log("Not enough movement point left, wait for the next turn");
            return;
        }

        remainingMovement -= DungeonController.Instance.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y);

        tileX = currentPath[1].x;
        tileY = currentPath[1].y;
        
        StartCoroutine(WaitBeforeNextMovement());

        currentPath.RemoveAt(0);

        if (currentPath.Count == 1)
        {
            currentPath = null;
        }
    }

    public void Attack()
    {
        attacking = false;

        if (remainingAction <= 0)
        {
            Debug.Log("No action points left, no action done");
            return;
        }

        if (remainingAction >= attackCost && CheckRange())
        {
            remainingAction -= attackCost;
            Debug.Log("Action points left : " + remainingAction);
            StartCoroutine(WaitForAttackCompletion());
        }
    }

    public bool CheckRange()
    {
        Debug.Log("Unit position: "+ tileX + "," + tileY);
        Debug.Log("Tile for Attack position: " + tileAttackX + "," + tileAttackY); // les valeurs sont égales à zéro, pourquoi ? Ok, si int en public mais pas en private (getter et setter fautif)

        // vérifier si la tile pour l'attaque est comprise entre unitPosX + rangeMin et unitPosX + rangeMax mais aussi si celle-ci est comprise entre unitPosY + rangeMin et unitPosY + rangeMax
        if (tileAttackX >= (tileX + rangeMin) && tileAttackX <= (tileX + rangeMax) && tileAttackY >= (tileY + rangeMin) && tileAttackY <= (tileY + rangeMax) || tileAttackX <= (tileX - rangeMin) && tileAttackX >= (tileX - rangeMax) && tileAttackY >= (tileY + rangeMin) && tileAttackY <= (tileY + rangeMax))
        {
            // Attaque coin supérieur droit ou coin supérieur gauche.
            Debug.Log("Range is Ok, we can attack");
            return true;
        }
        else if (tileAttackX <= (tileX - rangeMin) && tileAttackX >= (tileX - rangeMax) && tileAttackY <= (tileY - rangeMin) && tileAttackY >= (tileY - rangeMax) || tileAttackX >= (tileX + rangeMin) && tileAttackX <= (tileX + rangeMax) && tileAttackY <= (tileY - rangeMin) && tileAttackY >= (tileY - rangeMax))
        {
            // Attaque coin inférieur gauche ou coin inférieur droit.
            Debug.Log("Range is Ok, we can attack");
            return true;
        }
        else if (tileAttackX >= (tileX + rangeMin) && tileAttackX <= (tileX + rangeMax) || tileAttackX <= (tileX - rangeMin) && tileAttackX >= (tileX - rangeMax))
        {
            // Attaque Horizontale.
            Debug.Log("Range is Ok, we can attack");
            return true;
        }
        else if(tileAttackY >= (tileY + rangeMin) && tileAttackY <= (tileY + rangeMax) || tileAttackY <= (tileY - rangeMin) && tileAttackY >= (tileY - rangeMax))
        {
            // Attaque Verticale.
            Debug.Log("Range is Ok, we can attack");
            return true;
        }
        else
        {
            Debug.Log("Not in Range, abort Attack");
            return false;
        }
        
    }

    public void NextTurn()
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
        yield return new WaitForSecondsRealtime(1f);
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
