using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoBehaviour
{

    PlayerUnit currentUnit;

    public CombatSlot[] combatSlots;

    public void SetupParty(PlayerUnit[] units)
    {
        Debug.Log("Setting up the party");
        for (int i = 0; i < combatSlots.Length; i++)
        {
            if (units[i] == null)
            {
                Debug.Log("unit was null");
                //TODO Disable the slot
                continue;
            }
            Instantiate(units[i].gameObject, combatSlots[i].transform);
            combatSlots[i].Setup(units[i]);
        }
    }

    public void OnCombatStart()
    {

    }


}
