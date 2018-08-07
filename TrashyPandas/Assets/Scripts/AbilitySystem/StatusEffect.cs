using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType { Bleed, Poison, HoT}

public abstract class StatusEffect{

    protected ICharacter unit;
    public int duration;
    public int power;
    public StatusType type;
    //public bool canStack;

    public StatusEffect(ICharacter unit, int duration, StatusType type)
    {
        this.unit = unit;
        this.duration = duration;
        this.type = type;
    }


    public StatusEffect(ICharacter unit, int duration, StatusType type, int power)
    {
        this.unit = unit;
        this.duration = duration;
        this.power = power;
        this.type = type;
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

    public bool IsFinished()
    {
        if (duration > 0) return false;

        EndEffect();
        return true;
    }

    public virtual void EndEffect()
    {
        Debug.Log("Effect removed");   
    }


}
