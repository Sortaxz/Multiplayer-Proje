using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotgun : Gun
{
    [SerializeField] private Camera cam;


    public override void Use()
    {
        print("Using gun " + itemInfo.itemName);
        Shoot();
    }

    private void Shoot()
    {
        //Ray ray = new Ray(cam.transform.position,cam.transform.forward);

        Ray ray = new Ray(cam.transform.position,cam.transform.forward);

        RaycastHit hit;
        int layerMask = 1<<3;
        layerMask =~layerMask;

        if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layerMask))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).GunDamage);
        }
    }
}
