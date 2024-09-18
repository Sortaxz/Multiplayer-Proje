using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody bulletRb;
    private void Awake() 
    {
        bulletRb = GetComponent<Rigidbody>();    
    }
    
    void Start()
    {
    }

    void Update()
    {
        
    }

   
}
