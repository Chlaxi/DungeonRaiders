using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO split into a new script, controlling each side of the UI
public class CombatUIController : MonoBehaviour {

    #region Singleton
    public static CombatUIController instance;

        private void Awake()
    {
        instance = this;

        SetupAbilitySlots();
    }
    #endregion

    

    AbilitySlotController[] abilitySlots;

    private int currentAblityIndex = -1;

    [SerializeField]
    private GameObject abilitySlotContainer;

    [SerializeField]
    private ToolTip toolTip;

    [SerializeField]
    private CanvasGroup leftPanelCover;

    [SerializeField]
    private Text playerUnitName;
    [SerializeField]
    private Text playerCurHP;
    [SerializeField]
    private Text playerMaxHP;
    [SerializeField]
    private Text playerAC;
    [SerializeField]
    private Text playerBaB;

    [SerializeField]
    private Text targetUnitName;
    [SerializeField]
    private Text targetCurHP;
    [SerializeField]
    private Text targetMaxHP;
    [SerializeField]
    private Text targetAC;
    [SerializeField]
    private Text targetBaB;

    public CanvasGroup prepareForBattleScreen;


    private void SetupAbilitySlots()
    {
        abilitySlots = abilitySlotContainer.GetComponentsInChildren<AbilitySlotController>();
        for (int i = 0; i < abilitySlots.Length; i++)
        {
            abilitySlots[i].abilityIndex = i;

        }
    }

   
    public void UpdateLeftPanel(PlayerUnit unit)
    {
        if (abilitySlots.Length <= 0)
        {
            Debug.LogError("Setting up ability slots again?");
            SetupAbilitySlots();
        }

        currentAblityIndex = -1;

        for (int i = 0; i < abilitySlots.Length; i++)
        {
            if (unit.abilities.Count <= i || unit.abilities[i] == null)
            {
                abilitySlots[i].Disable();
                continue;
            }

            abilitySlots[i].SetupButton(unit, unit.abilities[i]);
        }

        playerUnitName.text = unit.name;
        if(unit.stats == null)
        {
            throw new System.Exception("Stats not set up");
        }
        playerCurHP.text = unit.stats.CurrentHealth.ToString();
        playerMaxHP.text = unit.stats.MaxHealth.ToString();
        playerAC.text = unit.GetAC().ToString();
        playerBaB.text = unit.stats.baseAttackBonus.ToString();
    }

    //TODO when not in combat, set the right panel to be a map or inventory
    public void UpdateRightPanel(ICharacter unit)
    {
        if (unit == null) return;
        targetUnitName.text = unit.name;
        targetCurHP.text = unit.stats.CurrentHealth.ToString();
        targetMaxHP.text = unit.stats.MaxHealth.ToString();
        targetAC.text = unit.GetAC().ToString();
        targetBaB.text = unit.stats.baseAttackBonus.ToString();
    }

    public void CoverLeftPanel(bool coverPanel)
    {
        if (coverPanel)
        {
            leftPanelCover.alpha = 1;
            leftPanelCover.blocksRaycasts = true;
        }
        else
        {
            leftPanelCover.alpha = 0;
            leftPanelCover.blocksRaycasts = false;
        }
    }

    public void SetActiveAbilityUI(int abilityIndex)
    {
        if (abilityIndex < 0) return;

        if (abilityIndex == currentAblityIndex) { ClearActiveAbility(); return; }

        ClearActiveAbility();

        currentAblityIndex = abilityIndex;
        abilitySlots[currentAblityIndex].SetAsActive(true);
    }

    public void ClearActiveAbility()
    {
        if (currentAblityIndex != -1)
        {
            abilitySlots[currentAblityIndex].SetAsActive(false);
            currentAblityIndex = -1;
        }
    }

    public void ShowTooltip(AbilitySlotController slot)
    {
        if(toolTip.SetupTooltip(slot))
            if(toolTip.isHidden)
                toolTip.ShowTooltip();
    }

    public void HideTooltip()
    {
        if (toolTip.isHidden) return;
        toolTip.HideTooltip();
    }

    #region Encapsulations

    public int CurrentAblityIndex
    {
        get
        {
            return currentAblityIndex;
        }
    }

    #endregion

}
