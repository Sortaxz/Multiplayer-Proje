using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CombatController : InputManager
{
    private Weapon weapon;
    [SerializeField] private WeaponController[] weapons;
    [SerializeField] private Transform gunItemHolder;
    private PhotonView pw;
    [SerializeField]private Camera cam;
    private bool fire;
    [SerializeField] private float timingFiring =0f;
    
    private void Awake() 
    {
        pw = GetComponent<PhotonView>();
        cam = GameObject.FindWithTag("CharacterCamera").GetComponent<Camera>();

        for (int i = 0; i < weapons.Length; i++)
        {
            if(weapons[i].gameObject.activeSelf)
            {
                weapon = new Weapon(i);
                weapons[i].CreateBullet(weapon.magazineCapacity,weapon.weaponName,transform.forward);
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

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(weapons[1].gameObject.activeSelf)
            {
                weapon = new Weapon(1);
                weapons[1].CreateBullet(weapon.magazineCapacity,weapon.weaponName,transform.forward);
            }
            
                
        }
        

        ShootControl();

    }
    
    private void ShootControl()
    {
        if (mousePressedLeftButton)
        {
            fire = true;

        }
        if (mousePressedLeftLeave)
        {
            fire = false;
        }



        Shoot();
    }

    private void FixedUpdate()
    {
       
    }

    private void Shoot()
    {
        if (fire && timingFiring > .8f)
        {
            for (int i = 0; i < gunItemHolder.childCount; i++)
            {
                if (gunItemHolder.GetChild(i).gameObject.activeSelf)
                {
                    weapon = new Weapon(i);
                    WeaponController weaponController = weapons[i];
                    print(weapon.weaponName + "-" + weapon.magazineCapacity);

                }
            }
            WeaponController.ToFire(cam, weapon.damage, weapon.magazineCapacity);
            timingFiring = 0f;
        }
        else
        {
            timingFiring += Time.deltaTime;
        }
    }



}
