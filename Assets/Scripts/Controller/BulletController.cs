using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody bulletRb;
    private Transform bulletParent;
    private Vector3 bulletPosition ;
    private Vector3 bulletRotation;
    private float bulletDamge;
    public float BulletDamage { get {return bulletDamge;}set {bulletDamge = value;} }
    private int bulletGetSiblingIndex=-1;
    private void Awake() 
    {
        bulletRb = GetComponent<Rigidbody>();    
        bulletParent = transform.parent;
        bulletGetSiblingIndex = transform.GetSiblingIndex();
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

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            transform.position = Vector3.zero;
    
            gameObject.SetActive(false);
            transform.SetParent(bulletParent);

            
            transform.SetSiblingIndex(bulletGetSiblingIndex);
            other.GetComponent<IDamageable>()?.TakeDamage(bulletDamge);
        }   
    }

   

    private IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(10f);
        
        transform.localPosition = Vector3.zero;

        transform.SetParent(bulletParent);
        gameObject.SetActive(false);


        transform.SetSiblingIndex(bulletGetSiblingIndex);
    }
    
    public void SetBulletTransformRotation(Vector3 position)
    {
        transform.localPosition = Vector3.zero;
    }
}
