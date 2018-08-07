using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUnit : ICharacter
{

    public override void ShowUI()
    {
        base.ShowUI();

        if (CombatManager.InCombat)
        {
           CombatUIController.instance.UpdateRightPanel(this);
            return;
        }


    }

    public override bool isPlayer()
    {
        return true;
    }

    public override void StartTurn()
    {

        base.StartTurn();
        CurrentAbility = null;
        CombatUIController.instance.UpdateLeftPanel(this);
    }

    public override void CheckAbilityRange()
    {
       // if (CurrentAbility == null) return;
        
        
        CombatManager.instance.GetAbilityRange(CurrentAbility);

    }

    public override void EndTurn()
    {
        base.EndTurn();
        CombatUIController.instance.ClearActiveAbility();
    }
}
