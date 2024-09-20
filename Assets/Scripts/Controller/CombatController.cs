using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CombatController : InputManager
{
    private GameManager gameManager;
    private Weapon weapon;
    [SerializeField] private WeaponController[] weapons;
    [SerializeField] private Transform gunItemHolder;
    private PhotonView pw;
    [SerializeField]private Camera cam;
    private bool fire;
    [SerializeField] private float timingFiring =0f;
    private int weaponIndex;

    private int previousWeaponIndex = -1;


    private void Awake() 
    {
        pw = GetComponent<PhotonView>();
        gameManager = GameManager.Instance;

        EquipGunItem(weaponIndex);
        
        if(pw.IsMine)
        {
            cam = GameObject.FindWithTag("CharacterCamera").GetComponent<Camera>();
            /*
            for (int i = 0; i < weapons.Length; i++)
            {
                if(weapons[i].gameObject.activeSelf)
                {
                    weapon = new Weapon(weapons[i].gameObject.transform.GetSiblingIndex());
                    print(weapon.weaponName);
                    weaponIndex = weapon.weopanIndex;
                    if(weapon.weaponName == "Scanner")
                    {
                        scannerCurrentLead = weapon.magazineCapacity;
                        scannerMaximumLead = weapon.maxCapacity;
                        scannerMagazineCapacity = weapon.magazineCapacity;
                    }
                    else if(weapon.weaponName == "Mp5")
                    {
                        mp5CurrentLead = weapon.magazineCapacity;
                        mp5MaximumLead = weapon.maxCapacity;
                        mp5MagazineCapacity = weapon.magazineCapacity;
                    }
                }
                else
                {
                    weapon = new Weapon(weapons[i].gameObject.transform.GetSiblingIndex());
                    if(weapon.weaponName == "Scanner")
                    {
                        scannerCurrentLead = weapon.magazineCapacity;
                        scannerMaximumLead = weapon.maxCapacity;
                        scannerMagazineCapacity = weapon.magazineCapacity;
                    }
                    else if(weapon.weaponName == "Mp5")
                    {
                        mp5CurrentLead = weapon.magazineCapacity;
                        mp5MaximumLead = weapon.maxCapacity;
                        mp5MagazineCapacity = weapon.magazineCapacity;
                    } 
                }
            }
            */
            
            for (int i = 0; i < weapons.Length; i++)
            {
                if(weapons[i].gameObject.activeSelf)
                {
                    weapon = new Weapon(weapons[i].gameObject.transform.GetSiblingIndex());
                    weapons[i].SetWeaponInfo(weapon.weopanIndex,weapon.weaponName,weapon.damage,weapon.magazineCapacity,weapon.weaponRange,weapon.maxCapacity);
                    
                }
                else
                {
                    weapon = new Weapon(weapons[i].gameObject.transform.GetSiblingIndex());
                    weapons[i].SetWeaponInfo(weapon.weopanIndex,weapon.weaponName,weapon.damage,weapon.magazineCapacity,weapon.weaponRange,weapon.maxCapacity);
                }
            }
        }
        
    }

    void Start()
    {
    }

    void Update()
    {
        if (!pw.IsMine)
            return;
        
        if(rewenal)
        {
            weapons[weaponIndex].MagazineControl();
            
        }
        
        
        WeaponSelection();

        ShootControl();

    }
    
    private void ShootControl()
    {
        if(!pw.IsMine)
            return;

        if (mousePressedLeftButton)
        {
            Shoot();

        }
       



    }

    
    private void Shoot()
    {
        /*
        for (int i = 0; i < gunItemHolder.childCount; i++)
        {
            if (gunItemHolder.GetChild(i).gameObject.activeSelf)
            {

                weapon = new Weapon(i);
                WeaponController weaponController = weapons[i];
                if (weapon.weaponName == "Scanner")
                {
                    scannerCurrentLead = weaponController.LeadReduction(scannerCurrentLead,weapon.magazineCapacity,weapon.maxCapacity);
                    bulletCount = scannerCurrentLead;
                }
                else if (weapon.weaponName == "Mp5")
                {
                    mp5CurrentLead = weaponController.LeadReduction(mp5CurrentLead,weapon.magazineCapacity,weapon.maxCapacity);
                    bulletCount = mp5CurrentLead;
                }

                CharacterGunFire(weaponController,bulletCount,weapon.maxCapacity);

            }
        }
        */
        for (int i = 0; i < gunItemHolder.childCount; i++)
        {
            if (gunItemHolder.GetChild(i).gameObject.activeSelf)
            {

                weapon = new Weapon(i);
                WeaponController weaponController = weapons[i];
                if (weapon.weaponName == "Scanner")
                {
                    weaponController.LeadReduction();
                }
                else if (weapon.weaponName == "Mp5")
                {
                    weaponController.LeadReduction();
                }

                CharacterGunFire(weaponController);

            }
        }
    }

    private void CharacterGunFire(WeaponController weaponController)
    {
        //weaponController.ToFire(cam, weapon.damage, weapon.magazineCapacity,maxCapacity, weapon.weaponName,bulletCount,cam.transform.forward);
        weaponController.ToFire(cam,cam.transform.forward);
    }

    private void WeaponSelection()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if(Input.GetKeyDown((i+1).ToString()))
            {
                EquipGunItem(i);
                break;
            }
        }

        if(mouseScrollWhell>0f)
        {

            if(weaponIndex >= weapons.Length -1)
            {
                EquipGunItem(0);
            }
            else
            {
                EquipGunItem(weaponIndex + 1);
            }
        }
        else if(mouseScrollWhell < 0f)
        {

            if(weaponIndex <= 0)
            {
                EquipGunItem(weapons.Length-1);
            }
            else
            {
                EquipGunItem(weaponIndex - 1);
            }
        }

       
        
    }

    public void EquipGunItem(int gunItemIndex)
    {
        if(gunItemIndex == previousWeaponIndex)
            return;

        weaponIndex = gunItemIndex;

        weapons[gunItemIndex].gameObject.SetActive(true);

        if(previousWeaponIndex != -1)
        {
            weapons[previousWeaponIndex].gameObject.SetActive(false);
        }

        previousWeaponIndex = gunItemIndex;

        if(pw.IsMine)
        {
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable();
            playerProps.Add("gunItemIndex",gunItemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        }

        if(weapons[weaponIndex].gameObject.activeSelf)
        {
            weapon = new Weapon(weaponIndex);
            weapons[weaponIndex].CreateBullet(weapon.magazineCapacity,weapon.weaponName,transform.forward);
        
        
        }

    }
}
