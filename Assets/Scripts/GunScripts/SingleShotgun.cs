using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SingleShotgun : Gun
{
    [SerializeField] private Camera cam;


    public override void Use()
    {
        Shoot();
    }

    private void Shoot()
    {

        //Ray ray = new Ray(cam.transform.position,cam.transform.forward);
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        int layerMask = 1<<3;
        layerMask =~layerMask;

        if(Physics.Raycast(ray,out hit,100f))
        {
                if(hit.collider.GetComponent<PhotonView>()?.IsMine == false)
                {
                    hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).GunDamage);
                }
        }
    }
}
