using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AbilityType represents the type of the ability. Is it a physical attack? A ranged? etc.
/// </summary>
public enum AbilityType { Physical, Ranged, Piercing, Shadow, Holy, Fire, Nature, Poison, Bleed }

public enum HitType { Hit, Heal, DoT, HoT, StatusEffect }

public struct EffectHitInfo
{
    public int hitValue;
    public bool isCrit;
    public bool isHit;
    public HitType hitType;

    public EffectHitInfo(int hitValue, HitRoll hitRoll, HitType hitType)
    {
        this.hitValue = hitValue;
        this.isCrit = hitRoll.wasCrit;
        this.isHit = hitRoll.wasHit;
        this.hitType = hitType;
    }

}

[System.Serializable]
public abstract class AbilityEffects : ScriptableObject{

    
    //These are set on the ability
    [HideInInspector] public Dice dice;
    [HideInInspector] public AbilityModifier[] damageRollModifiers;
    [HideInInspector] public int duration;

    public AbilityType abilityType;
    public HitType hitType;
    public EffectHitInfo hitInfo;
    public bool isStatusEffect = false;
    public StatusEffect statusEffect;

    /// <summary>
    /// Used to set the damage modifiers and dice.
    /// </summary>
    /// <param name="dice"></param>
    /// <param name="damageRollModifiers"></param>
    public void Initialize(Dice dice, AbilityModifier[] damageRollModifiers, int duration=0)
    {
        this.dice = dice;
        this.damageRollModifiers = damageRollModifiers;
        this.duration = duration;
        if(duration > 0)
        {
            isStatusEffect = true;
            Debug.Log(name + " is a status effect that lasts for " + duration + " turns");
        }
        Debug.Log("Dice set: " + this.dice.ToString());
    }

    public virtual void InitialEffect()
    {
        Debug.Log("Initial effect");
        //if(unit.stats.BleedResistance) EndEffect();
    }

    /// <summary>
    /// The effect
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="target"></param>
    public virtual void ApplyEffect(Ability ability, ICharacter target)
    {
        Debug.Log(name + " effect was applied");
        int effectValue = 0;

        effectValue = AbilityUtilities.EffectRoll(ability, this);
        if (ability.rollInfo.wasCrit) effectValue = effectValue * 2;

        hitInfo = new EffectHitInfo(effectValue, ability.rollInfo, hitType);

        target.ApplyEffect(this);
        
    }


    /*
    //TODO revise crit with crit chance
    /// <summary>
    /// Checks whether the ability hits the target or not. (Can't crit)
    /// </summary>
    /// <param name="attackBonus"></param>
    /// <param name="targetAC"></param>
    /// <returns></returns>
    public virtual HitRoll CheckHit(int attackBonus, int targetAC)
    {
        rollInfo.wasCrit = false;
        rollInfo.wasHit = false;

        int rollValue = Random.Range(1, 20);

        if (rollValue == 1)
        {
            Debug.Log("Natural 1");
            return rollInfo;
        }

        if(rollValue == 20)
        {
            Debug.Log("Natural 20!");
            rollInfo.wasHit = true;
            return rollInfo;
        }

        rollValue += attackBonus;
        Debug.Log("Your attack roll was " + rollValue + ". Target's AC is " + targetAC);

        if (rollValue >= targetAC)
        {
            rollInfo.wasHit = true;
        } 
        return rollInfo;
    }*/



}
