using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _interactabilityCanvasGroup;
    [SerializeField] private Text _coolDownText;
    [SerializeField] private Image _blackFillingImage;

    public void UpdateCoolDownView(bool interactability, int amountOfTotalTurn, int amountOfturnLeft)
    {
        string cooldown;
        float filling;

        cooldown = amountOfturnLeft.ToString();
        filling = amountOfturnLeft / amountOfTotalTurn;

        _interactabilityCanvasGroup.blocksRaycasts = interactability;
        _coolDownText.text = cooldown;
        _blackFillingImage.fillAmount = filling;
    }

    public void ResetCoolDownView(bool interactability)
    {
        _interactabilityCanvasGroup.blocksRaycasts = interactability;
        _coolDownText.text = "";
        _blackFillingImage.fillAmount = 0;
    }

}
