using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType { Bleed, Poison, HoT, Buff}

public abstract class StatusEffect{


    protected ICharacter target;
    public int duration;
    public Dice dice;
    public int modifier;
    public StatusType type;
    public EffectHitInfo hitInfo;
    //public bool canStack;

    public StatusEffect(ICharacter target, int duration, int modifier)
    {

        this.target = target;
        this.duration = duration;
        this.modifier = modifier;
    }

    public StatusEffect(ICharacter target, EffectHitInfo hitInfo, int duration, StatusType type, int modifier)
    {
        this.target = target;
        this.duration = duration;
        this.modifier = modifier;
        this.type = type;
        this.hitInfo = hitInfo;
    }


    public StatusEffect(ICharacter unit, EffectHitInfo hitInfo, int duration, StatusType type, int modifier, Dice dice)
    {
        this.target = unit;
        this.duration = duration;
        this.dice = dice;
        this.modifier = modifier;
        this.type = type;
        this.hitInfo = hitInfo;
    }

    /// <summary>
    /// When the effect is applied to the unit. (Usefull for one time effects, such as stat buffs)
    /// </summary>
    public virtual void InitialEffect()
    {
        
    }

    /// <summary>
    /// Is called whenever the unit starts their turn.
    /// </summary>
    public virtual void ApplyEffect()
    {
        duration--;
    }

    /// <summary>
    /// Checks whether the effect is finished. Once the duration runs out, EndEffect is called.
    /// </summary>
    /// <returns>Returns whether there's still duration left on the ability</returns>
    public bool IsFinished()
    {
        if (duration > 0) return false;

        EndEffect();
        return true;
    }

    /// <summary>
    /// Is called when the effect ends.
    /// </summary>
    public virtual void EndEffect()
    {
        Debug.Log("Effect removed");   
    }

    public override string ToString()
    {
        if (dice == null)
            return type + " Duration: " + duration;

        return type+ " Duration: " + duration + ". Dice: " + dice.ToString();
    }
}
