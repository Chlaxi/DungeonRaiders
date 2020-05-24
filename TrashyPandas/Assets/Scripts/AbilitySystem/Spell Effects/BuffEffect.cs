using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Effects/Buff")]
public class BuffEffect : AbilityEffects
{

    public override void ApplyEffect(Ability ability, ICharacter target)
    {
        int modifier = AbilityUtilities.GetDamageBonus(this, ability.caster);

        //We want the duration to be based on the caster level
        duration = modifier+1;
        modifier = Mathf.FloorToInt(modifier / 2);
        if (modifier < 1)
            modifier = 1;

        statusEffect.SetupStatus(target, duration, modifier);
        target.ApplyEffect(this);
    }

}
