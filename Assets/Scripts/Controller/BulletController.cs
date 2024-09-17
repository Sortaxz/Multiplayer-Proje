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
        bulletRb.AddForce(Vector3.forward * 100);
    }

    void Update()
    {
        
    }
}
