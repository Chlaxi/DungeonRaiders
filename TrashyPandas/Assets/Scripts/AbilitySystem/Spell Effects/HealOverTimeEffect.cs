using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Effects/Heal Over Time")]
public class HealOverTimeEffect : AbilityEffects {

    public int duration;

    public override void ApplyEffect(Ability ability, ICharacter target)
    {
        int effectValue = 0;

        effectValue = AbilityUtilities.EffectRoll(ability, this);

        statusEffect = new HealOverTimeStatus(target, duration, StatusType.HoT, effectValue);
        Debug.Log("Status effect to apply: " + statusEffect);
        target.ApplyEffect(this);
    }
}
