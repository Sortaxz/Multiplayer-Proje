using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody bulletRb;
    private Transform bulletParent;
    public Vector3 bulletPosition ;
    private Vector3 bulletRotation;
    private float bulletDamge;
    public float BulletDamage { get {return bulletDamge;}set {bulletDamge = value;} }
    private int bulletGetSiblingIndex=-1;
    private void Awake() 
    {
        bulletRb = GetComponent<Rigidbody>();    
        bulletParent = transform.parent;
        print(bulletParent.transform.name);
        bulletGetSiblingIndex = transform.GetSiblingIndex();
    }
    
    void Start()
    {
        StartCoroutine(BulletDestroy());
    }
    
    public void BulletMove(Vector3 direction,Transform target,float damage)
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
    private IEnumerator Move(Transform target)
    {
        while(transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position,target.position, Time.deltaTime * 200);
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
    }

   

    public IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(10f);
        

        gameObject.SetActive(false);
        transform.SetParent(bulletParent);
        transform.localPosition = bulletPosition;


        transform.SetSiblingIndex(bulletGetSiblingIndex);
    }
    
    public void SetBulletTransformRotation()
    {
        transform.position = bulletPosition;
    }
}
