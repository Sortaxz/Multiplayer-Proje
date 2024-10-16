using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : CameraInputManager
{
    public GameObject character;
    [SerializeField] private GameObject courseImage;
    public float mouseSensitivity = 100f;
    
    [SerializeField] private Transform[] positions;
    private float xRotation = 0f;

    private bool isHitCamera = false;
    private string hitObjectName = "";

    private int camerPositionIndex = -1;
    void Start()
    {
        
    }

    void Update()
    {
        print(isHitCamera);
    }
    private void LateUpdate()
    {
        if(!GameManager.Instance.CharacterDead)
        {
            if(!GameManager.Instance.GameStopted)
            {
                CameraMove();
                AdjustCameraPosition();
            }
        }
    }

    private void CameraMove()
    {
        // Mouse inputlarını al
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Kamerayı yukarı-aşağı döndürme (X ekseni)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -20, 10); // Yukarı ve aşağı bakış açısı sınırlandırılıyor

        // Kamerayı döndür
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Kamera objesinin etrafında karakteri döndür
        character.transform.Rotate(Vector3.up * mouseX);

        // Karakterin forward yönünü kameranın forward yönüne eşitle
        Vector3 cameraForward = new Vector3(transform.forward.x, 0, transform.forward.z); // Y ekseni (yukarı-aşağı) hariç tut
        character.transform.forward = cameraForward; // Karakteri kameranın forward yönüne hizala   
    }


    //adjusted position of camera
    private void AdjustCameraPosition()
    {
        if (fKey)
        {
            if(hitObjectName != "Wall")
            {
                if (transform.position == positions[1].position)
                {
                    transform.position = positions[0].position;
                    camerPositionIndex = 0;
                }
                else if (transform.position == positions[0].position)
                {
                    transform.position = positions[1].position;
                    camerPositionIndex = 1;

                }
            }
        }

    }   


    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Wall"))
        {
            hitObjectName = "Wall";
            
            CameraPositionChange();
        }   
        

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            hitObjectName = "Wall";
            
            CameraPositionChange();
        }
       
    }

    private void OnTriggerExit(Collider other) 
    {

        if(other.CompareTag("Wall"))
        {
            hitObjectName = "";
        }
    }

    // changes when the camera hits an object   
    private void CameraPositionChange()
    {
        if (transform.position == positions[1].position)
        {
            transform.position = positions[0].position;

        }

    }

}
