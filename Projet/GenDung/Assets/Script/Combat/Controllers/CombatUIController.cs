using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIController : MonoBehaviour {

    private Button btnStartGame, btnSpell1, btnSpell2, btnSpell3, btnNextTurn;
    public Sprite defaultIcon;

    void Start ()
    {
        linkButtons();
        SetButtonsActions();

        /* Get Specific Sprite from Multiple Sprites */
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/IconeBearClaw");
        defaultIcon = sprites[4];
        /**/
	}

    public void linkButtons() // link the gameobject to the variable.
    {
        btnStartGame = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Button_Start_Game").GetComponent<Button>();
        btnSpell1 = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_1").GetComponent<Button>();
        btnSpell2 = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_2").GetComponent<Button>();
        btnSpell3 = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/Panel/Button_Spell_3").GetComponent<Button>();
        btnNextTurn = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("Panel/Panel/Spells/PanelPassYourTurn/ButtonPassYourTurn").GetComponent<Button>();
    }

    public void SetButtonsActions() // set action on click.
    {
        btnStartGame.onClick.AddListener(CombatController.Instance.StartCombatMode);
        btnSpell1.onClick.AddListener(CombatController.Instance.SwitchAttackModeFirst);
        btnSpell2.onClick.AddListener(CombatController.Instance.SwitchAttackModeSecond);
        btnSpell3.onClick.AddListener(CombatController.Instance.SwitchAttackModeThird);
        btnNextTurn.onClick.AddListener(CombatController.Instance.NextEntityTurn);
    }

    public void SetStartVisual()
    {
        btnStartGame.GetComponent<CanvasGroup>().alpha = 0.5f;
        btnStartGame.GetComponent<CanvasGroup>().interactable = false;
    }
    public void SwitchStartVisual()
    {
        if (btnStartGame.GetComponent<CanvasGroup>().interactable == false)
        {
            btnStartGame.GetComponent<CanvasGroup>().alpha = 1f;
            btnStartGame.GetComponent<CanvasGroup>().interactable = true;
        }
        else
        {
            btnStartGame.GetComponent<CanvasGroup>().alpha = 0f;
            btnStartGame.GetComponent<CanvasGroup>().interactable = false;
        }
    }

    public void PlayerTurnButton()
    {
        // Réactivation des Boutons de Spells
        btnSpell1.GetComponent<Image>().sprite = CombatController.Instance.TargetUnit.PlayerSpells[0].spellIcon;
        btnSpell1.GetComponent<Button>().interactable = true;
        btnSpell1.GetComponent<CanvasGroup>().alpha = 1f;
        btnSpell2.GetComponent<Image>().sprite = CombatController.Instance.TargetUnit.PlayerSpells[1].spellIcon;
        btnSpell2.GetComponent<Button>().interactable = true;
        btnSpell2.GetComponent<CanvasGroup>().alpha = 1f;
        btnSpell3.GetComponent<Image>().sprite = CombatController.Instance.TargetUnit.PlayerSpells[2].spellIcon;
        btnSpell3.GetComponent<Button>().interactable = true;
        btnSpell3.GetComponent<CanvasGroup>().alpha = 1f;
        // Réactivation du bouton 'Next Turn'
        btnNextTurn.interactable = true;
    }

    public void MonsterTurnButton()
    {
        // Désactivation des Boutons de Spells
        btnSpell1.GetComponent<Image>().sprite = defaultIcon;
        btnSpell1.GetComponent<Button>().interactable = false;
        btnSpell2.GetComponent<Image>().sprite = defaultIcon;
        btnSpell2.GetComponent<Button>().interactable = false;
        btnSpell3.GetComponent<Image>().sprite = defaultIcon;
        btnSpell3.GetComponent<Button>().interactable = false;
        // Désactivation du bouton 'Next Turn'
        btnNextTurn.interactable = false;
    }
}
