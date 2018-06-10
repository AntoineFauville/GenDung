using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignMainCameraCanvas : MonoBehaviour {

	
	void Awake ()
	{
	    this.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
}
