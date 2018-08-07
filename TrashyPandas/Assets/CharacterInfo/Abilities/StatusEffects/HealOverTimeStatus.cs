using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealOverTimeStatus : StatusEffect {

    private AbilityEffects effect;

    public HealOverTimeStatus(ICharacter unit, int duration, StatusType type, int hot) : base(unit, duration, type)
    {

        power = hot;
        effect = new HealOverTimeEffect();
        effect.hitInfo = new EffectHitInfo(hot, new HitRoll(true, false), HitType.Heal);
        effect.hitType = HitType.Heal;
        effect.abilityType = AbilityType.Nature;
    }

    public override void InitialEffect()
    {
        Debug.Log("HoT applied!");
    }

    public override void ApplyEffect()
    {
        Debug.Log("Bleed damage!" + effect.hitInfo.hitValue);
        base.ApplyEffect();
        unit.OnEffectOverTime(effect);

    }
}
