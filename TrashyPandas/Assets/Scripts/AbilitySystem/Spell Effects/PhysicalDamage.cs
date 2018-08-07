using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Abilities/Effects/PhysicalDamage")]
public class PhysicalDamage : AbilityEffects
{

    public override void ApplyEffect(Ability ability, ICharacter target)
    {
        base.ApplyEffect(ability, target);
    }
}
