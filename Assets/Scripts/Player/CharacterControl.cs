using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
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
    
    private float verticalLookRotation;
    private static bool isPlayerJump = false;

    public static bool IsPlayerJump { get { return isPlayerJump;} set { isPlayerJump = value; } }

    #endregion

    [SerializeField] private Item[] gunItems;
    [SerializeField] private SkinnedMeshRenderer characterMSHRenderer;
    public SkinnedMeshRenderer CharacterMSHRenderer {get { return characterMSHRenderer;}}
    [SerializeField] private Material characterMainMaterial;
    [SerializeField] private Image otherPlayerHealtBar;
    public Image OtherPlayerHealtBar { get {return otherPlayerHealtBar;}}
    public Material CharacterMainMaterial { get { return characterMainMaterial;} set { characterMainMaterial = value; } }
    private int gunItemIndex;
    private int previousGunItemIndex = -1;


    private string playerNickName;
    public string PlayerNickName { get { return playerNickName;}}

    private int playerActorNumber;
    public  int PlayerActorNumber { get { return playerActorNumber;}}

    const float maxHealt= 100f;
    float currentHealt  = maxHealt;
    GameManager gameManager;
    Hashtable playerProps;
    private bool isLife = false;
    private bool resetSpeed = false;
    [SerializeField] private float jumpStrength;
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

        isLife = true;
        
        playerProps = new Hashtable();
        playerProps.Add("life",isLife);
        playerProps.Add("healt",currentHealt);
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        GameUI.Instance.OtherPlayerHealtBar = otherPlayerHealtBar.GetComponentInChildren<Image>();
    }

    

    private void Update()
    {
        if(!pw.IsMine)
            return;

        Look();
        Move();
        
        WeaponSelection();

        if(Input.GetKeyDown(KeyCode.P))
        {
            
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("healt",out object healt);
            print(healt);
        }
    }
    
    private void FixedUpdate()
    {
        if(!pw.IsMine)
            return;
    
        PlayerMovement();
       
        
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
        PlayerJumpMove();
        PlayerMoveForce();
    }

    private void PlayerMoveForce()
    {
        
        if (!forward && !left && !right && !backward && !leftShift && !ctrl)
        {
            print("hiz sifirlaniyor");
            rb.velocity = new Vector3(0,rb.velocity.y,0);
        }
        else if(ctrl && (!forward && !left && !right && !backward && !leftShift))
        {
            print("hiz sifirlaniyor2");
            rb.velocity = new Vector3(0,rb.velocity.y,0);
        }
        else
        {
            print("kuvvet uygulaniyor.");
            rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        }
    }

    private void PlayerJumpMove()
    {
        if(!forward && !right && !left && !ctrl && !leftShift)
        {
            if (jump && !isGround)
            {
                rb.AddForce(Vector3.up * jumpStrength);
                isGround = true;
                isPlayerJump = true;
            }
            else if (!jump && isGround)
            {
                isPlayerJump = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        isGround = false;    
    }

    private void WeaponSelection()
    {
        for (int i = 0; i < gunItems.Length; i++)
        {
            if(Input.GetKeyDown((i+1).ToString()))
            {
                EquipGunItem(i);
                break;
            }
        }

        if(mouseScrollWhell>0f)
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
        else if(mouseScrollWhell < 0f)
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

        if(mosueLeftKey)
        {
            gunItems[gunItemIndex].Use();
        }
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
           
            if(playerProps == null)
            {
                playerProps = new Hashtable();
                playerProps.Add("gunItemIndex",gunItemIndex);
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
            }
            else
            {
                playerProps["gunItemIndex"] = gunItemIndex;
                PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
            }

        }

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!pw.IsMine && targetPlayer == pw.Owner)
        {
            targetPlayer.CustomProperties.TryGetValue("gunItemIndex",out object itemIndex);
            EquipGunItem((int)itemIndex);

            targetPlayer.CustomProperties.TryGetValue("color",out object colorIndex);

            characterMSHRenderer.materials[1].color = gameManager.PlayerScriptableObject.PlayerColors[(int)colorIndex];

            targetPlayer.CustomProperties.TryGetValue("healt",out object healt);
            print(targetPlayer.UserId + "-" +healt);
        }

        if(targetPlayer.CustomProperties.TryGetValue("life",out object life))
        {
            gameManager.FindCharacterOfPlayers = false;
            if((bool)life)
            {
                StartCoroutine(gameManager.FindCharacter());
            }
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
        float deger = damage / 100; 
        currentHealt -= damage;
        GameUI.Instance.PlayerHealtBar(deger);
        otherPlayerHealtBar.fillAmount -= deger;

        playerProps["healt"] = currentHealt;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);

        if(currentHealt < 0)
        {
            Die();
            otherPlayerHealtBar.fillAmount = 1;
        }
    }

    private void Die()
    {
        isLife = false;
        playerProps["life"] = false;
        currentHealt = 100;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);


        gameManager.Die();
       
    }
}
