using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStatus : StatusEffect
{
    public BuffStatus(ICharacter target, int duration, int modifier) : base(target, duration, modifier)
    {
        type = StatusType.Buff;
    }

    public override void InitialEffect()
    {
        Debug.Log("Buff "+this.ToString()+"applied!");
        target.stats.damageBonusBuff += modifier;
        target.stats.attackBonusBuff += modifier;
        Debug.Log("Added " + modifier + ". The attack bonus buff is now " + target.stats.attackBonusBuff + " and damage bonus is " + target.stats.GetTempDamageBonus());
    }

    public override void ApplyEffect()
    {
        base.ApplyEffect();
        Debug.Log("Attackbonus: " + target.stats.GetAttackBonus() + " and damage bonus is " + target.stats.GetTempDamageBonus());
    }

    public override void EndEffect()
    {
        Debug.Log("Buff expired");
        target.stats.damageBonusBuff -= modifier;
        target.stats.attackBonusBuff -= modifier;

        Debug.Log("Subtracted " + modifier + ". The attack bonus buff is now " + target.stats.attackBonusBuff + " and damage bonus is " + target.stats.GetTempDamageBonus());
    }
}
