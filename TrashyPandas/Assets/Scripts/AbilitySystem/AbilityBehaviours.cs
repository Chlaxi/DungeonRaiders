using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviourStartTimes
{
    Beginning, Middle, End
}



public abstract class AbilityBehaviours : ScriptableObject{


    public bool canCrit;
    public bool guaranteedHit;

    public bool[] hitmodifiers = new bool[6];

    public bool canCastOnSelf = false;
    public bool canCastOnAllies = false;
    public bool canTargetOpponents = true;
    public bool AoE = false;

    public bool[] requiredPosition = new bool[4];
    public bool[] attackRange = new bool[4];
    public bool[] alliedRange = new bool[4];

}
