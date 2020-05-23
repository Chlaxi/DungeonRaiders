using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanelController : MonoBehaviour {

    #region Singleton
    public static CharacterPanelController instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public bool isOpen = false;

    private int unitIndex = -1; //-1 = none
    private PlayerUnit unit;
    private CharacterStats stats;
    private CanvasGroup canvasGroup;
    private AttributePanelController attributeController;
    public Text unitName;
    public Text unitClass;
    public Text unitLevel;
    public Text unitBaB;
    public Text UnitAC;
    private HeroPanel hero;


    private void Start()
    {
       
        attributeController = FindObjectOfType<AttributePanelController>();
        canvasGroup = GetComponent<CanvasGroup>();
        PlayerController.instance.unitInfoChanged += UpdateUI;
        Close();
    }

    /// <summary>
    /// Sets up the characterpanel
    /// </summary>
    public void Setup()
    {
        unitName.text = unit.name;
        stats = unit.GetComponent<CharacterStats>();
        unitClass.text = stats.unitClass.name;
        unitLevel.text = stats.level.ToString();
        unitBaB.text = stats.baseAttackBonus.ToString();
        UnitAC.text = (10+stats.GetArmour()).ToString();
        Debug.Log("BAB: " + stats.baseAttackBonus.ToString());
        Debug.Log("AC: " + stats.GetArmour().ToString());
        attributeController.Setup(stats);
    }

    /// <summary>
    /// Used to update the characterpanel. Should only be called, if the unit hasn't changed!
    /// </summary>
    public void UpdateUI()
    {
        if (unit == null) return;

        unitName.text = unit.name;
        unitClass.text = stats.unitClass.name;
        unitLevel.text = stats.level.ToString();
        unitBaB.text = stats.baseAttackBonus.ToString();
        UnitAC.text = (10+stats.GetArmour()).ToString();
        Debug.Log("BAB: " + stats.baseAttackBonus.ToString());
        Debug.Log("AC: " + stats.GetArmour().ToString());
        attributeController.GetInfo();

    }

    public void Open(HeroPanel hero)
    {
        if (isOpen && this.hero != null)
        {
            this.hero.Deselect();
        }

        this.hero = hero;
        unit = hero.unit;
        canvasGroup.alpha = 1;
        isOpen = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Setup();
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        isOpen = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        unitIndex = -1;
        if(hero != null)
        {
            hero.Deselect();
        }
    }

    public void DebugLevelUp()
    {
        stats.OnLevelUp();
        PlayerController.instance.unitInfoChanged();
    }

    public PlayerUnit GetUnit()
    {
        return unit;
    }

    public bool IsOpen()
    {
        if (canvasGroup.alpha == 0) return false;

        return true;
    }

    public int GetUnitIndex()
    {
        return unitIndex;
    }

    public void OnDestroy()
    {
        PlayerController.instance.unitInfoChanged -= UpdateUI;
    }

    public void SetUnitIndex(int index)
    {
        unitIndex = index;
    }
}
