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

    private void Start()
    {
        animator = GetComponent<Animator>();
        animatedObj = GetComponent<IAnimatable>();

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

    private void FixedUpdate()
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

    private void ChangeAnimationState(string newState)
    {
        if (newState == currentState)
            return;

        animator.Play(newState);

        currentState = newState;
    }
}
