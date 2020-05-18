using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create Hero")]
public class PlayerHeroInfo : ScriptableObject {

    public PlayerUnit[] party = new PlayerUnit[4];

    public List<PlayerUnit> availableUnits = new List<PlayerUnit>();

    public void AddUnitToParty(PlayerUnit unit, int index)
    {
     //   Debug.Log("Found " + availableUnits.Contains(unit));
        party[index] = unit;

    }

    public void RemoveUnitFromParty(int index)
    {
        PlayerUnit unit = party[index];

    }

    public void AddUnit(PlayerUnit unit)
    {
        if (availableUnits.Contains(unit))
        {
            Debug.Log(unit.name + " is already in the list");
            return;
        }
        availableUnits.Add(unit);
    }

    public void GetStats(int index)
    {
        CharacterStats stats = availableUnits[index].GetComponent<CharacterStats>();
        Debug.Log("Slow Bob's strength: "+stats.strength);
    }

    public PlayerUnit[] GetAllUnits() 
    {
        List<PlayerUnit> allUnits = new List<PlayerUnit>();
        foreach (PlayerUnit unit in party)
        {
            if (unit == null) continue;

            allUnits.Add(unit);
        }
        allUnits.AddRange(availableUnits.ToArray());

        return allUnits.ToArray();
    }

    public int GetPartySize()
    {
        int partyCount = 0;
        foreach(PlayerUnit unit in party)
        {
            if (unit != null)
                partyCount++;
        }
        return partyCount;
    }
}
