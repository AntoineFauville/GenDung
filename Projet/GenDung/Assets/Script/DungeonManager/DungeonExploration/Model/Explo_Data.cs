using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Data
{
    int roomAmount;
    List<Foe> deadFoes = new List<Foe>();
    List<Player> players = new List<Player>();
    List<Explo_Room> rooms = new List<Explo_Room>();

    public Explo_Data()
    {

    }
}