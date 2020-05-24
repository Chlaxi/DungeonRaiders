using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Abilities/Effects/Bleed")]
public class BleedEffect : AbilityEffects
{
    public override void ApplyEffect(Ability ability, ICharacter target)
    {
        

        int effectValue = 0;

        effectValue = AbilityUtilities.EffectRoll(ability, this);

        statusEffect.SetupStatus(target, hitInfo, duration, AbilityUtilities.GetDamageBonus(this, ability.caster), dice);
        Debug.Log("Status effect to apply: " + statusEffect);
        target.ApplyEffect(this);
    }
}
