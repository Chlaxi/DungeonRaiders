using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatSlotPosition { Front, AggressiveMid, DefensiveMid, Backline}

public class CombatSlot : MonoBehaviour {

    public CombatSlotPosition CBPosition;
    public ICharacter unit;
    public bool isActive;
    private BoxCollider2D collider;
    public bool isInRange = false;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        Deactivate();
    }

    private void Start()
    {
        CombatManager.instance.combatEndedDelegate += CheckSurvival;
    }

    /// <summary>
    /// Setups the combat slot. Should only be called when combat is started.
    /// </summary>
    /// <param name="unit"></param>
    public void Setup(ICharacter unit)
    {
        if (unit == null) return;

        Activate();

        this.unit = unit;
        unit.unitDied += ClearDeadUnit;
        SpawnUnit();
    }

    public void SpawnUnit()
    {
        unit.SpawnCharacter();

    }

    //TODO Assign units
    /// <summary>
    /// Assigns a new unit to the combat Slot. A unitcan only be assigned, if the slot is active
    /// </summary>
    /// <param name="unit"></param>
    public void AssignUnit(ICharacter unit)
    {
        if (!isActive) return;
        this.unit = unit;
    }

    public void RemoveUnit()
    {
        this.unit = null;
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
        if (!isActive || unit == null) return;


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


    public void Activate()
    {
        collider.enabled = true;
        IsActive = true;
    }

    public void Deactivate()
    {
        collider.enabled = false;
        IsActive = false;
    }

    private void CheckSurvival()
    {
        if (unit == null)
            Deactivate();
    }

}
