using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BleedStatus : StatusEffect
{
    private AbilityEffects effect;

    public BleedStatus(ICharacter unit, int duration, StatusType type, int dot): base(unit, duration, type)
    {
        
        power = dot;
        effect = new BleedEffect();
        effect.hitInfo = new EffectHitInfo(dot, new HitRoll(true, false), HitType.Hit);
        effect.abilityType = AbilityType.Bleed;
    }

    public override void InitialEffect()
    {
        Debug.Log("Bleed applied!");
        //if(unit.stats.BleedResistance) EndEffect();
    }

    public override void ApplyEffect()
    {
        Debug.Log("Bleed damage!" + effect.hitInfo.hitValue);
        base.ApplyEffect();  
        unit.OnEffectOverTime(effect);

    }
}
