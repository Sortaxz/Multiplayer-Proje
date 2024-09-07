using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CharacterControl : InputManager
{
    private PhotonView pw;
    private Rigidbody rb;
    [SerializeField] private GameObject characterCamera;
    [SerializeField] private float mouseSensitivity, sprintSpeed , walkSpeed, jumpForce , smothTime;
    private Vector3 smoothMoveVelocity;
    private Vector3 moveAmount;
    private bool playerStop = false;
    private static bool isGround = false;
    
    private static bool isPlayerJump = false;
    private float verticalLookRotation;

    public static bool IsPlayerJump { get { return isPlayerJump;} set { isPlayerJump = value; } }
    private void Awake() 
    {
        pw = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();

        if(!pw.IsMine)
        {
            Destroy(characterCamera.gameObject);
        }
    }
    
    private void Update()
    {
        if(pw.IsMine)
        {
            Look();
            Move();
        }

        

    }
    
    private void FixedUpdate()
    {
        if(pw.IsMine)
        {
            PlayerMovement();

            rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);

        }
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
}
