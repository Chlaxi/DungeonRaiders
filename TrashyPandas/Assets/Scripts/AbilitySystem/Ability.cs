using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitRoll
{
    public bool wasHit;
    public bool wasCrit;
    public HitRoll(bool hit, bool crit)
    {
        wasHit = hit;
        wasCrit = crit;
    }
}

//TODO Revise ability system
[CreateAssetMenu(menuName = "Abilities/New Ability")]
public class Ability : ScriptableObject {


    public HitRoll rollInfo = new HitRoll();

    new public string name = "New ability";
    public Sprite icon;
    public string animationTrigger;
    [TextArea(1,3)]
    public string description;
    private int cooldown = 0;
    public int basePower = 0;
    public AbilityEffects[] effects;
    public AbilityBehaviours behaviour;

    public GameObject ParticleEffect;

    [HideInInspector] public ICharacter caster;

    private void SetHitInfo()
    {

    }

    public bool CastAbility(ICharacter caster, ICharacter target)
    {
        if (target.isDead) // || Range>attackRange)
            return false;

        this.caster = caster;
        caster.AnimationTrigger(animationTrigger);
        //Do something with he particle effect
        UseAbility(target);
        return true;
    }

    public void UseAbility(ICharacter target)
    {
        int critChance = 0; //Get caster crit chance or ability crit chance

        rollInfo = CheckHit(AbilityUtilities.GetAttackBonus(this), target.GetAC(), critChance);

        if (behaviour.guaranteedHit) rollInfo.wasHit = true;


        if (rollInfo.wasHit)
        {
//            Debug.Log(name + "Was a hit");
            foreach (AbilityEffects effect in effects)
            {
                effect.ApplyEffect(this, target);
            }
        }
        
    }


    /// <summary>
    /// Checks whether the ability hits the target or not, and whether it's a crit
    /// </summary>
    /// <param name="attackBonus"> The caster's attackBonus </param>
    /// <param name="targetAC"> The target's Armour Class </param>
    /// <param name="critChance"> The caster's critchance </param>
    /// <returns></returns>
    public HitRoll CheckHit(int attackBonus, int targetAC, int critChance)
    {
        rollInfo.wasCrit = false;
        rollInfo.wasHit = false;
        bool threat = false;

        int rollValue = Random.Range(1, 20);
        //        Debug.Log("Rolled " + rollValue);
        if (rollValue == 1)
        {
            Debug.Log("Natural 1");
            return rollInfo;
        }

        if (rollValue >= 20 - critChance)
        {
            Debug.Log("Threat gained!");
            threat = true;
            if (rollValue == 20)
            {
                Debug.Log("Natural 20!");
                rollInfo.wasHit = true;
            }
        }

        rollValue += attackBonus;
        //  Debug.Log("Your attack roll was " + rollValue + ". Target's AC is " + targetAC);

        if (!rollInfo.wasHit && rollValue >= targetAC)
        {
            //   Debug.Log("Your roll was higher than the target's AC!");
            rollInfo.wasHit = true;
        }

        if (threat) //Locks in the crit
        {
            rollValue = Random.Range(1, 20) + attackBonus;
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
