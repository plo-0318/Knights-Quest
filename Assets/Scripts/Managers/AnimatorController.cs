using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatable
{
    bool IsIdle();
    bool IsMoving();
    bool IsDead();
}

public class AnimatorController : MonoBehaviour
{
    protected Animator animator;
    protected IAnimatable animatedObj;
    protected string currentState;

    [Header("Animation Names")]
    [SerializeField]
    protected string idle;

    [SerializeField]
    protected string move;

    [SerializeField]
    protected string death;

    [System.NonSerialized]
    public float deathAnimationLength;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        GetAnimatedObject();

        deathAnimationLength = 0;

        if (death.Length != 0)
        {
            AnimationClip deathClip = null;

            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == death)
                {
                    deathClip = clip;
                }
            }

            if (deathClip != null)
            {
                deathAnimationLength = deathClip.length;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (animatedObj.IsDead())
        {
            ChangeAnimationState(death);
            return;
        }

        if (animatedObj.IsIdle() && idle.Trim() != "")
        {
            ChangeAnimationState(idle);
        }
        else
        {
            ChangeAnimationState(move);
        }
    }

    protected void ChangeAnimationState(string newState)
    {
        if (newState == currentState)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    protected virtual void GetAnimatedObject()
    {
        if (TryGetComponent<IAnimatable>(out animatedObj))
        {
            return;
        }

        animatedObj = GetComponentInParent<IAnimatable>();
    }
}
