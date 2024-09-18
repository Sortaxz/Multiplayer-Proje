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
    private int scannerMaximumLead = 120;
    private int scannerCurrentLead = 0;
    private int scannerBulletIndex = 0;
    
    private int mp5MaximumLead = 120;
    private int mp5CurrentLead = 0;
    private int mp5BulletIndex = 0;

    private int bulletCount = -1;
    private void Awake() 
    {
        pw = GetComponent<PhotonView>();
        gameManager = GameManager.Instance;

        EquipGunItem(weaponIndex);
        
        if(pw.IsMine)
        {
            cam = GameObject.FindWithTag("CharacterCamera").GetComponent<Camera>();
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
                    }
                    else if(weapon.weaponName == "Mp5")
                    {
                        mp5CurrentLead = weapon.magazineCapacity;
                    }
                }
                else
                {
                    weapon = new Weapon(weapons[i].gameObject.transform.GetSiblingIndex());
                    if(weapon.weaponName == "Scanner")
                    {
                        scannerCurrentLead = weapon.magazineCapacity;
                    }
                    else if(weapon.weaponName == "Mp5")
                    {
                        mp5CurrentLead = weapon.magazineCapacity;
                    } 
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
        for (int i = 0; i < gunItemHolder.childCount; i++)
        {
            if (gunItemHolder.GetChild(i).gameObject.activeSelf)
            {

                weapon = new Weapon(i);
                WeaponController weaponController = weapons[i];
                if (weapon.weaponName == "Scanner")
                {
                    scannerCurrentLead = weaponController.LeadReduction(scannerCurrentLead,weapon.magazineCapacity);
                    bulletCount = scannerCurrentLead;
                }
                else if (weapon.weaponName == "Mp5")
                {
                    mp5CurrentLead = weaponController.LeadReduction(mp5CurrentLead,weapon.magazineCapacity);
                    bulletCount = mp5CurrentLead;
                }

                CharacterGunFire(weaponController,bulletCount);

            }
        }
    }

    private void CharacterGunFire(WeaponController weaponController,int bulletCount)
    {
        weaponController.ToFire(cam, weapon.damage, weapon.magazineCapacity, weapon.weaponName,bulletCount,weapon.magazineCapacity);
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
