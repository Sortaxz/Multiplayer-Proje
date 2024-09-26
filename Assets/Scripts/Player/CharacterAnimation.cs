using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CharacterAnimation : PlayerAnimation
{
    private Animator animator;
    private PhotonView pw;
    private bool playedDeathAnimation = false;
    private PhotonAnimatorView photonAnimator;
    private bool hitReactionAnimation;
    public bool HitReactionAnimation {get {return hitReactionAnimation;} set {hitReactionAnimation = value;} }
    public bool reloading = false;
    public bool fire = false;
    private bool jumpAnimation = false;
    public bool  JumpAnimation {get {return jumpAnimation;} set {jumpAnimation = value;} }
    private bool hitAnimation = false;
    public bool HitAnimation {get {return hitAnimation;} set {hitAnimation = value;}}
    private void Awake() 
    {
        animator = GetComponent<Animator>();    
        pw = GetComponent<PhotonView>();
        GameManager.deatDelegate += Deat_Method;
        photonAnimator = GetComponent<PhotonAnimatorView>();
    }

    void Start()
    {
        //GameManager.animationDelegate += HitAnimation;

    }
    
    void Update()
    {
        
        if(!GameManager.Instance.CharacterDead)
        {
            if (pw.IsMine)
            {
                CharacterMovementAnimation();

                if(isFire)
                {
                    animator.Play("Firing_Rifle");
                }


                if(reloading )
                {
                    animator.Play("Reloading");
                }
               
                
                
                if(isJumping && !jumpAnimation)
                {
                    jumpAnimation = true;
                }
                
                if(hitAnimation)
                {
                    animator.Play("Hit Reaction");
                    hitAnimation = false;
                }

            }   
        }    

    }

    public void HitReactionAnimation_Method(bool value,Animator animator)
    {
        print(transform.name);
            animator.SetBool("isHitReaction",true);

    }
    private void CharacterMovementAnimation()
    {
        if (forward || left || right || backward || leftShift)
        {
            animator.SetBool("isMovement", true);
            if (isWalking)
            {
                animator.SetFloat("speed", .26f);
                animator.SetFloat("forward", .26f);
            }
            if (isWalkForwardLeft)
            {
                animator.SetFloat("speed", .26f);
                animator.SetFloat("forward", .56f);
            }
            if (isWalkForwardRight)
            {
                animator.SetFloat("speed", .26f);
                animator.SetFloat("forward", .76f);
            }
            if (isRunning)
            {
                animator.SetFloat("speed", .26f);
                animator.SetFloat("forward", 1f);
            }

            if (isLeftWalking && !forward)
            {
                animator.SetFloat("speed", .51f);
                animator.SetFloat("left", .22f);
            }
            if (isLeftRun)
            {
                animator.SetFloat("speed", .51f);
                animator.SetFloat("left", .35f);
            }
            if (isLeftForwardRun)
            {
                animator.SetFloat("speed", .51f);
                animator.SetFloat("left", .68f);
            }
            if (isLeftBackwardRun)
            {
                animator.SetFloat("speed", .51f);
                animator.SetFloat("left", 1f);
            }

            if (isLeftWalking && !forward)
            {
                animator.SetFloat("speed", .51f);
                animator.SetFloat("left", .22f);
            }
            if (isLeftRun)
            {
                animator.SetFloat("speed", .51f);
                animator.SetFloat("left", .35f);
            }
            if (isLeftForwardRun)
            {
                animator.SetFloat("speed", .51f);
                animator.SetFloat("left", .68f);
            }
            if (isLeftBackwardRun)
            {
                animator.SetFloat("speed", .51f);
                animator.SetFloat("left", 1f);
            }

            if (isRightWalking && !forward)
            {
                animator.SetFloat("speed", .76f);
                animator.SetFloat("right", .22f);
            }
            if (isRightRun)
            {
                animator.SetFloat("speed", .76f);
                animator.SetFloat("right", .48f);
            }
            if (isRightForwardRun)
            {
                animator.SetFloat("speed", .76f);
                animator.SetFloat("right", .75f);
            }
            if (isRightBacwardRun)
            {
                animator.SetFloat("speed", .76f);
                animator.SetFloat("right", 1f);
            }

            if (isBackwardWalk)
            {
                animator.SetFloat("speed", 1f);
                animator.SetFloat("backward", .22f);
            }
            if (isWalkBackwardLeft)
            {
                animator.SetFloat("speed", 1f);
                animator.SetFloat("backward", .48f);
            }
            if (isWalkBacwardRight)
            {
                animator.SetFloat("speed", 1f);
                animator.SetFloat("backward", .75f);
            }
            if (isBackwardRun)
            {
                animator.SetFloat("speed", 1f);
                animator.SetFloat("backward", 1f);
            }

        }
        if (!forward && !left && !right && !backward && !leftShift)
        {
            animator.SetBool("isMovement", false);
            animator.SetFloat("speed", 0f);
        }
        if (!ctrl)
        {
            PlayPlayerAnimation("isJumping", jumpAnimation, "bool");
        }

        PlayPlayerCrounchMoveAnimation();
    }


    private void PlayPlayerCrounchMoveAnimation()
    {
        if(ctrl)
        {
            animator.SetBool("isCrounch",true);

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
        }
        if (!ctrl)
        {
            animator.SetBool("isCrounch",false);

            animator.SetFloat("crounchSpeed", 0f);
        }
    }


    public void SetAnimation(string animationBoolName,object animationBoolValue,string saveAnimationType)
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

    public void PlayPlayerAnimation(string animationBoolName,bool animationDirection,string setValueType, bool  additionalAnimationDirection =false)
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

    public void Deat_Method()
    {
        if(animator != null)
        {
            animator.SetBool("isDeath",true); 
        }
    }

    

}
