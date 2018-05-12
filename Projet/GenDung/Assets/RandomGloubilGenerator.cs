using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGloubilGenerator : MonoBehaviour {

	List<string> Gloubils = new List<string>();

	int gloubil;

	void Start () {

		Gloubils.Add ("Raph");
		Gloubils.Add ("Toinoug");
		Gloubils.Add ("RaphP");
		Gloubils.Add ("Robin");
		Gloubils.Add ("Coco");
		Gloubils.Add ("Vic");
		Gloubils.Add ("Arthur");
		Gloubils.Add ("Raph");
		Gloubils.Add ("Aline");
		Gloubils.Add ("Theo");
		Gloubils.Add ("Vincent");
		Gloubils.Add ("Alex");
		Gloubils.Add ("Jordan");
		Gloubils.Add ("Gui");
		Gloubils.Add ("Flo");
		Gloubils.Add ("Ben");
		Gloubils.Add ("Brodco");
		Gloubils.Add ("Brice");


		gloubil = Random.Range (0, 16);

		//print (Gloubils[gloubil].ToString());

	}

}
