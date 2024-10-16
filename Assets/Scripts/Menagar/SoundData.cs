using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create Cound",menuName = "Soun SO",order = 3)]
public class SoundData : ScriptableObject
{
    public WeaponSound[] weaponSound;
    public AudioSource[] playerSound;

}

[System.Serializable]
public class WeaponSound
{
    public string weaponName;
    public AudioClip[] weaponSoundClip;
}