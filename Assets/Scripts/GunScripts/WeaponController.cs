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
    [SerializeField] private ParticleSystem[] weaponEffects;
    [SerializeField] private AudioClip[] weaponAudioClips;
    [SerializeField] private AudioSource[] audioSource;
    private PhotonView weopanPw;
    private void Awake() 
    {
        gameManager = GameManager.Instance;
        BulletsParent = GameObject.FindWithTag("BulletsParent").transform;
        
    }
    private void Start() 
    {
        
    }

    private void Update() 
    {
           
    }
    
    public void ToFire(Camera characterCamera,float damage,int magazineCapacity,string weopenName)
    {
        weaponEffects[0].Play();
        audioSource[0].Play();
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
            if(gunName == "Scanner")
            {
                if(gameManager.Scanner.Count != bulletCount)
                {
                    for (int i = 0; i < bulletCount; i++)
                    {
                        GameObject spawnBullet =  Instantiate(bullet,BulletExitPosition.position,Quaternion.identity,BulletExitPosition);
                        spawnBullet.transform.forward = direction;
                        spawnBullet.gameObject.SetActive(false);

                        if (!gameManager.Scanner.Contains(spawnBullet))
                        {
                            gameManager.Scanner.Add(spawnBullet);
                        }
                    }
                }
            }
            else if(gunName == "Mp5")
            {
                if(gameManager.Mp5.Count != bulletCount)
                {
                    for (int i = 0; i < bulletCount; i++)
                    {
                        GameObject spawnBullet =  Instantiate(bullet,BulletExitPosition.position,Quaternion.identity,BulletExitPosition);
                        spawnBullet.transform.forward = direction;
                        spawnBullet.gameObject.SetActive(false);

                        if (!gameManager.Mp5.Contains(spawnBullet))
                        {
                            gameManager.Mp5.Add(spawnBullet);
                        }
                    }
                }
            }
            
        }
    }
   

}
