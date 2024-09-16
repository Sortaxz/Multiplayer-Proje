using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform BulletExitPosition;
    [SerializeField] private Transform BulletsParent;
    private GameManager gameManager;
    private void Awake() 
    {
        gameManager = GameManager.Instance;
        BulletsParent = GameObject.FindWithTag("BulletsParent").transform;
    }

    void Start()
    {
    }

    void Update()
    {
        
       
    }

    private void FixedUpdate() 
    {
    }
    
    public static void ToFire(Camera characterCamera,float damage,int magazineCapacity)
    {
        Ray ray = characterCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.GetComponent<PhotonView>()?.IsMine == false)
            {
                hit.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
            }
        }
    }
    
    public void CreateBullet(int bulletCount,string gunName,Vector3 direction)
    {
        if(transform.name == gunName)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                GameObject spawnBullet =  Instantiate(bullet,BulletExitPosition.position,Quaternion.identity,BulletsParent);
                spawnBullet.transform.forward = direction;
                spawnBullet.gameObject.SetActive(false);
                if(gunName == "Scanner")
                {
                    if(!gameManager.Scanner.Contains(spawnBullet))
                    {
                        gameManager.Scanner.Add(spawnBullet);
                    }
                }
                if(gunName == "Mp5")
                {
                    print("Mp5");
                    if(!gameManager.Mp5.Contains(spawnBullet))
                    {
                        gameManager.Mp5.Add(spawnBullet);
                    }
                }
            }
        }
    }
   

}
