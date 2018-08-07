using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityUtilities {

    //TODO: Add dies to the ablity
    public static int EffectRoll(Ability ability, AbilityEffects effect)
    {
        int value = 0;
        int numDices = 1;   //ability.numDies
        int size = ability.basePower;

        for (int i = 0; i < numDices; i++)
        {
            value += Random.Range(1, size);
        }

        int[] casterStats = ability.caster.stats.GetModifiersFromArray();

        for (int i = 0; i < ability.behaviour.hitmodifiers.Length; i++)
        {
            value += Mathf.RoundToInt(casterStats[i] * effect.multipliers[i]);
        }
//        Debug.Log("The ability power is " + value);
        if (value < 0) value = 0;
        return value;
    }

    public static int GetAttackBonus(Ability ability)
    {

        int BaB = ability.caster.stats.baseAttackBonus;

        int mod = 0;

        ICharacter caster = ability.caster;
        int[] casterStats = caster.stats.GetModifiersFromArray();

        for (int i = 0; i < ability.behaviour.hitmodifiers.Length; i++)
        {
            if (ability.behaviour.hitmodifiers[i]) mod += casterStats[i];
        }
        return Mathf.FloorToInt(BaB + mod);
    }


}
