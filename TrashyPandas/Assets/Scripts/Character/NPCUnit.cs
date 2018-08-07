using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NPCUnit : ICharacter {


    public override void PrepareForBattle()
    {
        base.PrepareForBattle();
    }

    public override void StartTurn()
    {
        base.StartTurn();
      
        StartCoroutine(waitForAnim());
       
    }

    IEnumerator waitForAnim()
    {

        yield return new WaitForSeconds(1);
        GetComponent<CombatAI>().DecideAction();
    }

    public override void CheckAbilityRange()
    {
        if (CurrentAbility == null) return;

        CombatManager.instance.GetAbilityRange(CurrentAbility);

    }

    public override void UseAbility(Ability ability, ICharacter target)
    {
        if (ability.CastAbility(this, target))
        {

            EndTurn();
        }

        else
        {
            Debug.Log("Cast failed!");
            GetComponent<CombatAI>().DecideAction();
        }

    }

}
