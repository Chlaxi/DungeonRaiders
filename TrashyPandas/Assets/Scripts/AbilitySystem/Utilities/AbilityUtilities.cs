using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to define a dice, such as 1d6
/// </summary>
[System.Serializable]
public class Dice
{
    public int dice;
    public int power;
    public int modifier;
    
    /// <summary>
    /// Defines a new dice.
    /// </summary>
    /// <param name="dice">The amount of dice</param>
    /// <param name="power">The power of the dice</param>
    public Dice(int dice, int power, int modifier=0)
    {
        this.dice = dice;
        this.power = power;
        this.modifier = modifier;
    }

    public override string ToString()
    {
        return dice + "d" + power;
    }

    /// <summary>
    /// Rolls the dice and returns the result. Note, the modifier is not applied
    /// </summary>
    /// <returns>Returns the result of the dice roll</returns>
    public int RollDice()
    {
        int total = 0;
        for (int i = 0; i < dice; i++)
        {
            total += Random.Range(1, power + 1);
        }
        return total;
    }
}

public enum AbilitySaveInfo { none, fortitude, reflex, will}

[System.Serializable]
public class AbilitySaves{

    public AbilitySaveInfo saveType = AbilitySaveInfo.none;
    public bool halves = false;

    public override string ToString()
    {
        string saveInfo = saveType.ToString();
        if (halves)
            saveInfo += " halves";
        else
            saveInfo += " negates";

        return saveInfo;
    }

}


public enum AbilityModifierTypes { CL, STR, DEX, CON, INT, WIS, CHA }

[System.Serializable]
public class AbilityModifier
{
    public AbilityModifierTypes modifierType;

    public AbilityModifier(AbilityModifierTypes modType)
    {
        modifierType = modType;
    }
    public bool isApplied = false;
    [SerializeField] [Tooltip("The maximum applicable bonus for this modifier. If set to 0, there's no maximum")] private int maxBonus;
    [SerializeField] [Tooltip("The multplier, the character's modifer will be multiplied with")] public float multiplier = 1;

    /// <summary>
    /// Calculates the effect bonus. Useful when an ability wants to add a number based on stats or caster level.
    /// </summary>
    /// <param name="basePower">The power, i.e. caster level, the calculation is based on</param>
    /// <returns>The floored value of the modifier</returns>
    public int GetModifier(int basePower)
    {
        if (!isApplied)
            return 0;

        if (maxBonus > 0 && basePower > maxBonus)
        {
            basePower = maxBonus;
        }

        return Mathf.FloorToInt(basePower * multiplier);
    }

}

public static class AbilityUtilities {

    //TODO: Add dies to the ablity
    /// <summary>
    /// Calculates the damage roll for the effect
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="effect"></param>
    /// <returns></returns>
    public static int EffectRoll(Ability ability, AbilityEffects effect)
    {
        int value = 0;
        Dice dice = effect.dice;

        value = dice.RollDice();

        int[] casterStats = ability.caster.stats.GetModifiersFromArray();
        int damagebonus=0;
        //Rewrite this to loop in GetDamageBonus?
       // for (int i = 0; i < ability.abilityModifiers.Length; i++)
       // {
            damagebonus += GetDamageBonus(effect, ability.caster);
      //  }
//        Debug.Log("The ability power is " + value);
        if (value < 0) value = 0;

        Debug.Log("You rolled " + dice.ToString() + " resulting in: " + value + " + " + damagebonus);
        return value+ damagebonus;
    }

    /// <summary>
    /// Returns the character's attack bonus based on the applicable modifiers on the ability.
    /// </summary>
    /// <param name="ability"></param>
    /// <returns>A floored value of the attack bonus</returns>
    public static int GetAttackBonus(Ability ability)
    {

        int BaB = ability.caster.stats.baseAttackBonus;

        int mod = 0;

        ICharacter caster = ability.caster;

        for (int i = 0; i < ability.attackRollModifiers.Length; i++)
        {
            if (ability.attackRollModifiers[i].isApplied)
            {
                mod += ability.attackRollModifiers[i].GetModifier(caster.stats.GetModifierFromName(ability.attackRollModifiers[i].modifierType));
            }
        }
        return Mathf.FloorToInt(BaB + mod);
    }

    /// <summary>
    /// Calculates the damage bonus from an effect
    /// </summary>
    /// <param name="effect">The current effect</param>
    /// <param name="caster">The caster of the effect</param>
    /// <returns>A floored value of the damage bonuses</returns>
    public static int GetDamageBonus(AbilityEffects effect, ICharacter caster)
    {
        int mod = 0;

        for (int i = 0; i < effect.damageRollModifiers.Length; i++)
        {
            if (effect.damageRollModifiers[i].isApplied)
            {
                mod += effect.damageRollModifiers[i].GetModifier(caster.stats.GetModifierFromName(effect.damageRollModifiers[i].modifierType));
            }
        }
        return Mathf.FloorToInt(mod);
    }
}
