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
    private int BulletCount = -1;
    private int bullerIndex = -1;
    private void Awake() 
    {
        gameManager = GameManager.Instance;
        //BulletsParent = GameObject.FindWithTag("BulletsParent").transform;
        
    }
    private void Start() 
    {
        
    }

    private void Update() 
    {
           
    }
    
    public void ToFire(Camera characterCamera,float damage,int magazineCapacity,string weopenName,int bulletCount,int bulletMaxCount,Vector3 direction)
    {
        if(bulletMaxCount > bulletCount)
        {
            weaponEffects[0].Play();
            audioSource[0].Play();
            print(weopenName + "-" + bullerIndex);

            Ray ray = new Ray(characterCamera.transform.position,characterCamera.transform.forward);
            RaycastHit hit;

            WeopenLeadActivated(weopenName,bullerIndex,direction);
            
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.GetComponent<PhotonView>()?.IsMine == false)
                {
                    hit.collider.GetComponent<IDamageable>()?.TakeDamage(damage);
                }
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
                        spawnBullet.transform.name = $"scanner-bullet-{i}";
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
                        spawnBullet.transform.name = $"mp5-bullet-{i}";
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
   
    public int LeadReduction(int bulletCount,int maxBulletCount)
    {
        if(bulletCount > 0)
        {
            bulletCount--;

            bullerIndex ++;
            
            return bulletCount;
        
        }
        else
        {
            BulletCount = bulletCount;
            return bulletCount;

        }
    }

    private void WeopenLeadActivated(string weopanName,int bulletIndex,Vector3 direction)
    {
        if(weopanName == "Scanner")
        {
            if(gameManager.Scanner.Count >bullerIndex)
            {
                if(!gameManager.Scanner[bulletIndex].gameObject.activeSelf && gameManager.Scanner[bulletIndex] != null)
                {
                    gameManager.Scanner[bulletIndex].gameObject.SetActive(true);
                    gameManager.Scanner[bulletIndex].gameObject.transform.SetParent(null);
                    gameManager.Scanner[bulletIndex].GetComponent<BulletController>().BulletMove(direction);
                }
            }
        }
        else if(weopanName == "Mp5")
        {
            if(gameManager.Mp5.Count >bullerIndex)
            {
                if(!gameManager.Mp5[bulletIndex].gameObject.activeSelf && gameManager.Mp5[bulletIndex] != null)
                {
                    gameManager.Mp5[bulletIndex].gameObject.SetActive(true);
                    gameManager.Mp5[bulletIndex].gameObject.transform.SetParent(null);
                    gameManager.Mp5[bulletIndex].GetComponent<BulletController>().BulletMove(direction);
                }

            }

        }
    }
}
