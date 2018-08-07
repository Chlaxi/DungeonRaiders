using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EncounterDifficulty  { Easy, Normal, Hard}
public enum EnemyLevel { Lower, Same, Higher}

public class EncounterGenerator : MonoBehaviour {

    public static EncounterGenerator instance;


    private void Awake()
    {
        instance = this;
    }


    private int maxEncounterSize =  4;

    public int encounterSize = 3;

    public EncounterDifficulty encounterDifficulty;
    private EnemyLevel enemyLevel;
    public int levelOffset;

    public GameObject[] availableEnemies;
    public GameObject[] CombatSlots;

    public void GenerateEncounter()
    {
        GenerateDifficulty();

        CalculateEnemyLevels();

        GenerateEnemies();
    }



    private void GenerateDifficulty()
    {
        switch (encounterDifficulty)
        {
            case EncounterDifficulty.Easy:
                //Well shit
                if (GetChance() < 4)
                {
                    encounterSize = 3;
                    enemyLevel = EnemyLevel.Higher;
                }//Somewhat difficult
                else if (GetChance() > 5 && GetChance() < 8)
                {
                    encounterSize = 3;
                    enemyLevel = EnemyLevel.Same;
                }//still difficult, but not a problem
                else if (GetChance() > 8 && GetChance() < 12)
                {
                    encounterSize = 2;
                    enemyLevel = EnemyLevel.Higher;
                }
                else if (GetChance() > 12 && GetChance() < 16)
                {
                    encounterSize = 2;
                    enemyLevel = EnemyLevel.Same;
                }
                else if (GetChance() > 16)
                {
                    encounterSize = 2;
                    enemyLevel = EnemyLevel.Lower;
                }
                    break;


            case EncounterDifficulty.Normal:
                //Well shit
                if (GetChance() < 4)
                {
                    encounterSize = Random.RandomRange(3, 4);
                    enemyLevel = EnemyLevel.Higher;
                }//Somewhat difficult
                else if (GetChance() > 5 && GetChance() < 8)
                {
                    encounterSize = Random.RandomRange(3, 4);
                    enemyLevel = EnemyLevel.Same;
                }//still difficult, but not a problem
                else if (GetChance() > 8 && GetChance() < 12)
                {
                    encounterSize = Random.Range(2, 3);
                    enemyLevel = EnemyLevel.Same;
                }
                else if (GetChance() > 12 && GetChance() < 16)
                {
                    encounterSize = Random.Range(2, 3);
                    enemyLevel = (EnemyLevel)Random.Range(1, 2);
                }
                else if (GetChance() > 16)
                {
                    encounterSize = 2;
                    enemyLevel = EnemyLevel.Same;
                }
                break;

            case EncounterDifficulty.Hard:
                //Well shit
                if (GetChance() < 4)
                {
                    encounterSize = 4;
                    enemyLevel = EnemyLevel.Higher;
                }//Somewhat difficult
                else if (GetChance() > 5 && GetChance() < 8)
                {
                    encounterSize = Random.RandomRange(3,4);
                    enemyLevel = (EnemyLevel)Random.Range(1, 2);
                }//still difficult, but not a problem
                else if (GetChance() > 8 && GetChance() < 12)
                {
                    encounterSize = Random.Range(2,4);
                    enemyLevel = (EnemyLevel)Random.Range(1, 2);
                }
                else if (GetChance() > 12 && GetChance() < 16)
                {
                    encounterSize = Random.Range(2, 3);
                    enemyLevel = EnemyLevel.Higher;
                }
                else if (GetChance() > 16)
                {
                    encounterSize = 2;
                    enemyLevel = EnemyLevel.Higher;
                }
                break;

        }

//        Debug.Log("You got an opponent pool based on: " + enemyLevel + " with " + encounterSize + " enemies");

     
        
        //Instantiate them (Maybe use a pool?)
        //Populate them to the CombatSlots
        //Determine Loot?

    }

    public void CalculateEnemyLevels()
    {

        //Get the average player level

        levelOffset = 0;

        switch (enemyLevel)
        {
            case EnemyLevel.Higher:
                if (encounterDifficulty == EncounterDifficulty.Easy)
                    levelOffset = 1;


                if (encounterDifficulty == EncounterDifficulty.Normal)
                    levelOffset = Random.Range(1, 3);

                if (encounterDifficulty == EncounterDifficulty.Hard)
                    levelOffset = Random.Range(2, 4);

                break;


            case EnemyLevel.Lower:
                if (encounterDifficulty == EncounterDifficulty.Easy)
                    levelOffset -= Random.Range(2, 4);


                if (encounterDifficulty == EncounterDifficulty.Normal)
                    levelOffset -= Random.Range(1, 3);

                if (encounterDifficulty == EncounterDifficulty.Hard)
                    levelOffset -= 1;

                break;

            default:
                levelOffset = 0;
                break;
        }

    }

    private void GenerateEnemies()
    {
        //Spawn random classes! 

        //int tanks
        //int damage dealers
        //int healers
        List<GameObject> enemiesToSpawn = new List<GameObject>();
        for (int i = 0; i < encounterSize; i++)
        {
            GameObject unit = availableEnemies[Random.Range(0, availableEnemies.Length)];

            SpawnEnemy(unit, i);
        }
    }

    public void SpawnEnemy(GameObject unit, int index)
    {
        Instantiate(unit, CombatSlots[index].transform);       
    }

    private int GetChance()
    {
        return Random.Range(0, 20);

    }

}
