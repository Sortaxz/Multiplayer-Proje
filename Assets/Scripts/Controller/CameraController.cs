using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject character;
    public float mouseSensitivity = 100f;
    
    private float xRotation = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void LateUpdate() 
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
}
