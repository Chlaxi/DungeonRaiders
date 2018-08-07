using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatSlotPosition { Front, AggressiveMid, DefensiveMid, Backline}

public class CombatSlot : MonoBehaviour {

    public CombatSlotPosition CBPosition;
    public ICharacter unit;
    public bool isActive;

    public bool isInRange = false;

    /// <summary>
    /// Setups the combat slot. Should only be called when combat is started.
    /// </summary>
    /// <param name="unit"></param>
    public void Setup(ICharacter unit)
    {
        this.unit = unit;
        unit.unitDied += ClearDeadUnit;
        isActive = true;
        SpawnUnit();
    }

    public void SpawnUnit()
    {
        unit.SpawnCharacter();

    }

    /// <summary>
    /// Assigns a new unit to the combat Slot. A unitcan only be assigned, if the slot is active
    /// </summary>
    /// <param name="unit"></param>
    public void AssignUnit(ICharacter unit)
    {
        if (!isActive) return;
        this.unit = unit;
        


    }

    public void ClearDeadUnit(ICharacter corpse)
    {
        Debug.Log(corpse.name+" has met their end");
        unit = null;
    }

    public bool IsActive
    {
        get { return isActive; }

        set { isActive = value; }

    }

    public void SetInRange(bool isInRange)
    {
        this.isInRange = isInRange;

        if (unit == null) return;

        unit.healthBar.SetPotentialTargetBorder(isInRange);
        
    }

    public void OnClick()
    {
        if (!isActive) return;


        PlayerController.instance.CurrentTarget = unit;
        unit.ShowUI();

        if (isInRange)
        {
            CombatManager.instance.SetTarget(unit);
        }

    }

    public int GetPositionIndex()
    {
        return (int) CBPosition;
    }

    public ICharacter GetUnit()
    {
        if (unit == null) GetComponentInChildren<ICharacter>();
        return unit;
    }

}
