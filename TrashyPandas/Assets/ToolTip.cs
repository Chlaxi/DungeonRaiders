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

        range = ability.requiredPosition;
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



        if (ability.canCastOnAllies)
        {
            curColour = allyColour;
            range = ability.alliedRange;
        }
        else
        {
            curColour = enemyColour;
            range = ability.attackRange;
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

        for (int i = 0; i < ability.attackRollModifiers.Length; i++)
        {
            if(ability.attackRollModifiers[i].isApplied)
            {
                if (!modifierText.Equals("")) modifierText += ", ";
                modifierText += ability.attackRollModifiers[i].modifierType.ToString();
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
