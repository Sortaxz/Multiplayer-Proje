using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody bulletRb;
    private Transform bulletParent;
    private float bulletDamge;
    public float BulletDamage { get {return bulletDamge;}set {bulletDamge = value;} }
    private void Awake() 
    {
        bulletRb = GetComponent<Rigidbody>();    
        bulletParent = transform.parent;
    }
    
    void Start()
    {
        StartCoroutine(BulletDestroy());
    }

    public void BulletMove(Vector3 direction,float damage)
    {
        bulletRb.AddForce(direction * 1000);
        bulletDamge = damage;
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(other.collider.CompareTag("Player"))
        {
            print($"{other.collider.transform.name} oyuncusuna çarpti.");
            gameObject.SetActive(false);
            transform.SetParent(bulletParent);
            other.collider.GetComponent<IDamageable>()?.TakeDamage(bulletDamge);
        }   
    }

    private IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(10f);
        transform.SetParent(bulletParent);
        gameObject.SetActive(false);
    }
   
}
