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
    protected bool leftShift => Input.GetKey(KeyCode.LeftShift);
    protected bool ctrl => Input.GetKey(KeyCode.C);
    protected bool rewenal => Input.GetKeyDown(KeyCode.R);
    protected float Horizontal => Input.GetAxis("Horizontal") ;
    protected float Vertical => Input.GetAxis("Vertical") ;
    protected float mouseScrollWhell => Input.GetAxisRaw("Mouse ScrollWheel");
    protected bool mousePressedLeftButton => Input.GetMouseButtonDown(0);
    protected bool mousePressedLeftLeave => Input.GetMouseButtonUp(0);
}

public class PlayerAnimation : InputManager
{
    protected bool isWalking => forward ? true : false;
    protected bool isWalkForwardLeft => forward && left ? true : false;
    protected bool isWalkForwardRight => forward && right ? true : false;
    protected bool isRunning => forward && leftShift ? true : false;


    protected bool isBackwardWalk => backward ? true : false;
    protected bool isWalkBackwardLeft => backward && left ? true : false;
    protected bool isWalkBacwardRight => backward && right ? true : false;

    protected bool isBackwardRun => backward && leftShift ? true : false;

    protected bool isRightWalking => right ? true : false;
    protected bool isRightRun => right  && leftShift ? true : false;
    protected bool isRightForwardRun => right && forward && leftShift ? true : false;
    protected bool isRightBacwardRun => right && backward && leftShift ? true : false;
    
    protected bool isLeftWalking => left ? true : false; 
    protected bool isLeftRun => left && leftShift ? true : false;
    protected bool isLeftForwardRun => left && forward && leftShift ? true : false;
    protected bool isLeftBackwardRun => left && backward && leftShift ? true : false;

    protected bool isJumping => jump ? true : false;

    protected bool isCrounchIdle =>ctrl ? true : false;
    protected bool isCrounchLeft =>ctrl && left ? true : false;
    protected bool isCrounchRight =>ctrl && right ? true : false;
    protected bool isCrouncWalkForward => ctrl && forward ? true : false;
    protected bool isCrounchWalkBackward => ctrl && backward ? true : false;

    protected bool isReload => rewenal ? true : false;
    protected bool isFire => mousePressedLeftButton ? true : false;
}
