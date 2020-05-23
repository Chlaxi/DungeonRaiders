using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour {

    #region Singleton
    public static CombatManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public static bool InCombat;

    public delegate void CombatStarted();
    public CombatStarted combatStartedDelegate;
    public delegate void CombatEnded();
    public CombatStarted combatEndedDelegate;


    public CombatSlot[] playerPool = new CombatSlot[4];
    public CombatSlot[] enemyPool = new CombatSlot[4];

    List<ICharacter> initiativeQueue = new List<ICharacter>();
    ICharacter currentUnit;

    private bool isPlayersTurn;

    public List<ICharacter> PlayerUnits = new List<ICharacter>();
    public List<ICharacter> EnemyUnits = new List<ICharacter>();
    private List<ICharacter> deathList = new List<ICharacter>();

    Transform graveyard;


    private int numberOfUnits;
    private int turnCounter = 0;
    private int roundCounter = 0;

    int playerCount;
    int enemyCount;

    private void Start()
    {
        
        graveyard = GameObject.Find("Graveyard").transform;
    }

    public void Setup()
    {
        SetupUnits(playerPool);
    }

    #region Combat Logic
    public void StartCombat()
    {
        if (InCombat) return;
            

        
        foreach (ICharacter corpse in graveyard.GetComponentsInChildren<ICharacter>())
        {
            Destroy(corpse.gameObject);
        }

        turnCounter = 0;
        roundCounter = 0;
        InCombat = true;
        
        //Spawns the units
        EncounterGenerator.instance.GenerateEncounter();

        SetupUnits(playerPool);
        SetupUnits(enemyPool);

     
        playerCount = PlayerUnits.Count;
        enemyCount = EnemyUnits.Count;

        combatStartedDelegate();

        StartCoroutine(CombatSpawner());
    }

    IEnumerator CombatSpawner()
    {
        CombatUIController.instance.prepareForBattleScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);

        PrepareUnitsForBattle();
        CalculateInitiative();
        CombatUIController.instance.prepareForBattleScreen.GetComponent<Animator>().SetTrigger("FadeOut");
        StartRound();
    }

    private void SetupUnits(CombatSlot[] pool)
    {

        if (pool.Equals(playerPool))
        {
            PlayerUnits.Clear();
        }

        ICharacter unit;
        for (int i = 0; i < pool.Length; i++)
        {

            if (pool[i].GetComponentInChildren<ICharacter>() != null)
            {
                unit = pool[i].GetComponentInChildren<ICharacter>();
                if (unit.isPlayer())
                {
                    PlayerUnits.Add(unit);                  
                }
                else
                {
                    EnemyUnits.Add(unit);
                }

                pool[i].Setup(unit);
            }
        }
    }

    private void StartRound()
    {
        turnCounter = 0;
        foreach (ICharacter unit in initiativeQueue)
        {
            unit.OnRoundStart();
        }

        NextTurn();
    }


    public void NextTurn()
    {
        if (!InCombat)
            return;

        isPlayersTurn = false;

        currentUnit = initiativeQueue[turnCounter];
        //initiativeQueue.RemoveAt(0);

        //        Debug.Log("Units in queue " + initiativeQueue.Count);
        //Makes sure, we don't get a unit, that doesn't exist, and that we aren't stuck in combat, if the queue is done.
        if (currentUnit == null || currentUnit.isDead)
        {
            Debug.Log("current unit is either null or dead!");
            if (initiativeQueue.Count <= 0)
            {
                Debug.Log("No queue! Ending combat");
                EndCombat();
                return;
            }
            else
            {
                Debug.Log("Unit was null! Ending turn");
                EndTurn();
            }
        }

     
        //Refactor this a bit (isPlayerturn = currentUnit.isPlayerTurn; )
        if (currentUnit.isPlayer())
        {
            isPlayersTurn = true;
            CombatUIController.instance.CoverLeftPanel(false);
        }
        else
        {
            CombatUIController.instance.CoverLeftPanel(true);
        }
        currentUnit.StartTurn();


    }

    public void PassTurn()
    {
        Debug.Log(currentUnit.name + " Passed the turn");
        EndTurn();
    }

    public void ClearAbilityTarget()
    {
//        Debug.Log("No ability! Clear all potential targets");
        foreach (CombatSlot slot in enemyPool)
        {
            slot.SetInRange(false);
        }
        foreach (CombatSlot slot in playerPool)
        {
            slot.SetInRange(false);
        }
        return;
    }

    public void GetAbilityRange(Ability ability)
    {

        //Do I need thos for the enemies, or does the AI handle it?


        CombatSlot[] selfPool;
        CombatSlot[] opponentPool;

        if (ability == null)
        {
            ClearAbilityTarget();
            return;
        }


        if (currentUnit.isPlayer())
        {
            selfPool = playerPool;
            opponentPool = enemyPool;
        }
        else
        {
            selfPool = enemyPool;
            opponentPool = playerPool;
        }
        
        //Check for allied casting
        if (ability.canCastOnAllies)
        {
            for (int i = 0; i < ability.alliedRange.Length; i++)
            {

                if(selfPool[i].unit == null) continue;

                selfPool[i].SetInRange(ability.alliedRange[i]);
                    //GetUnitFromPool(i, selfPool).healthBar.SetPotentialTargetBorder(true);
                
            }
        }


    //Check for opponent casting
        if (ability.canTargetOpponents)
        {
        
          //  Debug.Log("Ability can target opponents");
            for (int i = 0; i < ability.attackRange.Length; i++)
            {
                if(opponentPool[i].unit == null) continue;


                opponentPool[i].SetInRange(ability.attackRange[i]);
            }
        }

    }

    public ICharacter GetUnitFromPool(int slotIndex, CombatSlot[] pool)
    {

        if(slotIndex < pool.Length || slotIndex > 0)
        {
            if (pool[slotIndex].unit == null) return null;

            return pool[slotIndex].unit;
        }

        Debug.Log("A unit was not found in this slot");
        return null;
    }

    public void EndTurn()
    {
//        Debug.Log("Turn "+ turnCounter +" ended!");
        turnCounter++;
        if (turnCounter >= initiativeQueue.Count)
        {
            //Next round
  //          Debug.Log("Round " + roundCounter + " is over!");
            roundCounter++;
            StartRound();
            return;
        }


        instance.NextTurn();
    }

    public void UnitDied(ICharacter unit)
    {
       

        //Remove from queue
        if (initiativeQueue.Contains(unit))
        {
            Debug.Log(unit.name + " was killed and removed from queue");
            initiativeQueue.Remove(unit);
        }
        if (unit.isPlayer())
        {
                PlayerUnits.Remove(unit);
                playerCount--;
        }
        else
        {
            EnemyUnits.Remove(unit);
            enemyCount--;
        }

        MoveToGraveyard(unit.transform);
        //remove from list
        //Move other units forward.
        if(enemyCount <= 0)
        {
            Debug.Log("Victory!");
            EndCombat();
        }

        if(playerCount <= 0)
        {
            Debug.Log("Defeat");
            EndCombat();
        } 
    }

    public void EndCombat()
    {
        InCombat = false;
        CombatUIController.instance.CoverLeftPanel(false);
        initiativeQueue.Clear();
        foreach (ICharacter unit in PlayerUnits)
        {
            
            unit.EndCombat();
           
        }


        Debug.Log("Combat is done!");
        for (int i = 0; i < deathList.Count; i++)
        {

            ICharacter del = deathList[i];
            del.transform.SetParent(graveyard);
            
        }
        

        PlayerUnits.Clear();
        EnemyUnits.Clear();

        combatEndedDelegate();
        //End combat!!
    }

    private void MoveToGraveyard(Transform unit)
    {
        unit.transform.SetParent(graveyard);
    }
    #endregion

    #region PlayerCombat
    public void SetTarget(ICharacter target)
    {
        
        if (!isPlayersTurn)
        {
            Debug.Log("It's not the player's turn");
            return;
        }


//        Debug.Log("Target set to " + target.name);

        if(currentUnit.CurrentAbility != null)
        {
            currentUnit.UseAbility(currentUnit.CurrentAbility, target);
        }
        //target acquired
        //Throw spell
        

    }




    #endregion

    private void CalculateInitiative()
    {
        List<ICharacter> unsortedList = new List<ICharacter>();
        List<ICharacter> initiativeList = new List<ICharacter>();
   
        for (int i = 0; i < PlayerUnits.Count; i++)
        {
            if (PlayerUnits[i] != null)
            {
                unsortedList.Add(PlayerUnits[i]);
            }
        }
        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            if (EnemyUnits[i] != null)
            {
                unsortedList.Add(EnemyUnits[i]);
            }
        }


     //   Debug.Log("Calculating Initative");
        foreach (ICharacter unit in unsortedList)
        {
            if (unit != null)
            {

                for (int i = 0; i < unsortedList.Count; i++)
                {

                    if (initiativeList.Count == i)
                    {
                        initiativeList.Add(unit);
                        break;
                    }

                    if (unit.GetInitiative() > initiativeList[i].GetInitiative())
                    {

                        initiativeList.Insert(i, unit);
                        break;
                    }
                }
            }
        }
        PrintInitativeList(initiativeList.ToArray());  
       
    }

    private void PrintInitativeList(ICharacter[] list)
    {
      //  Debug.Log("Initiative list:");
        for (int i = 0; i < list.Length; i++)
        {
        //    Debug.Log(i + ": "+list[i].name +" with an initiative of "+ list[i].getInitiative());
            initiativeQueue.Add(list[i]);
        }

    }

    private void PrepareUnitsForBattle()
    {
        foreach (ICharacter unit in PlayerUnits)
        {
            if (unit == null) break;
            unit.PrepareForBattle();
        }

        foreach (ICharacter unit in EnemyUnits)
        {if (unit == null) break;
            unit.PrepareForBattle();
        }

        
    }

    public static ICharacter GetCurrentUnit()
    {
        return instance.currentUnit;
    }

    public void ReturnToTown()
    {
        if (InCombat) return;

        SceneManager.LoadScene(0);
    }
}
