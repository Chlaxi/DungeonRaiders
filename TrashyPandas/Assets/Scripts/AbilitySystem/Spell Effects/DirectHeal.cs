using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Effects/DirectHealDamage")]
public class DirectHeal : AbilityEffects
{



    public override void ApplyEffect(Ability ability, ICharacter target)
    {
        base.ApplyEffect(ability, target);
    }
}
