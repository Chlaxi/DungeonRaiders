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

    private int unitIndex;
    private PlayerUnit unit;
    private CharacterStats stats;
    private CanvasGroup canvasGroup;
    private AttributePanelController attributeController;
    public Text unitName;
    public Text unitClass;
    public Text unitLevel;



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

        attributeController.Setup(stats);
    }

    /// <summary>
    /// Used to update the characterpanel. Should only be called, if the unit hasn't changed!
    /// </summary>
    public void UpdateUI()
    {
        Debug.Log("Updating info for " + unit.name);
        if (unit == null) return;

        unitName.text = unit.name;
        unitClass.text = stats.unitClass.name;
        unitLevel.text = stats.level.ToString();
        attributeController.GetInfo();

    }

    public void Open(PlayerUnit unit)
    {
        this.unit = unit;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        Setup();
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
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

}
