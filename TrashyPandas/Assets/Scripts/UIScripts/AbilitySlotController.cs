using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbilitySlotController : MonoBehaviour {

    Image image;

    [HideInInspector] public PlayerUnit unit;

    public Ability ability;
    public int abilityIndex;

    private bool isActiveAbility;
    public bool canInteract;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public Image abilityUICover;

    [SerializeField] private Color activeAbilityColour;
    [SerializeField] private Color defaultAbilityColour;
    [SerializeField] private Color unavailableAbilityColour;

    private bool onCoolDown;
    private bool canUse;

    public void SetupButton(PlayerUnit unit, Ability ability)
    {
        this.unit = unit;
        this.ability = ability;

        CheckRange();

    }

    private void CheckRange()
    {
       int position = unit.GetPosition().GetPositionIndex();
        if (!ability.behaviour.requiredPosition[position])
        {
            Disable();
            image.sprite = ability.icon;
        }
        else
        {
            Enable();
        }

    }

    private bool AbilityIsReady()
    {
        if (onCoolDown)
        {
            abilityUICover.color = unavailableAbilityColour;
            return false;
        }


        return true;
    }

    public void OnClick()
    {
        if (!canInteract) return; 


        if (!AbilityIsReady()) return;

      /*  if (isActiveAbility)
        {
            CombatUIController.instance.ClearActiveAbility();
        }*/

        CombatUIController.instance.SetActiveAbilityUI(abilityIndex);

/*if (isActiveAbility)
{
    SetAsActive(false);
}
else
{
    SetAsActive(true);
}*/

    }


    /// <summary>
    /// Should this be the active ability?
    /// </summary>
    /// <param name="active"></param>
    public void SetAsActive(bool active)
    {

        if (active)
        {
            isActiveAbility = true;
            
            abilityUICover.color = activeAbilityColour;
            unit.CurrentAbility = ability;
        }
        else
        {
            isActiveAbility = false;
           
            abilityUICover.color = defaultAbilityColour;
            unit.CurrentAbility = null;
        }
    }

    public void Enable()
    {
        SetAsActive(false);
        canInteract = true;
        image.sprite = ability.icon;
    }

    public void Disable()
    {
        abilityUICover.color = unavailableAbilityColour;
        canInteract = false;
        image.sprite = null;
    }

    public void ShowTooltip()
    {
        CombatUIController.instance.ShowTooltip(this);
    }


    public void HideToolTip()
    {

        CombatUIController.instance.HideTooltip();
            }

}
