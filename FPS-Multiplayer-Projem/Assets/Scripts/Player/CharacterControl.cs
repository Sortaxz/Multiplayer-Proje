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

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }
    
    

    void Update()
    {
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (forward)
        {
            rb.AddForce(Vector3.forward * 10);
            playerStop = true;
        }


        if (!forward && !backward && !left && !right && playerStop)
        {
            rb.velocity = Vector3.zero;
            playerStop = false;
        }


        if (backward)
        {
            rb.AddForce(Vector3.back * 10);
            playerStop = true;

        }


        if (left)
        {
            rb.AddForce(Vector3.left * 10);
            playerStop = true;

        }


        if (right)
        {
            rb.AddForce(Vector3.right * 10);
            playerStop = true;
        }
    }
}
