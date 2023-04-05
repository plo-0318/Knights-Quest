using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyAnimatorController : AnimatorController
{
    [SerializeField]
    protected string attack;

    protected bool isAttacking;
    protected float attackAnimationLength;

    protected Coroutine waitForAttackAnimation;

    protected virtual void Awake()
    {
        isAttacking = false;
    }

    protected override void Start()
    {
        base.Start();

        if (attack.Length != 0)
        {
            AnimationClip attackClip = null;

            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == attack)
                {
                    attackClip = clip;
                }
            }

            if (attackClip != null)
            {
                attackAnimationLength = attackClip.length;
            }
        }
    }

    protected override void FixedUpdate()
    {
        if (animatedObj.IsDead())
        {
            ChangeAnimationState(death);
            return;
        }

        if (isAttacking)
        {
            ChangeAnimationState(attack);
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

    public virtual void HandlePlayerAttack()
    {
        isAttacking = true;

        if (waitForAttackAnimation != null)
        {
            StopCoroutine(waitForAttackAnimation);
        }

        waitForAttackAnimation = StartCoroutine(WaitForAttackAnimation());
    }

    protected IEnumerator WaitForAttackAnimation()
    {
        yield return new WaitForSeconds(attackAnimationLength);

        isAttacking = false;
    }
}
