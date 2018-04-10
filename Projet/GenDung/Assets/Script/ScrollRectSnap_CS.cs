using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollRectSnap_CS : MonoBehaviour 
{
	// Public Variables
	public RectTransform panel;	// To hold the ScrollPanel
	public Button[] bttn;
	public RectTransform center;	// Center to compare the distance for each button

	// Private Variables
	public float[] distance;	// All buttons' distance to the center
	public float[] distReposition;
	private bool dragging = false;	// Will be true, while we drag the panel
	private int bttnDistance;	// Will hold the distance between the buttons
	private int minButtonNum;	// To hold the number of the button, with smallest distance to center
	private int bttnLength;
	private int initialBtn = 0;

	void Start()
	{
		bttnLength = bttn.Length;
		distance = new float[bttnLength];
		distReposition = new float[bttnLength];

		updateAnim (bttn [initialBtn]);

		// Get distance between buttons
		bttnDistance  = (int)Mathf.Abs(bttn[1].GetComponent<RectTransform>().anchoredPosition.x - bttn[0].GetComponent<RectTransform>().anchoredPosition.x);
	}

	void Update()
	{
		updateAnim (bttn [minButtonNum]);

		for (int i = 0; i < bttn.Length; i++)
		{
			distReposition[i] = center.GetComponent<RectTransform>().position.x - bttn[i].GetComponent<RectTransform>().position.x;
			distance[i] = Mathf.Abs(distReposition[i]);

			if (distReposition[i] > 2100)
			{
				float curX = bttn[i].GetComponent<RectTransform>().anchoredPosition.x;
				float curY = bttn[i].GetComponent<RectTransform>().anchoredPosition.y;

				Vector2 newAnchoredPos = new Vector2 (curX + (bttnLength * bttnDistance), curY);
				bttn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
			}

			if (distReposition[i] < -2100)
			{
				float curX = bttn[i].GetComponent<RectTransform>().anchoredPosition.x;
				float curY = bttn[i].GetComponent<RectTransform>().anchoredPosition.y;

				Vector2 newAnchoredPos = new Vector2 (curX - (bttnLength * bttnDistance), curY);
				bttn[i].GetComponent<RectTransform>().anchoredPosition = newAnchoredPos;
			}
		}
	
		float minDistance = Mathf.Min(distance);	// Get the min distance

		for (int a = 0; a < bttn.Length; a++)
		{
			if (minDistance == distance[a])
			{
				minButtonNum = a;
				if (dragging) {
					Debug.Log (bttn [minButtonNum].name);
				}
			}
		}

		if (!dragging)
		{
		//	LerpToBttn(minButtonNum * -bttnDistance);
			LerpToBttn (-bttn[minButtonNum].GetComponent<RectTransform>().anchoredPosition.x);
		}
	}

	void LerpToBttn(float position)
	{
		float newX = Mathf.Lerp (panel.anchoredPosition.x, position, Time.deltaTime * 5f);
		Vector2 newPosition = new Vector2 (newX, panel.anchoredPosition.y);

		panel.anchoredPosition = newPosition;
	}

	public void StartDrag()
	{
		dragging = true;
	}

	public void EndDrag()
	{
		dragging = false;
	}

	public void updateAnim (Button btn) {
		for (int a = 0; a < bttn.Length; a++) {
			//bttn [a].transform.localScale = new Vector3 (1, 1, 1);
			//bttn[a].transform.GetComponent<Animator>().Play("PopingWindowSelectionCharacterBack");
			bttn[a].transform.GetComponent<Animator>().SetBool("go",false);
		}

		//btn.transform.GetComponent<Animator>().Play("PopingWindowSelectionCharacter");
		btn.transform.GetComponent<Animator>().SetBool("go",true);
		//btn.transform.localScale = new Vector3 (1.2f, 1.2f, 1.2f);
	}
}













