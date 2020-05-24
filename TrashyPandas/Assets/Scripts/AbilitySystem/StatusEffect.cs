using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType { Bleed, Poison, HoT}

public abstract class StatusEffect{

    protected ICharacter unit;
    public int duration;
    public Dice dice;
    public int modifier;
    public StatusType type;
    public EffectHitInfo hitInfo;
    //public bool canStack;

    public StatusEffect(ICharacter unit, EffectHitInfo hitInfo, int duration, StatusType type, int modifier)
    {
        this.unit = unit;
        this.duration = duration;
        this.modifier = modifier;
        this.type = type;
        this.hitInfo = hitInfo;
    }


    public StatusEffect(ICharacter unit, EffectHitInfo hitInfo, int duration, StatusType type, int modifier, Dice dice)
    {
        this.unit = unit;
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
        return type+ " Duration: " + duration + ". Dice: " + dice.ToString();
    }
}
