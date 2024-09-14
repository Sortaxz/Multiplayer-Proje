using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CharacterAnimation : PlayerAnimation
{
    private Animator animator;
    private PhotonView pw;
    private void Awake() 
    {
        animator = GetComponent<Animator>();    
        pw = GetComponent<PhotonView>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {


        if(!pw.IsMine)
            return;

        PlayPlayerMoveAnimation("isWalking", isWalking, "bool");
        PlayPlayerMoveAnimation("isRightWalking", isRightWalking, "bool");
        PlayPlayerMoveAnimation("isLeftWalking", isLeftWalking, "bool");
        PlayPlayerMoveAnimation("isRunning", isRunning, "bool", forward);
        PlayPlayerMoveAnimation("isWalkingBack", backward, "bool");
        PlayPlayerMoveAnimation("isJumping", isJumping, "bool");
        PlayPlayerCrounchMoveAnimation();

    }

    private void PlayPlayerCrounchMoveAnimation()
    {
        if (isCrounchIdle)
        {
            animator.SetFloat("crounchSpeed", .2f);
        }
        if (isCrounchLeft)
        {
            animator.SetFloat("crounchSpeed", .25f);
        }
        if (isCrounchRight)
        {
            animator.SetFloat("crounchSpeed", .5f);
        }
        if (isCrouncWalkForward)
        {
            animator.SetFloat("crounchSpeed", .75f);
        }
        if (isCrounchWalkBackward)
        {
            animator.SetFloat("crounchSpeed", 1f);
        }
        if (!ctrl)
        {
            animator.SetFloat("crounchSpeed", 0f);
        }
    }

    private void SetAnimation(string animationBoolName,object animationBoolValue,string saveAnimationType)
    {
        switch (saveAnimationType)
        {
            case "bool":
                animator.SetBool(animationBoolName,(bool)animationBoolValue);
            break;
            case "float":
                animator.SetFloat(animationBoolName,(float)animationBoolValue);
            break;
            case "int":
                animator.SetInteger(animationBoolName,(int)animationBoolValue);
            break;
            default:
            break;
        }
    }

    public void PlayPlayerMoveAnimation(string animationBoolName,bool animationDirection,string setValueType, bool  additionalAnimationDirection =false)
    {
        if(animationBoolName == "isRunning" )
        {
            if(animationDirection && additionalAnimationDirection || additionalAnimationDirection && animationDirection)
            {
                SetAnimation(animationBoolName,animationDirection,setValueType);
            }
            else
            {
                SetAnimation(animationBoolName,animationDirection,setValueType);
            }
        }
        else if(animationBoolName != "isRunning")
        {
            if(animationDirection)
            {
                SetAnimation(animationBoolName,animationDirection,setValueType);
            }
            else
            {
                SetAnimation(animationBoolName,animationDirection,setValueType);
            }
        }
        
    }
}
