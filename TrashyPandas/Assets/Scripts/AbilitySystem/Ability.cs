using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct EffectInfo
{
    public AbilityEffects effect;
    public bool UseOwnAttackRoll;
    public Dice dice;
    public bool canCrit;
    
    [Tooltip("A list of modifiers, that can increase the damage roll, such as strength, dexterity, or caster level")]
    public AbilityModifier[] damageRollModifiers;
    
    [Tooltip("Defines whether the effect applies a status effect or not. If set to 0, it will only be a direct hit effect")]
    public int duration;
}

/// <summary>
/// Contains information about whether the attack was a hit and or a crit.
/// </summary>
public struct HitRoll
{
    public HitRoll(bool hit, bool crit)
    {
        wasHit = hit;
        wasCrit = crit;
        roll = 0;
        modifier = 0;
    }

    public bool wasHit;
    public bool wasCrit;

    public int roll;
    public int modifier;

    public override string ToString()
    {
        return "You rolled a "+ roll + " With a modifier of "+modifier;
    }
}

//TODO Revise ability system
[CreateAssetMenu(menuName = "Abilities/New Ability")]
{

    public HitRoll rollInfo = new HitRoll();

    [Header("General info")]
    new public string name = "New ability";
    public Sprite icon;
    [TextArea(1,3)]
    public string description;
    private int cooldown = 0;
    [Header("Effect info")]

    //public AbilityModifier CasterLevel = new AbilityModifier(AbilityModifierTypes.CL);
    public AbilityModifier[] attackRollModifiers;
    public AbilitySaves Saves;
    
    public EffectInfo[] effects;

    [Header("Target Info")]
    public bool guaranteedHit;

    public bool canCastOnSelf = false;
    public bool canCastOnAllies = false;
    public bool canTargetOpponents = true;
    public bool AoE = false;

    public bool[] requiredPosition = new bool[4];
    public bool[] attackRange = new bool[4];
    public bool[] alliedRange = new bool[4];

    [Header("Graphics")]
    public GameObject ParticleEffect;
    public string animationTrigger;

    [HideInInspector] public ICharacter caster;

    private void SetHitInfo()
    {

    }

    /// <summary>
    /// Used to cast an ability
    /// </summary>
    /// <param name="caster">The caster. Used to get info, such as modifiers</param>
    /// <param name="target">The targets for the ability</param>
    /// <returns></returns>
    public bool CastAbility(ICharacter caster, ICharacter target)
    {
        if (target.isDead) // || Range>attackRange)
            return false;

        this.caster = caster;
        caster.AnimationTrigger(animationTrigger);
        //Do something with the particle effect

        UseAbility(target);
        return true;
    }

    /// <summary>
    /// Applies the ability's effect on the target.
    /// </summary>
    /// <param name="target">The current target for the ability</param>
    private void UseAbility(ICharacter target)
    {
        int critChance = 0; //Get caster crit chance or ability crit chance
        
        
        for(int i = 0; i < effects.Length; i++)
        {
            EffectInfo effect = effects[i];

            effect.effect.Initialize(effect.dice, effect.damageRollModifiers, effect.duration);
         

            //Update check, so the first effect must either be guaranteed or will roll.
            if (i > 0 && !effect.UseOwnAttackRoll)
            {
                
                if (rollInfo.wasHit)
                {
                    effect.effect.ApplyEffect(this, target);
                    Debug.Log("Effect auto-confirmed, since the ability already hit");
                }
                return;
            }

            rollInfo = CheckHit(target.GetAC(), critChance);

            if (guaranteedHit) rollInfo.wasHit = true;
            
            Debug.Log(caster.name + " used " + name + " (" + rollInfo.ToString() + ")");

            if (rollInfo.wasHit)
            {
                //          Debug.Log(name + "Was a hit");
                effect.effect.ApplyEffect(this, target);
            }
        }
    }


    /// <summary>
    /// Checks whether the ability hits the target or not and whether it's a crit
    /// </summary>
    /// <param name="attackBonus"> The caster's attackBonus </param>
    /// <param name="targetAC"> The target's Armour Class </param>
    /// <param name="critChance"> The caster's critchance </param>
    /// <returns></returns>
    public HitRoll CheckHit(int targetAC, int critChance)
    {
        rollInfo.wasCrit = false;
        rollInfo.wasHit = false;
        bool threat = false;

        int rollValue = Random.Range(1, 20);
        rollInfo.roll = rollValue;
        rollInfo.modifier = AbilityUtilities.GetAttackBonus(this);
        //        Debug.Log("Rolled " + rollValue); //Show this somewhere?
        if (rollValue == 1)
        {
            Debug.Log("Natural 1");
            //Nat 1 = auto miss.
            return rollInfo;
        }

        //Checks if it's within the critical threat range. If target is an ally, nat 20 should just confirm hit
        if (rollValue >= 20 - critChance)
        {
            Debug.Log("Threat gained!");
            threat = true;

            //Hit is only auto-confirmed if the roll is a natural 20.
            if (rollValue == 20)
            {
                Debug.Log("Natural 20!");
                rollInfo.wasHit = true;
            }
        }

        //Adds the attackbonus to the attack, to calculate whether it's a hit.
        rollValue += rollInfo.modifier;

        //If you aren't already guaranteed a hit and you exceed the target's AC, confirm hit.
        if (!rollInfo.wasHit && rollValue >= targetAC)
        {
            rollInfo.wasHit = true;
        }

        if (threat) //Locks in the crit
        {
            rollValue = Random.Range(1, 20) + rollInfo.modifier;
            if (rollValue >= targetAC)
            {
                rollInfo.wasCrit = true;
                Debug.Log("Crit locked!");
            }
            else
            {
                Debug.Log("Crit failed");
            }
        }


        return rollInfo;
    }
}
