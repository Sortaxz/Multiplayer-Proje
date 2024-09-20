using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon 
{
    public int weopanIndex;
    public string weaponName;
    public int damage;
    public int magazineCapacity;
    public float weaponRange;
    public int maxCapacity;
    public Weapon(int weapon_SO_Index)
    {
        weopanIndex = Weapon_ScriptableObject.Instance.weapons[weapon_SO_Index].weaponIndex;
        weaponName = Weapon_ScriptableObject.Instance.weapons[weapon_SO_Index].weaponName;        
        damage = Weapon_ScriptableObject.Instance.weapons[weapon_SO_Index].damage;        
        magazineCapacity = Weapon_ScriptableObject.Instance.weapons[weapon_SO_Index].magazineCapacity;        
        weaponRange = Weapon_ScriptableObject.Instance.weapons[weapon_SO_Index].weaponRange;        
        maxCapacity = Weapon_ScriptableObject.Instance.weapons[weapon_SO_Index].maxCapacity;
    }

    
}
