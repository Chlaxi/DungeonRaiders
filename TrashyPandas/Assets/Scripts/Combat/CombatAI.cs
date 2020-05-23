using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIPersonality { Aggressive, Defensive, Healer, Assassin}
public enum AIAction { Attack, Heal, Buff}

public class CombatAI : MonoBehaviour {

    public AIPersonality personality;
    public AIAction action;
    private Ability[] abilities;
    private CombatSlot combatSlot;
    private int currentPosition;
    private ICharacter self;

    private void Start()
    {
        self = GetComponent<ICharacter>();
        abilities = self.abilities.ToArray();
    }

    public void DecideAction()
    {
        if (self.isDead) return;
        //Based on personality, number of allies, own health, opponent health, and other stuff, a decision will be made:
        //Will it heal an ally? Damage an opponent? etc.
        combatSlot = GetComponentInParent<CombatSlot>();
        currentPosition = combatSlot.GetPositionIndex();
        action = AIAction.Attack;
        
        DetermineTarget(DetermineAblity());
        
    }

    private Ability DetermineAblity()
    {



        List<Ability> availableAbilities = new List<Ability>();
        for (int i = 0; i < abilities.Length; i++)
        {
            Ability ability = abilities[i];
            CombatSlot[] targetPool = GetPool(ability);
            bool[] range;

            if (ability.requiredPosition[currentPosition])
            {
                if (ability.canTargetOpponents)
                {
                    range = ability.attackRange;
                }
                else
                {
                    range = ability.alliedRange;
                }

                for (int j = 0; j < targetPool.Length; j++)
                {
                    if (!range[j]) continue;

                    if (targetPool[j].GetUnit() == null)
                    {
                        continue;
                    }
                    else
                    { 
                    availableAbilities.Add(ability);
                    break;
                    }
                    
               
                }

            }
             
        }

        if (availableAbilities.Count == 0)
        {
           
            //throw new System.Exception
            Debug.Log(self.name + " can't hit from this position! " + currentPosition);
            return null;

        }


        switch (personality)
        {



            default:        //Random available ability
                return availableAbilities[Random.Range(0, availableAbilities.Count)];
        }
    }

    private void DetermineTarget(Ability ability)
    {
        ICharacter target = null;
        if (ability == null)
        {
            self.PassTurn();
            return;
        }
        CombatSlot[] targetPool = GetPool(ability);
  

        if (ability.canCastOnSelf)
        {
            Debug.Log("Self cast!");
            StartCoroutine(Act(ability, self));
            return;
        }

//        Debug.Log("Trying to use " + ability.name);
        bool[] range = GetRange(ability);



        for (int i = 0; i < range.Length; i++)
        {
            //if (targetPool.Length < i) break;
            
            if (!range[i] || targetPool[i].unit == null) continue;
   
            if (target == null)
            {
                if (targetPool[i].unit.isDead) continue;
                target = targetPool[i].unit;
                
   //             Debug.Log("Target set to " + target.name);
                continue;
            }

            //TODO Get rid of switch, by using plugable AI, where the AI behaviour is inside the ScriptableObject
            switch (personality)
            {
                case AIPersonality.Assassin:
                    if(targetPool[i].unit != null)
                    {
                        if (targetPool[i].unit.isDead) continue;

                        if(targetPool[i].unit.stats.CurrentHealth < target.stats.CurrentHealth)
                        {
                            target = targetPool[i].unit;
                        }
                                
                    }
                    break;


                    //Defaults to a random target
                default:
                    target = GetRandomUnitFromPool(targetPool, range);
                    Debug.Log("Target changed to " + target.name);
                    break;
            }

        }
        StartCoroutine(Act(ability, target));
    }

    

    private IEnumerator Act(Ability ability, ICharacter target)
    {
        if (target == null)
        {
            self.PassTurn();
            Debug.Log("No target!");//throw new System.Exception("No target!");

        }
        if (ability == null)
        {
            self.PassTurn();
            Debug.Log("No ability!");
            //   throw new System.Exception("No ability!");

        }
        Debug.Log("Success! Casting "+ability.name+" on "+target.name);
        yield return new WaitForSeconds(1);
        self.UseAbility(ability, target);
    }

    public bool InRange(bool range)
    {
        if (range) return true;

        return false;

    }

    private ICharacter GetRandomUnitFromPool(CombatSlot[] pool, bool[] range)
    {
        bool minSet = false;
        int min = 0;
        int max = pool.Length;

        for (int i = 0; i < pool.Length; i++)
        {
            if (!range[i]) continue;

            if (pool[i] == null) continue;

            if (!minSet)
            {
                minSet = true;
                min = i;
            }

            max = i;
        }

        Debug.Log("Random generated with a range of " + min + "-" + max);
        ICharacter unit = pool[Random.Range(min, max)].unit;
        Debug.Log(unit.name);
        return unit;

    }

    private CombatSlot[] GetPool(Ability ability)
    {
        bool[] range = ability.attackRange;

        if (ability.canTargetOpponents)
        {
            return CombatManager.instance.playerPool;
            //range = ability.behaviour.attackRange;
        }
        else if (ability.canCastOnAllies)
        {
           return CombatManager.instance.enemyPool;
           // range = ability.behaviour.alliedRange;
        }
        return null;
    }

    private bool[] GetRange(Ability ability)
    {

        if (ability.canTargetOpponents)
        {
            return ability.attackRange;
        }
        return ability.alliedRange;              
    }
}
