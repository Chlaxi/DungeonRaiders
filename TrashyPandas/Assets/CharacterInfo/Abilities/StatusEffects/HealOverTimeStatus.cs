using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Status Effect/Heal")]
public class HealOverTimeStatus : StatusEffect {

    private AbilityEffects effect;

    public HealOverTimeStatus(ICharacter target, int duration, int modifier) : base(target, duration, modifier)
    {
        
    }

  /*  public HealOverTimeStatus(ICharacter unit, EffectHitInfo hitInfo, int duration, StatusType type, int modifier, Dice dice) : base(unit, hitInfo, duration, type, modifier)
    {

        this.dice = dice;
        effect = new HealOverTimeEffect();
        this.hitInfo = new EffectHitInfo(0, new HitRoll(true, false), HitType.Heal);
        effect.hitType = HitType.Heal;
        effect.abilityType = AbilityType.Nature;
    }
    */
    public override void InitialEffect()
    {
        Debug.Log("HoT applied!");
    }

    public override void ApplyEffect()
    {
        
        base.ApplyEffect();
        hitInfo = new EffectHitInfo(0, new HitRoll(true, false), HitType.Heal);
        hitInfo.hitValue = dice.RollDice() + modifier;
        
        target.OnEffectOverTime(this);
        Debug.Log("Healed for" + hitInfo.hitValue+" Duration left: "+duration);
    }
}
