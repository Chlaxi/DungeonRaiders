using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ScriptableObject {

    int minHit=1;
    int maxHit=2;

    public int GetDamage()
    {
        return Random.Range(minHit, maxHit);
    }
	
}
