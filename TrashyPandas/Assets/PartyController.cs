using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyController : MonoBehaviour
{
    #region Singleton
    public static PartyController instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] PlayerUnit currentUnit;

    public CombatSlot[] combatSlots;

    public IEnumerator SetupParty(PlayerUnit[] units)
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
            GameObject hero = Instantiate(units[i].gameObject, combatSlots[i].transform);
  
            combatSlots[i].Setup(units[i]);
        
        }
        CombatManager.instance.Setup(); //Otherwise the characters aren't loaded properly. Figure out why.

        yield return new WaitForSeconds(0.5f);
        
        SetCurrentUnit(0);
    }

    public void OnCombatStart()
    {

    }

    public PlayerUnit GetCurrentUnit()
    {
        return currentUnit;
    }

    public void SetCurrentUnit(int index)
    {
        //Check length
        if (index < 0)
            return;

        currentUnit = (PlayerUnit)combatSlots[index].unit;
        ShowInfo();
    }

    /// <summary>
    /// Shows the info of the currently selected partymember
    /// </summary>
    public void ShowInfo()
    {
        if (currentUnit == null)
            return;

        if (currentUnit.stats == null)
            throw new System.Exception("stats not set up");

        CombatUIController.instance.UpdateLeftPanel(currentUnit);
    }
}
