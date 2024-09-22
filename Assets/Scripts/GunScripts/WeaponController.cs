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
    private int bulletCount = -1;
    public int BulletCount { get { return bulletCount; }}
    private int bullerIndex = -1;

    private int weopanIndex;
    private string weaponName;
    private int damage;
    private int magazineCapacity;
    private float weaponRange;
    private int maxCapacity;
    public int MaxCapacity { get { return maxCapacity;}}
    private void Awake() 
    {
        gameManager = GameManager.Instance;
        
    }
    private void Start() 
    {
        
    }

    private void Update() 
    {
           
    }
    
    public void SetWeaponInfo(int _weopnIndex,string _weaponName,int _damage,int _magazineCapacity,float _weaponRange,int _weaponMaxCapacity)
    {
        weopanIndex = _weopnIndex;
        damage = _damage;
        magazineCapacity = _magazineCapacity;
        weaponRange = _weaponRange;
        bulletCount = _magazineCapacity;
        maxCapacity = _weaponMaxCapacity;
        weaponName = _weaponName;
        GameUI.Instance.WeaponInformationUi(BulletCount,maxCapacity);
        
    }

    public void ToFire(Camera characterCamera,Vector3 direction)
    {
        if(bulletCount > 0)
        {
            if(bulletCount > 0) 
            {
                weaponEffects[0].Play();
                audioSource[0].Play();
                

                Ray ray = new Ray(characterCamera.transform.position,characterCamera.transform.forward);

                WeopenLeadActivated(direction);
                
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
    
    

    public void LeadReduction()
    {
        if(bulletCount > 0)
        {
            bulletCount--;

            bullerIndex++;
            GameUI.Instance.WeaponInformationUi(bulletCount,maxCapacity);
        }
        

    }

   

    private void WeopenLeadActivated(Vector3 direction)
    {
        if(weaponName == "Scanner")
        {
            if(gameManager.Scanner.Count >bullerIndex)
            {
                if(!gameManager.Scanner[bullerIndex].gameObject.activeSelf && gameManager.Scanner[bullerIndex] != null)
                {
                    gameManager.Scanner[bullerIndex].gameObject.SetActive(true);
                    gameManager.Scanner[bullerIndex].gameObject.transform.SetParent(null);
                    gameManager.Scanner[bullerIndex].GetComponent<BulletController>().BulletMove(direction,damage);

                }
            }
        }
        else if(weaponName == "Mp5")
        {
            if(gameManager.Mp5.Count >bullerIndex)
            {
                if(!gameManager.Mp5[bullerIndex].gameObject.activeSelf && gameManager.Mp5[bullerIndex] != null)
                {
                    gameManager.Mp5[bullerIndex].gameObject.SetActive(true);
                    gameManager.Mp5[bullerIndex].gameObject.transform.SetParent(null);
                    gameManager.Mp5[bullerIndex].GetComponent<BulletController>().BulletMove(direction,damage);
                }

            }

        }
        
    }

    public void MagazineControl()
    {
        bullerIndex = -1;
        if(weaponName == "Scanner")
        {
            for (int i = 0; i < gameManager.Scanner.Count; i++)
            {
                if(gameManager.Scanner[i].transform.parent == null)
                {
                    gameManager.Scanner[i].transform.SetParent(BulletExitPosition);
                    gameManager.Scanner[i].SetActive(false);
                    gameManager.Scanner[i].transform.SetSiblingIndex(i);
                    gameManager.Scanner[i].GetComponent<BulletController>().SetBulletTransformRotation(BulletExitPosition.transform.position);
                }
            }
        }
        else if(weaponName == "Mp5")
        {
            for (int i = 0; i < gameManager.Mp5.Count; i++)
            {
                if(gameManager.Mp5[i].transform.parent == null)
                {
                    gameManager.Mp5[i].transform.SetParent(BulletExitPosition);
                    gameManager.Mp5[i].SetActive(false);
                    gameManager.Mp5[i].transform.SetSiblingIndex(i);
                    gameManager.Mp5[i].GetComponent<BulletController>().SetBulletTransformRotation(BulletExitPosition.transform.position);
                }
            }
        }

        if(maxCapacity > 0)
        {
            if(bulletCount < magazineCapacity)
            {
                int courseAdd = magazineCapacity - bulletCount;
                int a = maxCapacity - courseAdd;

                if(a > 0)
                {
                    bulletCount += courseAdd;
                    maxCapacity -= courseAdd;
                }
                else
                {
                    bulletCount += maxCapacity;
                    maxCapacity = 0;
                }

                
                print($"{weaponName}'un kurşun sayisi : {bulletCount} ve Toplam kalan kurşun sayisi : {maxCapacity}");
            }
            else if(bulletCount <= 0)
            {
                int a = maxCapacity - magazineCapacity;
                
                if( a > 0)
                {
                    bulletCount = magazineCapacity;
                    maxCapacity -= magazineCapacity;
                }
                else
                {
                    bulletCount = maxCapacity;
                    maxCapacity = 0;
                }
            }
            GameUI.Instance.WeaponInformationUi(bulletCount,maxCapacity);

        }
       

    }
}
