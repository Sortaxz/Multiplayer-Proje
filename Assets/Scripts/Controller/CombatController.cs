using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CombatController : PlayerInputManager
{
    private GameManager gameManager;
    private CharacterAnimation characterAnimation;
    private CharacterControl characterControl;
    private Weapon weapon;
    [SerializeField] private WeaponController[] weapons;
    public WeaponController[] Weapons { get { return weapons; } }
    [SerializeField] private Transform gunItemHolder;
    private PhotonView pw;
    [SerializeField]private Camera cam;
    private int weaponIndex;

    private int previousWeaponIndex = -1;


    private void Awake() 
    {
        pw = GetComponent<PhotonView>();
        gameManager = GameManager.Instance;
        
        characterControl = GetComponent<CharacterControl>();
    }

    void Start()
    {
        
        if(pw.IsMine)
        {
            characterAnimation = GetComponent<CharacterAnimation>();
            EquipGunItem(weaponIndex);
            cam = GameObject.FindWithTag("CharacterCamera").GetComponent<Camera>();
            
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

    void Update()
    {
        if (!pw.IsMine)
            return;
        if(!gameManager.CharacterDead)
        {
            if(!gameManager.GameStopted)
            {
                if(rewenal)
                {
                    weapons[weaponIndex].MagazineControl();
                    characterAnimation.reloading = weapons[weaponIndex].CharacterReloading;

                }
                else
                {
                    characterAnimation.reloading = false;

                }

                if(mousePressedLeftButton)
                {
                    characterAnimation.fire = weapons[weaponIndex].CharacterFire;
                }
                else
                {
                    characterAnimation.fire = false;
                }
                
                
                WeaponSelection();

                ShootControl();
            }
        }

    }
    
    private void ShootControl()
    {
        if(!pw.IsMine)
            return;

        if(!gameManager.CharacterDead)
        {
            if (mousePressedLeftButton)
            {
                Shoot();

            }
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
        weaponController.ToFire(cam,transform,cam.transform.forward);
    }

    private void WeaponSelection()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if(Input.GetKeyDown((i+1).ToString()))
            {
                weaponIndex = i;
                EquipGunItem(weaponIndex);
                break;
            }
        }

        if(mouseScrollWhell>0f)
        {

            if(weaponIndex >= weapons.Length -1)
            {
                weaponIndex = 0;
                EquipGunItem(weaponIndex);
            }
            else
            {
                weaponIndex+=1;
                EquipGunItem(weaponIndex);
            }
        }
        else if(mouseScrollWhell < 0f)
        {

            if(weaponIndex <= 0)
            {
                weaponIndex = weapons.Length-1;
                EquipGunItem(weaponIndex);
            }
            else
            {
                weaponIndex-=1;
                EquipGunItem(weaponIndex);
            }
        }

        GameUI.Instance.WeaponInformationUi(weapons[weaponIndex].BulletCount,weapons[weaponIndex].MaxCapacity);
        
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

            if(weapon.weaponName == "Scanner")
            {
                weapons[weaponIndex].SetBulletParent(gameManager.Scanner);
            }
            else if(weapon.weaponName == "Mp5")
            {
                weapons[weaponIndex].SetBulletParent(gameManager.Mp5);
            }
        }
       

    }
}
