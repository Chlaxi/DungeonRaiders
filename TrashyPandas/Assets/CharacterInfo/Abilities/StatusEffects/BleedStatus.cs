using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BleedStatus : StatusEffect
{
    private AbilityEffects effect;

    public BleedStatus(ICharacter unit, EffectHitInfo hitInfo, int duration, StatusType type, int modifier, Dice dice): base(unit, hitInfo, duration, type, modifier)
    {
        
        this.dice = dice;
        effect = new BleedEffect();
        this.hitInfo = new EffectHitInfo(0, new HitRoll(true, false), HitType.Hit);
        effect.abilityType = AbilityType.Bleed;
    }

    public override void InitialEffect()
    {
        Debug.Log("Bleed applied!");
        //if(unit.stats.BleedResistance) EndEffect();
    }

    public override void ApplyEffect()
    {

        base.ApplyEffect();
        //Roll damage
        hitInfo.hitValue = dice.RollDice() + modifier;
        //Deal damage
        target.OnEffectOverTime(this);
        Debug.Log("Bleed damage!" + hitInfo.hitValue + " Duration left: " + duration);
    }
}
