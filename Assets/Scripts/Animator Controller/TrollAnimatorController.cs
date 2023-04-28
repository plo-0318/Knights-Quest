using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatableTroll : IAnimatable
{
    bool IsCharging();
    bool IsAttacking();
}

public class TrollAnimatorController : AnimatorController
{
    protected IAnimatableTroll troll;

    [SerializeField]
    protected string charge;

    [SerializeField]
    protected string attack;

    [System.NonSerialized]
    public float attackAnimationLength;

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

        troll = (IAnimatableTroll)animatedObj;
    }

    protected override void FixedUpdate()
    {
        if (troll.IsDead())
        {
            ChangeAnimationState(death);
            return;
        }

        if (attack.Length != 0 && troll.IsAttacking())
        {
            ChangeAnimationState(attack);
            return;
        }

        if (charge.Length != 0 && troll.IsCharging())
        {
            ChangeAnimationState(charge);
            return;
        }

        if (troll.IsIdle() && idle.Trim() != "")
        {
            ChangeAnimationState(idle);
        }
        else
        {
            ChangeAnimationState(move);
        }
    }
}
