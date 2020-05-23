using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    ICharacter character;
    public Classes unitClass;

    private int exp;

    public int level;
    public int availableAttPoints;
    private bool canLevelUp = false;

    private int maxHealth;
    private int currentHealth;
    //Use maths instead?
    private int[] modifiers = {-5, -4, -4, -3, -3, -2, -2, -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 16, 16, 17, 17};

    [SerializeField] private List<int> hitDieRolls = new List<int>();

    public int strength = 0;
    public int dexterity = 0;
    public int constitution = 0;
    public int intellect = 0;
    public int wisdom = 0;
    public int charisma = 0;


    public int baseAttackBonus = 0;
    [SerializeField] private int armour = 0;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        character = GetComponent<ICharacter>();
        if (unitClass == null) unitClass = Resources.Load<Classes>("Default Class");

        character.healthBar.SetupBar();

        if (level != hitDieRolls.Count)
        {
            //   Debug.Log("Not enough hit rolls!");
            for (int i = hitDieRolls.Count; i < level; i++)
            {
                RollHitDie();
            }
        }
        else
        {
            SetMaxHealth();
        }
        if (level > unitClass.BaseAttackBonus.Length)
        {
            baseAttackBonus = unitClass.BaseAttackBonus[unitClass.BaseAttackBonus.Length - 1];
        }
        else
        {
            baseAttackBonus = unitClass.BaseAttackBonus[level - 1];
        }

        currentHealth = maxHealth;
        character.onHealthChange();

    }


    public void SetMaxHealth()
    {
        int hitDieTotal = 0;
        for (int i = 0; i < hitDieRolls.Count; i++)
        {
            hitDieTotal += hitDieRolls[i];
        }
        MaxHealth = (hitDieRolls.Count * GetModifier(constitution)) + hitDieTotal; //+ gear stats;
        if (character!=null)
            character.onHealthChange();
    }

    public void ModifyHealth(EffectHitInfo hitInfo)
    {
        if (!hitInfo.isHit) return;

        if (hitInfo.hitType == HitType.Heal) currentHealth += hitInfo.hitValue;

        if (hitInfo.hitType == HitType.Hit) { 
                currentHealth -= hitInfo.hitValue;
        }

        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        if (CurrentHealth <= 0)
        {
            character.Die();
        }

        character.onHealthChange();
    }

    //TODO stat priorities?
    public void RandomlyGeneratedStats()
    {
        int[] values = new int[6];



        for (int i = 0; i < values.Length; i++)
        {
            int value = 0;

            for (int d = 0; d < 3; i++)
            {
                value += Random.Range(1, 6);
            }

            values[i] = value;

        }

        strength = values[0];
        dexterity = values[1];
        constitution = values[2];
        intellect = values[3];
        wisdom = values[4];
        charisma = values[5];


    }

    public int MaxHealth
    {
        get
        {

            return maxHealth;
        }

        set
        {
            maxHealth = value;
        }
    }

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            currentHealth = value;
        }
    }

    public bool CanLevelUp
    {
        get
        {
            return canLevelUp;
        }

        set
        {
            canLevelUp = value;
        }
    }

    public int GetArmour()
    {
        return armour;

    }

    /// <summary>
    /// Gets the raw stat values in an array.
    /// </summary>
    /// <returns> 0 = Strength, 1 = Dexterity, 2 = Constitution, 3 = Intelligence, 4 = Wisdom, 5 = Charisma </returns>
    public int[] GetStatsFromArray()
    {
        int[] stats = { strength, dexterity, constitution, intellect, wisdom, charisma };
        return stats;
    }

    public int GetRawStat(int index)
    {
        return GetStatsFromArray()[index];
    }

    public void SetStats(int[] stats)
    {
        strength = stats[0];
        dexterity = stats[1];
        constitution = stats[2];
        intellect = stats[3];
        wisdom = stats[4];
        charisma = stats[5];

    }
    /// <summary>
    /// Gets the modifers from the stats in an array
    /// </summary>
    /// <returns> 0 = Strength, 1 = Dexterity, 2 = Constitution, 3 = Intelligence, 4 = Wisdom, 5 = Charisma </returns>
    public int[] GetModifiersFromArray()
    {
        int[] modifiers = new int[6];
        int[] stats = GetStatsFromArray();
        for (int i = 0; i < modifiers.Length; i++)
        {
            modifiers[i] = GetModifier(stats[i]);
        }

        return modifiers;
    }

    /// <summary>
    /// Gets the modifer for a specific stat
    /// </summary>
    /// <param name="statScore"> A stat (i.e. Strength) </param>
    /// <returns> Returns the modified stat (i.e. a strength of 13 would return +1) </returns>
    public int GetModifier(int statScore)
    {
        if (statScore > 45) statScore = 45;
        if (statScore < 0) statScore = 0;

       return modifiers[statScore];
    }

    /// <summary>
    /// Gets the character's ability modifiers based on the modifier name
    /// </summary>
    /// <param name="type"></param>
    /// <returns>The ability modifier</returns>
    public int GetModifierFromName(AbilityModifierTypes type)
    {
        switch (type)
        {
            case AbilityModifierTypes.STR:
                return GetModifier(strength);
            
            case AbilityModifierTypes.DEX:
                return GetModifier(dexterity);

            case AbilityModifierTypes.CON:
                return GetModifier(constitution);

            case AbilityModifierTypes.INT:
                return GetModifier(intellect);

            case AbilityModifierTypes.WIS:
                return GetModifier(wisdom);

            case AbilityModifierTypes.CHA:
                return GetModifier(charisma);

            case AbilityModifierTypes.CL:
                return GetCasterLevel();

            default:
                return 0;
        }
    }

    /// <summary>
    /// Returns caster level
    /// </summary>
    /// <returns></returns>
    public int GetCasterLevel()
    {
        //Refactor: If multiclasses are used, make sure to only get the relevant caster level.
        return level;
    }

    public string GetStatName(int index)
    {
        switch (index)
        {
            case 0:
                return "STR";
            case 1:
                return "DEX";
            case 2:
                return "CON";
            case 3:
                return "INT";
            case 4:
                return "WIS";
            case 5:
                return "CHA";
            default:
                return "";
        }
    }

    public void OnLevelUp()
    {
       // Debug.Log(character.name + "Gained a level!");
        CanLevelUp = true;
        availableAttPoints = AddAttributePoints();
        
    }

    public void ApplyLevel(int[] stats)
    {
        if (!canLevelUp) return;


        availableAttPoints = 0;
        CanLevelUp = false;
        level++;
        RollHitDie();
        SetStats(stats);
        PlayerController.instance.unitInfoChanged();
    }

    private void RollHitDie()
    {
        bool rollIsAvg = true;

        int hitDie = unitClass.hitDieSize;

        if (hitDieRolls.Count >= level) return;

//        Debug.Log("You can have " + (level) + " hit dies");
        if (level == 1 || hitDieRolls.Count==0)
        {
//            Debug.Log("Level is 1, so hit die");
            hitDieRolls.Add(hitDie);
            SetMaxHealth();
            return;
        }

        int roll = 0;
        if (rollIsAvg)
        {
            int isOdd = 1;
            if (level % 2 == 0) isOdd = 0;
            roll = Mathf.RoundToInt(hitDie / 2) + isOdd;
        }
        else
        {
            roll = Random.Range(1, hitDie);
        }
        hitDieRolls.Add(roll);
 //          Debug.Log("You gained " + roll + " max health!");
        SetMaxHealth();

    }

    //TODO Get the attributepoints from the class!
    public int AddAttributePoints()
    {
        int newPoints=2;

        return newPoints;
    }

}
