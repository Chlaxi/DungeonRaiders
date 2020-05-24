using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Effects/Heal Over Time")]
public class HealOverTimeEffect : AbilityEffects {

    public override void ApplyEffect(Ability ability, ICharacter target)
    {
        int effectValue = 0;

        effectValue = AbilityUtilities.EffectRoll(ability, this);

        statusEffect = new HealOverTimeStatus(target, hitInfo, duration, StatusType.HoT, AbilityUtilities.GetDamageBonus(this, ability.caster), dice);
        Debug.Log("Status effect to apply: " + statusEffect+" ("+statusEffect.type+")");
        target.ApplyEffect(this);
    }
}
