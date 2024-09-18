using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField] protected ItemInfo itemInfo;
    [SerializeField] protected GameObject gunItemGameObject;
    public GameObject GunItemGameObject { get { return gunItemGameObject;}}

    public abstract void Use();
}
