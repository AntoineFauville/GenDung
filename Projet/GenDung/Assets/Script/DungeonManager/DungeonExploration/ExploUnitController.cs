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
            SetDefaultSpawn(GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position);
            AdvancePathing();
        }
        else if (SceneManager.GetActiveScene().name != "ExploEditor") // On vérifie que la scene n'est pas l'editeur et que le placement pré-combat a été réalisé.
        {
            AdvancePathing();
            transform.position = Vector3.Lerp(transform.position, GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime);
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

    /* IEnumerator Methods*/

    public IEnumerator WaitBeforeNextMovement()
    {
        yield return new WaitForSecondsRealtime(1f);
        transform.position = Vector3.Lerp(transform.position, GameObject.Find("GridCanvas(Clone)").transform.Find("PanelGrid/Tile_" + tileX + "_" + tileY).transform.position, 5f * Time.deltaTime); // 5f for lerp is also a good speed for movement
    }

    public IEnumerator WaitBeforeNextTile()
    {
        yield return new WaitForSecondsRealtime(0.3f); // 0.3f is perfect for waiting between movement.
        remainingMovement = 1;
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
