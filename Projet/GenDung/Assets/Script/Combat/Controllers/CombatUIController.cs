using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIController : MonoBehaviour {

    private static CombatUIController instance;

    private Button btnStartGame, btnSpell1, btnSpell2, btnSpell3, btnNextTurn;
    private Sprite defaultIcon;
    private GameObject UIMonsterDisplayPrefab, UIMonsterDisplay, UIPlayerDisplay;

    void CreateInstance()
    {
        if (instance != null)
        {
            Debug.Log("There should never have two CombatUIControllers.");
        }
        instance = this;
    }

    void Start ()
    {
        CreateInstance();
        linkButtons();
        SetButtonsActions();

        /* Get Specific Sprite from Multiple Sprites */
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/IconeBearClaw");
        defaultIcon = sprites[4];
        /**/

        SetStartVisual();
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
        btnStartGame.onClick.AddListener(PreCombatController.Instance.StartCombatMode);
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

    public void CreatePlayerUIBattleOrder()
    {
        UIMonsterDisplayPrefab = Resources.Load("UI_Interface/UIBattleOrderDisplay") as GameObject;

        for (int i = 0; i < GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedSizeOfTheTeam; i++)
        {
            UIPlayerDisplay = Instantiate(UIMonsterDisplayPrefab);
            UIPlayerDisplay.transform.parent = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel");
            UIPlayerDisplay.transform.localScale = new Vector3(1, 1, 1);
            UIPlayerDisplay.name = "UIDisplayCharacter_" + i;
            UIPlayerDisplay.transform.Find("PVOrderDisplay").GetComponent<Image>().fillAmount = ((float)GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].Health_PV / (float)GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].Health_PV);

            UIPlayerDisplay.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].TempSprite;

            UIPlayerDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text>().text = GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].Name.ToString();
            UIPlayerDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text>().text = "PV : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].Health_PV.ToString();
            UIPlayerDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text>().text = "PA : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].ActionPoints_PA.ToString();
            UIPlayerDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPM").GetComponent<Text>().text = "PM : " + GameObject.Find("DontDestroyOnLoad").GetComponent<SavingSystem>().gameData.SavedCharacterList[i].MovementPoints_PM.ToString();
        }
    }

    public void CreateMonsterUIBattleOrder(int x)
    {
            UIMonsterDisplay = Instantiate(UIMonsterDisplayPrefab);
            UIMonsterDisplay.transform.parent = GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel");
            UIMonsterDisplay.transform.localScale = new Vector3(1, 1, 1);
            UIMonsterDisplay.name = "UIDisplayFoe_" + x;
            UIMonsterDisplay.transform.Find("PVOrderDisplay").GetComponent<Image>().fillAmount = (PreCombatController.Instance.Foe.FoeHealth / PreCombatController.Instance.Foe.FoeMaxHealth);

            UIMonsterDisplay.transform.Find("MASK/PlayerRepresentation").GetComponent<Image>().sprite = GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().dungeonList.myDungeons[MapController.Instance.DungeonIndex].dungeon.RoomOfTheDungeon[GameObject.Find("DontDestroyOnLoad").GetComponent<DungeonLoader>().actualIndex].enemiesList[x].enemyIcon;

            UIMonsterDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayName").GetComponent<Text>().text = PreCombatController.Instance.Foe.FoeName.ToString();
            UIMonsterDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPV").GetComponent<Text>().text = "PV : " + PreCombatController.Instance.Foe.FoeHealth.ToString();
            UIMonsterDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPA").GetComponent<Text>().text = "PA : " + PreCombatController.Instance.Foe.FoePA.ToString();
            UIMonsterDisplay.transform.Find("ToolTipAlpha/TooltipPanel/PanelInfo/OrderDisplayPM").GetComponent<Text>().text = "PM : " + PreCombatController.Instance.Foe.FoePM.ToString();
    }

    public void OrganizeUIBattleOrder(List<GameObject> initiativeSortedList)
    {
        for (int i = 0; i < initiativeSortedList.Count; i++)
        {
            GameObject.Find("CanvasUIDungeon(Clone)").transform.Find("OrderOfBattle/OrderBattlePanel/UIDisplay" + initiativeSortedList[i].transform.parent.name).gameObject.transform.SetAsLastSibling(); 
        }
    }

    public static CombatUIController Instance
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
