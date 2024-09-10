using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable =ExitGames.Client.Photon.Hashtable;

public class CharacterControl : InputManager,IDamageable
{
    private PhotonView pw;
    private Rigidbody rb;

    #region  Character Move 
    [SerializeField] private GameObject characterCamera;
    [SerializeField] private float mouseSensitivity, sprintSpeed , walkSpeed, jumpForce , smothTime;
    private Vector3 smoothMoveVelocity;
    private Vector3 moveAmount;
    private bool playerStop = false;
    private static bool isGround = false;
    
    private static bool isPlayerJump = false;
    private float verticalLookRotation;

    public static bool IsPlayerJump { get { return isPlayerJump;} set { isPlayerJump = value; } }

    #endregion

    [SerializeField] private Item[] gunItems;


    private int gunItemIndex;
    private int previousGunItemIndex = -1;


    private string playerNickName;
    public string PlayerNickName { get { return playerNickName;}}

    private int playerActorNumber;
    public  int PlayerActorNumber { get { return playerActorNumber;}}

    const float maxHealt= 100f;
    float currentHealt  = maxHealt;

    GameManager gameManager;

    private void Awake() 
    {
        currentHealt = maxHealt;

        pw = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        gameManager = PhotonView.Find((int)pw.InstantiationData[0]).GetComponent<GameManager>();

        playerNickName = pw.Owner.NickName;
        playerActorNumber = pw.Owner.ActorNumber;
        transform.name = playerNickName;
        
        if(pw.IsMine)
        {
            EquipGunItem(0);
        }
        else
        {
            Destroy(characterCamera.gameObject);

        }
    }
    
    private void Update()
    {
        if(!pw.IsMine)
            return;

        Look();
        Move();
        
        for (int i = 0; i < gunItems.Length; i++)
        {
            if(Input.GetKeyDown((i+1).ToString()))
            {
                EquipGunItem(i);
                break;
            }
        }

        if(Input.GetAxisRaw("Mouse ScrollWheel")>0f)
        {
            if(gunItemIndex >= gunItems.Length -1)
            {
                EquipGunItem(0);
            }
            else
            {
                EquipGunItem(gunItemIndex + 1);
            }
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if(gunItemIndex <= 0)
            {
                EquipGunItem(gunItems.Length-1);
            }
            else
            {
                EquipGunItem(gunItemIndex - 1);
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            gunItems[gunItemIndex].Use();
        }

    }
    
    private void FixedUpdate()
    {
        if(!pw.IsMine)
            return;
    
        PlayerMovement();
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Horizontal,0,Vertical).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ?  sprintSpeed : walkSpeed) , ref smoothMoveVelocity,smothTime);
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -20f, 20f);

        characterCamera.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }


    private void PlayerMovement()
    {

        if(jump && !isGround)
        {
            rb.AddForce(Vector3.up * 300);
            isGround = true;
            isPlayerJump = true;
        }
        else if(!jump && isGround)
        {
            isPlayerJump = false;
        }
       
        
    }

    private void OnCollisionEnter(Collision other) 
    {
        isGround = false;    
    }

    private void EquipGunItem(int gunItemIndex)
    {
        if(gunItemIndex == previousGunItemIndex)
            return;

        this.gunItemIndex = gunItemIndex;

        gunItems[gunItemIndex].GunItemGameObject.SetActive(true);

        if(previousGunItemIndex != -1)
        {
            gunItems[previousGunItemIndex].GunItemGameObject.SetActive(false);
        }

        previousGunItemIndex = gunItemIndex;

        if(pw.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("gunItemIndex",gunItemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!pw.IsMine && targetPlayer == pw.Owner)
        {
            EquipGunItem((int)changedProps["gunItemIndex"]);
        }
    }

    public void TakeDamage(float damage)
    {
        pw.RPC("RPC_TakeDamage",RpcTarget.All,damage);
    }

    [PunRPC]
    private void RPC_TakeDamage(float damage)
    {
        if(!pw.IsMine)
            return;

        currentHealt -= damage;

        if(currentHealt < 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameManager.Die();
    }
}
