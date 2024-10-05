using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform BulletExitPosition;
    [SerializeField] private Transform BulletsParent;
    private GameManager gameManager;
    [SerializeField] private ParticleSystem[] weaponEffects;
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
    [SerializeField] private Transform hedef;
    private bool characterReloading = false;
    public bool CharacterReloading { get { return characterReloading;}}
    private bool characterFire = false;
    public bool CharacterFire { get { return characterFire;}}
    
    [SerializeField] private GameObject mermiEffect;

    private void Awake() 
    {
        gameManager = GameManager.Instance;


    }
    private void Start() 
    {
        gameManager.gunEffectDelegate += GunFireSound;
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
            characterFire = true;

            //GunFireEffects();
            audioSource[0].Play();
            //gameManager.StartEveryOne(audioSource[0]);
            weaponEffects[0].Play();

            Ray ray = new Ray(characterCamera.transform.position, characterCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, weaponRange))
            {
                if (hit.collider.GetComponent<PhotonView>()?.IsMine == false)
                {
                    WeopenLeadActivated(characterCamera.transform.forward, hit.point);

                    if (hit.collider.GetComponent<PhotonView>() != null)
                    {
                        if (hit.collider.GetComponent<PhotonView>().Owner != null)
                        {

                            Player player = hit.collider.GetComponent<PhotonView>().Owner;

                            if (player.CustomProperties.TryGetValue("healt", out object hitOtherHealt))
                            {
                                if (hitOtherHealt != null)
                                {
                                    if ((float)hitOtherHealt <= damage)
                                    {
                                        GameManager.Instance.PlayerKillSkor(1, PhotonNetwork.LocalPlayer);
                                    }

                                }

                            }
                        }


                    }



                }
                else
                {
                    if (!hit.collider.CompareTag("Player"))
                    {
                        WeopenLeadActivated(characterCamera.transform.forward, hit.point);

                    }
                }


            }

        }
        else
        {
            audioSource[2].Play();
            characterFire = false;
        }
        
    }

    private void GunFireSound()
    {
        if(audioSource[0] != null)
        {
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
                        spawnBullet.GetComponent<BulletController>().bulletPosition = spawnBullet.transform.position;
                        spawnBullet.transform.name = $"scanner-bullet-{i}";
                        spawnBullet.transform.forward = direction;
                        spawnBullet.gameObject.SetActive(false);

                        if (!gameManager.Scanner.Contains(spawnBullet))
                        {
                            gameManager.Scanner.Add(spawnBullet);
                        }
                    }
                }
                /*
                for (int i = 0; i < gameManager.Scanner.Count; i++)
                {
                    gameManager.Scanner[i]?.transform.SetParent(BulletExitPosition);
                }
                */
            }
            else if(gunName == "Mp5")
            {
                if(gameManager.Mp5.Count != bulletCount)
                {
                    for (int i = 0; i < bulletCount; i++)
                    {
                        GameObject spawnBullet =  Instantiate(bullet,BulletExitPosition.position,Quaternion.identity,BulletExitPosition);
                        spawnBullet.GetComponent<BulletController>().bulletPosition = spawnBullet.transform.localPosition;
                        spawnBullet.transform.name = $"mp5-bullet-{i}";
                        spawnBullet.transform.forward = direction;
                        spawnBullet.gameObject.SetActive(false);

                        if (!gameManager.Mp5.Contains(spawnBullet))
                        {
                            gameManager.Mp5.Add(spawnBullet);
                        }
                    }
                }
                /*
                for (int i = 0; i < gameManager.Mp5.Count; i++)
                {
                    gameManager.Mp5[i]?.transform.SetParent(BulletExitPosition);
                }
                */
            }
            
        }

       
       
    }


    public void SetBulletParent(List<GameObject> weapon)
    {
        for (int i = 0; i <weapon.Count; i++)
        {
            weapon[i]?.transform.SetParent(BulletExitPosition);
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

   

    private void WeopenLeadActivated(Vector3 direction,Vector3 target)
    {
        
        if(weaponName == "Scanner")
        {
            if(gameManager.Scanner.Count >bullerIndex)
            {
                if(!gameManager.Scanner[bullerIndex].gameObject.activeSelf && gameManager.Scanner[bullerIndex] != null)
                {
                    gameManager.Scanner[bullerIndex].gameObject.SetActive(true);
                    gameManager.Scanner[bullerIndex].gameObject.transform.SetParent(null);
                    gameManager.Scanner[bullerIndex].GetComponent<BulletController>().BulletMove(direction,target,damage);

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
                    gameManager.Mp5[bullerIndex].GetComponent<BulletController>().BulletMove(direction,target,damage);
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
                    gameManager.Scanner[i].SetActive(false);
                    gameManager.Scanner[i].transform.SetParent(BulletExitPosition);

                    gameManager.Scanner[i].transform.localPosition = BulletExitPosition.transform.position;
                    gameManager.Scanner[i].transform.localRotation = BulletExitPosition.transform.rotation;

                    gameManager.Scanner[i].transform.SetSiblingIndex(i);
                    
                }
            }
        }
        else if(weaponName == "Mp5")
        {
            for (int i = 0; i < gameManager.Mp5.Count; i++)
            {
                if(gameManager.Mp5[i].transform.parent == null)
                {
                    gameManager.Mp5[i].SetActive(false);
                    gameManager.Mp5[i].transform.SetParent(BulletExitPosition);
                    gameManager.Mp5[i].transform.SetSiblingIndex(i);
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

                characterReloading = true;
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
                characterReloading = true;
            }
            else
            {
                characterReloading = false;
            }
            GameUI.Instance.WeaponInformationUi(bulletCount,maxCapacity);

            if(characterReloading)
            {
                audioSource[1].Play();
            }
        }
        else
        {
            characterReloading = false;
        }

    }

   

}
