using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_TresorRoom : MonoBehaviour {

	private static Explo_TresorRoom instance;
	private GameObject tresorCanvas;

	private Animator animTresorImageAnimator;

	LogGestionTool logT;

	void CreateInstance()
	{
		if (instance != null)
		{
			Debug.Log("There should never have two world controllers.");
		}
		instance = this;
	}

	// Use this for initialization
	void Start () {
		logT = GameObject.Find ("DontDestroyOnLoad").GetComponent<LogGestionTool> ();
		animTresorImageAnimator = GameObject.Find ("AnimImageTresorExplo").GetComponent<Animator> ();
		tresorCanvas = GameObject.Find("CanvasTresorExplo");
		tresorCanvas.GetComponent<Canvas>().sortingOrder = 38;
	}
	
	public void LinkToRoom()
	{
		tresorCanvas.GetComponent<Canvas>().sortingOrder += 40;
	}

	public void OpenTreasure()
	{
		RandomPicker ();
	}

	public void RandomPicker()
	{
		int rand = Random.Range (0,100);

		if (rand <= 60) {
			OpenChest ();
		} else {
			Itsatrap ();
		}
	}

	public void Itsatrap()
	{
		logT.AddLogLine("It's a trap");
	}

	public void OpenChest()
	{
		logT.AddLogLine("Chest Opened !");
		animTresorImageAnimator.Play ("Highlighted");
	}

	public void ClosingTab()
	{
		tresorCanvas.GetComponent<Canvas>().sortingOrder -= 40;
	}

	public static Explo_TresorRoom Instance
	{
		get
		{
			return instance;
		}

		set
		{
			instance = value;
		}
	}
}
