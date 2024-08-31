using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : InputManager
{
    Rigidbody rb;
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private GameObject playerRightHand;
    public GameObject PlayerRightHand { get { return gunPrefab;}}
    private GameObject playerGun;
    
    private bool playerStop = false;
    private static bool isGround = false;
    
    private static bool isPlayerJump = false;
    private float stopDamping = 5f;

    public static bool IsPlayerJump { get { return isPlayerJump;} set { isPlayerJump = value; } }
    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }
    

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (forward)
        {
            rb.AddForce(Vector3.forward * 9,ForceMode.Force);
            playerStop = true;
        }

        if(runing)
        {
            rb.AddForce(Vector3.forward * 14,ForceMode.Force);
            playerStop = true;
        }

        if (!forward && !backward && !left && !right && playerStop)
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, stopDamping * Time.deltaTime);
            
            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector3.zero;
                playerStop = false;
            }
        }

        if (backward)
        {
            rb.AddForce(Vector3.back * 9,ForceMode.Force);
            playerStop = true;

        }


        if (left)
        {
            rb.AddForce(Vector3.left * 9 ,ForceMode.Force);
            playerStop = true;
        }


        if (right)
        {
            rb.AddForce(Vector3.right * 9,ForceMode.Force);
            playerStop = true;
        }

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
