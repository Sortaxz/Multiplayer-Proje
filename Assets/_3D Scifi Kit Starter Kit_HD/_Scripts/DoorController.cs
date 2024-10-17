using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private OpenTriggerControl OpenTrigger;
    private bool doorOpen = false;
    public bool DoorOpenClose { get { return doorOpen; } set { doorOpen = value; } }

    private bool leaveDoorOpen = false;
    public bool LeaveDoorOpen { get { return leaveDoorOpen;} set { leaveDoorOpen = value; } }
    
    private bool doorClose = false;
    public bool DoorClose { get { return doorClose; } set { doorClose = value; } }

    private Vector3 doorFirstPosition;
    private float doorTargetPositionX;
    private float doorTargetPositionZ;

    private string slammedDoor;
    public string SlammedDoor {get { return slammedDoor;} set { slammedDoor = value;}}


    [SerializeField] private bool positionX,positionY,positionZ,negative,positive;
    private void OnEnable() 
    {
        OpenTrigger = GetComponentInChildren<OpenTriggerControl>();    
    }

    void Start()
    {
        DoorTargetDistance(true);
    }
    //it allows us to find the distance the door will go.
    private void DoorTargetDistance(bool firstValue ,string doorMoveDirection = "",string doorNormalDirection = "")
    {
        if(firstValue)
        {
            if (positionX)
            {
                if (negative)
                {
                    doorTargetPositionX = transform.position.x - 5;
                }
                else
                {

                    doorTargetPositionX = transform.position.x + 5;
                }
            }
            else if (positionZ)
            {
                if (negative)
                {
                    doorTargetPositionZ = transform.position.z - 5;
                }
                else
                {
                    doorTargetPositionZ = transform.position.z + 5;
                }
            }
        }
    }

    void Update()
    {

    }

    public void DoorOpen()
    {
        StartCoroutine(DoorOpenCloseControl());
    }

    //This function is control  open or close of door 
    private IEnumerator DoorOpenCloseControl()
    {
        while (!doorClose)
        {
            if(positionX)
            {
                if(negative)
                {
                    if(transform.position.x > doorTargetPositionX)
                    {
                        transform.position = new Vector3(transform.position.x - .4f * Time.deltaTime,transform.position.y,transform.position.z );
                        if(transform.position.x < doorTargetPositionX)
                        {
                            doorTargetPositionX =transform.position.x + 5;
                        }
                    }
                    else if(transform.position.x < doorTargetPositionX)
                    {
                        if(!leaveDoorOpen)
                        {
                            transform.position = new Vector3(transform.position.x + .4f * Time.deltaTime, transform.position.y, transform.position.z );
                            if(transform.position.x >= doorTargetPositionX)
                            {
                                doorTargetPositionX = transform.position.x - 5;
                                doorClose = true;
                                
                            }
                        }
                    }
                    
                }
                else
                {
                    if(transform.position.x < doorTargetPositionX)
                    {
                        transform.position = new Vector3(transform.position.x + .4f * Time.deltaTime,transform.position.y,transform.position.z);
                        if(transform.position.x > doorTargetPositionX)
                        {
                            doorTargetPositionX = transform.position.x - 5;
                            DoorTargetDistance(false,"Forward","X");
                        }
                    }
                    else if(transform.position.x > doorTargetPositionX)
                    {
                        if(!leaveDoorOpen)
                        {
                            transform.position = new Vector3(transform.position.x - .4f * Time.deltaTime,transform.position.y,transform.position.z);
                            if(transform.position.x <= doorTargetPositionX)
                            {
                                doorTargetPositionX = transform.position.x + 5;
                                doorClose = true;
                            }
                        }
                    }
                }
            }
            else if(positionZ)
            {
                if(negative)
                {
                    if(transform.position.z > doorTargetPositionZ)
                    {
                        transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z - .4f * Time.deltaTime);
                        if(transform.position.z < doorTargetPositionZ)
                        {
                            doorTargetPositionZ = transform.position.z + 5;
                        }
                    }
                    else if(transform.position.z < doorTargetPositionZ)
                    {
                        if(!leaveDoorOpen)
                        {
                            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + .4f * Time.deltaTime);
                            if(transform.position.z >= doorTargetPositionZ)
                            {
                                doorTargetPositionZ = transform.position.z - 5;
                                doorClose = true;
                            }
                        }
                    }
                    
                   
                }
                else
                {
                    if(transform.position.z < doorTargetPositionZ)
                    {
                        transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z + .4f * Time.deltaTime);
                        if(transform.position.z > doorTargetPositionZ)
                        {
                            doorTargetPositionZ = transform.position.z - 5;
                        }
                    }
                    else if(transform.position.z > doorTargetPositionZ)
                    {
                        if(!leaveDoorOpen)
                        {
                            transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z - .4f * Time.deltaTime);
                            if(transform.position.z <= doorTargetPositionZ)
                            {
                                doorTargetPositionZ = transform.position.z + 5;
                                doorClose = true;
                            }
                        }
                    }
                    
                }
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            print(transform.name);
        }    
    }
}
