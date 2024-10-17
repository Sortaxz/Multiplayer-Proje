using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenTriggerControl : MonoBehaviour
{
    [SerializeField] private BoxCollider Collider;
    [SerializeField] private DoorController doorController;

    private void OnEnable() 
    {
        Collider = GetComponent<BoxCollider>();    
        doorController = transform.parent.GetComponent<DoorController>();
    }


    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            print("çarpti");
            doorController.DoorClose = false;
            doorController.DoorOpen();
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            doorController.LeaveDoorOpen = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            doorController.LeaveDoorOpen = false;
        }
        
    }

}
