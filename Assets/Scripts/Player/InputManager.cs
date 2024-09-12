using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public  class InputManager : MonoBehaviourPunCallbacks
{
    protected bool jump => Input.GetKey(KeyCode.Space);
    protected bool forward => Input.GetKey(KeyCode.W);
    protected bool left => Input.GetKey(KeyCode.A);
    protected bool right => Input.GetKey(KeyCode.D);
    protected bool backward => Input.GetKey(KeyCode.S);
    protected bool runing => Input.GetKey(KeyCode.LeftShift);
    protected float Horizontal => Input.GetAxis("Horizontal") ;
    protected float Vertical => Input.GetAxis("Vertical") ;
    protected float mouseScrollWhell => Input.GetAxisRaw("Mouse ScrollWheel");
    protected bool mosueLeftKey => Input.GetMouseButtonDown(0);
}

public class PlayerAnimation : InputManager
{
    protected bool isWalking => forward ? true : false;
    protected bool isRightWalking => left ? true : false;
    protected bool isLeftWalking => right ? true : false; 
    protected bool isRunning => runing ? true : false;
    protected bool isJumping => CharacterControl.IsPlayerJump ? true : false;
}
