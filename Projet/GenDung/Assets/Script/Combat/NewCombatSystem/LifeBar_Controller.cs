using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar_Controller : MonoBehaviour {

	public GameObject LifeBar;
	float lifeAmount;
	public GameObject LifeYellowBar;
	float lifeYellowAmount;
	public bool updating;
	public bool shake;

	float initialX = 0;
	float initialY = 72;

	float randomX;
	float randomY;

	float numberX;
	float numberY;

	float minX, maxX, minY, maxY;

	public bool IsThisUIBattleOrderDisplay;

	void start(){
		if(!IsThisUIBattleOrderDisplay)
			this.GetComponent<RectTransform>().anchoredPosition = new Vector2 (initialX,initialY);
	}

	void Update(){
		lifeAmount = LifeBar.GetComponent<Image> ().fillAmount;

		if (lifeAmount < lifeYellowAmount && !updating) {
			if (!IsThisUIBattleOrderDisplay) {
				GetRandomShake ();
			}
			StartCoroutine (updateLifeBar());
		}

		if (lifeYellowAmount < lifeAmount) {
			lifeYellowAmount = lifeAmount;
			updating = false;
			if (!IsThisUIBattleOrderDisplay) {
				shake = false;
				this.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (initialX, initialY);
			}
		}

		if (updating) {
			lifeYellowAmount -= 0.003f;
			updateLifeUI ();
		}

		if(shake){
			shakeEffect ();
		}
	}



	void updateLifeUI(){
		LifeYellowBar.GetComponent<Image> ().fillAmount = lifeYellowAmount;
	}

	void GetRandomShake(){
		shake = true;

		minX = -2f;
		maxX = 2f;

		minY = -1f;
		maxY = 1f;

		randomX = Random.Range(minX,maxX);
		randomY = Random.Range(minY,maxY);
	}

	void shakeEffect(){

		if (minX >= 0) {
			minX = 0;
		} else {
			minX += 0.3f;
		}

		if (maxX <= 0) {
			maxX = 0;
		} else {
			maxX -= 0.3f;
		}

		if (minY >= 0) {
			minY = 0;
		} else {
			minY += 0.3f;
		}

		if (maxY <= 0) {
			maxY = 0;
		} else {
			maxY -= 0.3f;
		}

		randomX = Random.Range(minX,maxX);
		randomY = Random.Range(minY,maxY);

		numberX = initialX + randomX;
		numberY = initialY + randomY;

		this.GetComponent<RectTransform>().anchoredPosition = new Vector2 (numberX,numberY);
	}

	IEnumerator updateLifeBar()
	{
		yield return new WaitForSeconds (1.0f);	
		updating = true;
	}
}
