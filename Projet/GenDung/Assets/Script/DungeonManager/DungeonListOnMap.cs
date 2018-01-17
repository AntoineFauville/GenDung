using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonListOnMap : MonoBehaviour {

	//bonjour je suis votre assistant pour l'ajout de donjon sur la carte
	//vous trouverez ici le script qui vous permet de rajouter un donjon a la carte
	//1. ajouter un boutons et lui ajouter un triggerEvent avec pointerEnter en methode
	//2. assigner dans le pointerEnter le gameobject PanelDonjonListMachin
	//3. lui donner en parametre un nouvel index

	public GameObject[] dungeonOnTheMapList;

	public void setIndex (int a)
    {
        MapController.Instance.ChangeDungeon(a);
	}
}
