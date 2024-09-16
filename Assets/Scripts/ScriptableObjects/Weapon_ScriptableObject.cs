using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Weapon_SO", menuName = "Create Weapons So", order = 1)]
public class Weapon_ScriptableObject : ScriptableObject
{
    private static Weapon_ScriptableObject instance;
    public static Weapon_ScriptableObject Instance
    {
        get
        {
            if(instance == null)
            {
                instance = Resources.Load<Weapon_ScriptableObject>("ScriptableObjects/Weapons");
            }
            return instance;
        }
    }

    public WeaponInfos[] weapons;

}

[Serializable]
public class WeaponInfos
{
    public int weaponIndex;
    public string weaponName;
    public int damage;
    public int magazineCapacity;
    public float weaponRange;
}
