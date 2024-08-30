using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : PlayerAnimation
{
    private Animator animator;
    private void Awake() 
    {
        animator = GetComponent<Animator>();    
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        

        PlayPlayerMoveAnimation("isWalking",forward,"bool");
        PlayPlayerMoveAnimation("isRightWalking",right,"bool");
        PlayPlayerMoveAnimation("isLeftWalking",left,"bool");
        PlayPlayerMoveAnimation("isRunning",runing,"bool",forward);
        PlayPlayerMoveAnimation("isWalkingBack",backward,"bool");
        PlayPlayerMoveAnimation("isJumping",isJumping,"bool");
            

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
