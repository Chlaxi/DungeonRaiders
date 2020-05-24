using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ICharacter : MonoBehaviour
{
    public new string name;
    
    public delegate void UnitDied(ICharacter unit);
    public UnitDied unitDied;

    public delegate void UnitIsReady();
    public UnitIsReady unitIsReady;

    public delegate void OnHealthChange();
    public OnHealthChange onHealthChange;

    public CharacterStats stats;

    public int initiative = 0;

    public List<Ability> abilities = new List<Ability>();
    public HealthBarScript healthBar;
    public GameObject graphics;
    protected Animator animator;

    public List<Status> currentEffects = new List<Status>();

    private Ability currentAbility;

    bool inCombat;
    public bool hasHadTurn;
    public bool isDead;

    private void Awake()
    {

        if (animator == null) animator = GetComponent<Animator>();
    
        SetupCharacter();
    }

    public void SetupCharacter()
    {
        stats = GetComponent<CharacterStats>();
       // stats.Setup();
    }

    public void SpawnCharacter()
    {
        graphics.SetActive(true);
    }


    public virtual void ShowUI()
    {

    }

    /// <summary>
    /// Returns the character's calculated Armour Class (AC)
    /// </summary>
    /// <returns></returns>
    public int GetAC()
    {
        //10 + armourBonus + shieldBonus + dexterity + 
        return 10 + stats.GetArmour() + stats.GetModifier(stats.dexterity);


    }

    /// <summary>
    /// Rolls the initiative for the character (d20 + dexterity)
    /// </summary>
    public void SetInitiative()
    {
        int initiativeRoll = Random.Range(0, 20);
        initiative = stats.GetModifier(stats.dexterity) + initiativeRoll;
      //  Debug.Log(name + " rolled " + initiativeRoll + " for a total of " + initiative);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Returns the character's initiative</returns>
    public int GetInitiative()
    {
        return initiative;
    }

    public void CombatEffect(Ability ability)
    {

    }

    //TODO Rework this, so it makes more sense
    //This is where the AC should be calculated?
    public void OnHit(AbilityEffects effect)
    {
        bool wasHit = effect.hitInfo.isHit;
        healthBar.OnHit(effect.hitInfo);
        stats.ModifyHealth(effect.hitInfo);
    }

    //Perform Saveing throws here?
    public void OnEffectOverTime(StatusEffect status)
    {
        healthBar.OnEffectOverTime(status);
        stats.ModifyHealth(status.hitInfo);
    }

    /// <summary>
    /// Used to apply combat effects, such as damage, healing, damage over time, ad other status effects.
    /// </summary>
    /// <param name="effect"></param>
    public void ApplyEffect(AbilityEffects effect)
    {

        switch (effect.hitType)
        {
            case HitType.Hit:
                effect.hitInfo.hitValue = ReduceDamage(effect.hitInfo.hitValue, effect.abilityType);
                OnHit(effect);
                break;

            case HitType.Heal:
                OnHit(effect);
                //Add eventual heal mods
                break;

            case HitType.DoT:

                AddNewStatusEffect(effect.statusEffect);

            //    currentEffects[currentEffects.Count-1].InitialEffect();

                //Check type
                healthBar.ApplyBleed((BleedStatus)effect.statusEffect);
                break;

            case HitType.HoT:

                AddNewStatusEffect(effect.statusEffect);

               // currentEffects[currentEffects.Count - 1].InitialEffect();

                //Check type
                healthBar.ApplyHoT((HealOverTimeStatus)effect.statusEffect);
                break;

            case HitType.StatusEffect:


                AddNewStatusEffect(effect.statusEffect);



             //   currentEffects[currentEffects.Count - 1].InitialEffect();
                break;

            //Status effect type
            default:
                Debug.Log("defaulting the ability effect");
                OnHit(effect);
                break;
            

        }

        

    }

    public void AddNewStatusEffect(StatusEffect effect)
    {

        ;
        Status relation = new Status(effect, healthBar.AddNewStatus(effect));
        currentEffects.Add(relation);

    }

    //TODO Damage reduction! AbilityType is used to determine the reduction
    private int ReduceDamage(int damage, AbilityType type)
    {
        //damage -= damage reduction

        if (damage < 0)
        {
            Debug.Log("Damage was repelled! (Dodge)");
            damage = 0;

        }
        return damage;
    }

    public virtual void PrepareForBattle()
    {
      //  healthBar.SetupBar();
        SetInitiative();
    }

    public void AnimationTrigger(string trigger)
    {
//        Debug.Log("Trying to use the animation trigger: " + trigger);
        animator.SetTrigger(trigger);
    }

    public virtual void OnRoundStart()
    {
        hasHadTurn = false;
        healthBar.SetTurnIcon();
    }

    public virtual void StartTurn()
    {
        animator.SetBool("CombatStance", true);
        healthBar.StartTurn();
        ApplyStatusEffects();
        
        if (isDead)
        {
            EndTurn();
            return;
        }
    }

    public virtual void EndTurn()
    {
        animator.SetBool("CombatStance", false);

        hasHadTurn = true;
        healthBar.SetTurnIcon();
        CombatManager.instance.EndTurn();

    }

    public virtual void ApplyStatusEffects()
    {
        for (int i = 0; i < currentEffects.Count; i++)
        {
            
            Status status = currentEffects[i];

            status.ApplyEffect();
            //Necessary?
            if (status.IsFinished()) currentEffects.Remove(status);
        }
    }

    public virtual void CheckAbilityRange()
    {
        if (currentAbility == null) return;

    }

    public virtual void UseAbility(Ability ability, ICharacter target)
    {
//        Debug.Log(name + " is preparing to cast " + ability.name + " on " + target.name);
        if (ability.CastAbility(this, target)){
          EndTurn();
        }
        else
        {
//            Debug.Log("Can't hit that target");
        }
        
    }

    public virtual void PassTurn()
    {
        healthBar.CBT.Pass();
        EndTurn();
    }

    public void EndCombat()
    {
        inCombat = false;
        animator.SetBool("CombatStance", false);
    }


    public void Die()
    {
        animator.SetTrigger("DeathTrigger");
        isDead = true;
        healthBar.HideBar(true);
        unitDied(this);
        CombatManager.instance.UnitDied(this); //Use delegate instead
    }
    #region Encapsulations

    public virtual bool isPlayer()
    {
        return false;
    }
 
    public Ability CurrentAbility
    {
        get { return currentAbility;  }

        set
        {
            currentAbility = value;
            CheckAbilityRange();
            
        }
    }

    public CombatSlot GetPosition()
    {
        return GetComponentInParent<CombatSlot>();
    }
    
    public int GetCurrentHealth()
    {
        return stats.CurrentHealth;
    }

    #endregion



}
