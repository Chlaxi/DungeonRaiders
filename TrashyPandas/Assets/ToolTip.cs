using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

    public bool isHidden = true;

    [SerializeField]private Animator animator;

    public Text abiityNameText;
    public Text abilityModifiers;
    public Text description;

    public Toggle[] requiredPositions;
    public Toggle[] targetPositions;

    public Color enemyColour;
    public Color allyColour;

    private Color curColour;
    private bool[] range;

    public bool SetupTooltip(AbilitySlotController slot)
    {
        Ability ability = slot.ability;
        if (ability == null) return false;

        abiityNameText.text = ability.name;
        description.text = ability.description;

        range = ability.behaviour.requiredPosition;
        for (int i = 0; i < requiredPositions.Length; i++)
        {
            if (range[i])
            {
                requiredPositions[i].isOn = true;
            }
            else
            {
                requiredPositions[i].isOn = false;
            }
        }



        if (ability.behaviour.canCastOnAllies)
        {
            curColour = allyColour;
            range = ability.behaviour.alliedRange;
        }
        else
        {
            curColour = enemyColour;
            range = ability.behaviour.attackRange;
        }

        for (int i = 0; i < targetPositions.Length; i++)
        {
            targetPositions[i].graphic.color = curColour;
            if (range[i])
            {
                targetPositions[i].isOn = true;
            }
            else
            {
                targetPositions[i].isOn = false;
            }
        }

        //Change this once the ability/behaviour relation is reworked.
        string modifierText ="";

        for (int i = 0; i < ability.behaviour.hitmodifiers.Length; i++)
        {
            if(ability.behaviour.hitmodifiers[i])
            {
                if (!modifierText.Equals("")) modifierText += ", ";
                modifierText += slot.unit.stats.GetStatName(i);
            }
        }
        abilityModifiers.text = modifierText;
        return true;
    }

    public void ShowTooltip()
    {
        if (!isHidden) return;

        animator.SetTrigger("Show");
        isHidden = false;
    }

    public void HideTooltip()
    {
        if (isHidden) return;

        animator.SetTrigger("Hide");
        isHidden = true;
    }
}
