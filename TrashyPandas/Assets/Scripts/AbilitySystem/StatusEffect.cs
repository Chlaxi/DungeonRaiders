using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusType { Bleed, Poison, HoT, Buff}

public abstract class StatusEffect : ScriptableObject{

    public new string name = "new status effect";
    public Sprite statusIcon;
    [TextArea(1,3)]
    public string description;
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


    public void SetupStatus(ICharacter unit, EffectHitInfo hitInfo, int duration, int modifier, Dice dice)
    {
        this.target = unit;
        this.duration = duration;
        this.dice = dice;
        this.modifier = modifier;
        this.hitInfo = hitInfo;
    }

    public virtual void SetupStatus(ICharacter target, int duration, int modifier)
    {
        this.target = target;
        this.duration = duration;
        this.modifier = modifier;
    }

    /// <summary>
    /// When the effect is applied to the unit. (Usefull for one time effects, such as stat buffs)
    /// </summary>
    public virtual void InitialEffect()
    {
        //TODO: Saving throw!
        Debug.Log(name + " was initiated");
    }

    /// <summary>
    /// Is called whenever the unit starts their turn.
    /// </summary>
    public virtual void ApplyEffect()
    {
        Debug.Log(name + " Was applied!");
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
