using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody bulletRb;
    private Transform bulletParent;
    private Transform character;
    public Transform Character { get { return character; }  set { character = value; } }
    public Vector3 bulletPosition ;
    private float bulletDamge;
    public float BulletDamage { get {return bulletDamge;}set {bulletDamge = value;} }
    private int bulletGetSiblingIndex=-1;
    private void Awake() 
    {
        bulletRb = GetComponent<Rigidbody>();    
        bulletGetSiblingIndex = transform.GetSiblingIndex();
        bulletParent = transform.parent;
        print("ilk çikiş : " + transform.position);
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
            PhotonView otherPV =  other.GetComponent<PhotonView>();
            if(otherPV != null)
            {
                if(!otherPV.IsMine)
                {
                    gameObject.SetActive(false);
                    transform.SetParent(bulletParent);
                    transform.position = bulletPosition;
                    
                    transform.SetSiblingIndex(bulletGetSiblingIndex);
                    
                    other.GetComponent<IDamageable>()?.TakeDamage(bulletDamge,transform.position.z,other.name);
                }
            }
        
        }
       
    }

    private IEnumerator Follow(Transform healtBar)
    {
        while (transform.localRotation != character.rotation)
        {
            healtBar.localRotation = Quaternion.LookRotation(character.position);
            yield return null;
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
