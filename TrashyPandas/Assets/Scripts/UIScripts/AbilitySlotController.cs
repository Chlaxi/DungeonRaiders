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

    private void Awake()
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



        if (CombatManager.InCombat)
        {
            CheckRange();
        }
        else
        {
            Enable();
        }

    }

    private void CheckRange()
    {
        int position = unit.GetPosition().GetPositionIndex();
        //Checks if we are standing in the right spot for the ability.
        if (!ability.requiredPosition[position])
        {
            Disable();
            image.sprite = ability.icon;
            return;
        }

        //Checks whether there's enemies within range for the attack.
        if (ability.canTargetOpponents)
        {
            int enemiesAvailable = 0;
            for (int i = 0; i < ability.attackRange.Length; i++)
            {
                bool canHit = ability.attackRange[i];
                //If we can't hit anything in that spot, we don't even care to check for enemies there.
                if (!canHit)
                {
                    continue;
                }

                if (CombatManager.instance.enemyPool[i].unit != null)
                {
                    enemiesAvailable++;
                }

            }
            if (enemiesAvailable > 0)
            {
                Enable();
            }
            else
            {
                Disable();
                image.sprite = ability.icon;
            }
        }

        if(ability.canCastOnAllies || ability.canCastOnSelf)
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

    /// <summary>
    /// Used when an ability for this slot is found
    /// </summary>
    public void Enable()
    {
        SetAsActive(false);
        image.sprite = ability.icon;
        if (CombatManager.InCombat)
        {
            canInteract = true;
        }
        else
        {
            //Based on whether out of combat casting is allowed.
            canInteract = false;
        }
    }

    /// <summary>
    /// Used to display empty slots.
    /// </summary>
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
