using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody bulletRb;
    private Transform bulletParent;
    public Vector3 bulletPosition ;
    private float bulletDamge;
    public float BulletDamage { get {return bulletDamge;}set {bulletDamge = value;} }
    private int bulletGetSiblingIndex=-1;
    private void Awake() 
    {
        bulletRb = GetComponent<Rigidbody>();    
        bulletGetSiblingIndex = transform.GetSiblingIndex();
        bulletParent = transform.parent;
    }
    
    void Start()
    {
        StartCoroutine(BulletDestroy(10));
    }
    
    public void BulletMove(Vector3 direction,Vector3 target,float damage)
    {
        if(target != null)
        {
            StartCoroutine(Move(target));
        }
        else
        {
            bulletRb.AddForce(direction * 1000);
        }
        bulletDamge = damage;
    }
    private IEnumerator Move(Vector3 target)
    {
        while(transform.position != target)
        {
            transform.position = Vector3.Lerp(transform.position,target, Time.deltaTime * 20f);
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            
            
            gameObject.SetActive(false);
            transform.SetParent(bulletParent);
            transform.position = bulletPosition;
            
            transform.SetSiblingIndex(bulletGetSiblingIndex);
            
            other.GetComponent<IDamageable>()?.TakeDamage(bulletDamge);
        
        }
        else
        {
            /*
            gameObject.SetActive(false);
            transform.SetParent(bulletParent);
            transform.position = bulletPosition;
            
            transform.SetSiblingIndex(bulletGetSiblingIndex);
            */
            BulletDestroy(1);
        }   
    }

    

    public IEnumerator BulletDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        

        gameObject.SetActive(false);
        transform.SetParent(bulletParent);
        transform.localPosition = bulletPosition;


        transform.SetSiblingIndex(bulletGetSiblingIndex);
    }
    
    
}
