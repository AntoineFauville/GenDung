using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoeController : MonoBehaviour {

	public void SetDefaultSpawn(Vector3 pos)
    {
        this.transform.position = pos;
    }

    public void FoeClicked()
    {
        if (Input.GetMouseButtonUp(0))
            Debug.Log("I have been Clicked on ! Bitch stay away from me !!");
    }
}
