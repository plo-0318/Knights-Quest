using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackingAnimatable : IAnimatable
{
    bool IsAttacking();
}

public class WalkingProjectileEnemyAnimatiorController : AnimatorController
{
    [SerializeField]
    protected string attack;

    protected float attackAnimationLength;

    protected override void Start()
    {
        base.Start();

        attackAnimationLength = 0;

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
        IAttackingAnimatable attackingAnimatedObj = (IAttackingAnimatable)animatedObj;

        if (animatedObj.IsDead())
        {
            ChangeAnimationState(death);
            return;
        }

        if (attackingAnimatedObj.IsAttacking())
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

    public float AttackAnimationLength => attackAnimationLength;
}
