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
    private static bool isGround = false;
    
    private float verticalLookRotation;
    private static bool isPlayerJump = false;

    public static bool IsPlayerJump { get { return isPlayerJump;} set { isPlayerJump = value; } }

    #endregion

    [SerializeField] private SkinnedMeshRenderer characterMSHRenderer;
    public SkinnedMeshRenderer CharacterMSHRenderer {get { return characterMSHRenderer;}}
    [SerializeField] private Material characterMainMaterial;
    [SerializeField] private Image otherPlayerHealtBar;
    public Image OtherPlayerHealtBar { get {return otherPlayerHealtBar;} set { otherPlayerHealtBar = value;}}
    public Material CharacterMainMaterial { get { return characterMainMaterial;} set { characterMainMaterial = value; } }
    GameManager gameManager;
    [SerializeField] private CombatController combatController;
    [SerializeField] private CharacterAnimation characterAnimation;
    Hashtable playerProps;

    [SerializeField] private GameObject weapon_Info_Image;
    public GameObject Weapon_Info_Image { get {return weapon_Info_Image;} set {weapon_Info_Image = value;}}

    private string playerNickName;
    public string PlayerNickName { get { return playerNickName;}}

    private int playerActorNumber;
    public  int PlayerActorNumber { get { return playerActorNumber;}}

    private const float maxHealt= 100f;
    public float currentHealt  = maxHealt;
    private bool isLife = false;
    [SerializeField] private float jumpStrength;


    private void Awake() 
    {
        currentHealt = maxHealt;

        
        pw = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        gameManager = PhotonView.Find((int)pw.InstantiationData[0]).GetComponent<GameManager>();


        combatController = GetComponent<CombatController>();

        playerNickName = pw.Owner.NickName;
        playerActorNumber = pw.Owner.ActorNumber;
        transform.name = playerNickName;

        if(pw.IsMine)
        {
            transform.GetChild(0).GetComponent<CameraController>().character = gameObject;
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
        
        if(gameManager.IsCharacterDead())
        {
            Move();

        }
        
       
    }
    
    private void FixedUpdate()
    {
        if(!pw.IsMine)
            return;
        if(gameManager.IsCharacterDead())
        {
            PlayerMovement();

        }
       
        
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Horizontal,0,Vertical).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ?  sprintSpeed : walkSpeed) , ref smoothMoveVelocity,smothTime);
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
            rb.velocity = new Vector3(0,rb.velocity.y,0);
        }
        else if(ctrl && (!forward && !left && !right && !backward && !leftShift))
        {
            rb.velocity = new Vector3(0,rb.velocity.y,0);
        }
        else
        {
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
        characterAnimation.JumpAnimation = false;  
    }

   

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!pw.IsMine && targetPlayer == pw.Owner)
        {
            targetPlayer.CustomProperties.TryGetValue("gunItemIndex",out object itemIndex);
            if(itemIndex != null)
            {
                combatController.EquipGunItem((int)itemIndex);
            }

            targetPlayer.CustomProperties.TryGetValue("color",out object colorIndex);

            characterMSHRenderer.materials[1].color = gameManager.PlayerScriptableObject.PlayerColors[(int)colorIndex];

            
            
        }
        
        

        if(targetPlayer.CustomProperties.TryGetValue("life",out object life))
        {
            gameManager.FindCharacterOfPlayers = false;
            if((bool)life)
            {
                StartCoroutine(gameManager.FindCharacter());
            }
            else
            {
                StopCoroutine(gameManager.FindCharacter());
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
        playerProps["healt"] = currentHealt;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);
        pw.RPC("RPC_OtherPlayerHealtBar",RpcTarget.All,currentHealt);
        /*
        if (currentHealt < 0)
        {
            Die();
        }
        */
        if (currentHealt < 0)
        {
            currentHealt = 0;
            GameUI.Instance.PlayerHealtBar(1f);
            GameManager.deatDelegate();
        }
    }

    private void Die()
    {
        otherPlayerHealtBar.fillAmount = 1;
        isLife = false;
        playerProps["life"] = false;
        currentHealt = 100;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);


        gameManager.Die();
       
    }
    /*

    private void Die()
    {
        pw.RPC("RPC_Die",RpcTarget.All,null);
    }
    
    */
    [PunRPC]
    private void RPC_Die()
    {
        otherPlayerHealtBar.fillAmount = 1;
        isLife = false;
        playerProps["life"] = false;
        currentHealt = 100;
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProps);


        gameManager.Die();
    }

    [PunRPC]
    public void RPC_OtherPlayerHealtBar(float healt)
    {
        otherPlayerHealtBar.transform.parent.GetComponent<HealtBar_Control>().OtherHealtBar(healt);
    }

   
}
