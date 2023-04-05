using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class Shield : MonoBehaviour
{
    private Animator animator;

    [System.NonSerialized]
    public float breakClipLength;
    private const string BREAK_CLIP_NAME = "SkillShield_BubbleBreak";

    private int blocks;
    private bool isBreaking;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        breakClipLength = GetBreakClipLength();

        isBreaking = false;

        blocks = 1;
    }

    public void Init(bool blockTwice)
    {
        blocks = blockTwice ? 2 : 1;
    }

    public void Break()
    {
        if (isBreaking)
        {
            return;
        }

        if (blocks > 1)
        {
            spriteRenderer.color = new Color(183 / 255f, 29 / 255f, 27 / 255f);
            blocks--;

            return;
        }

        isBreaking = true;

        animator.Play(BREAK_CLIP_NAME);

        Destroy(gameObject, breakClipLength);
    }

    private float GetBreakClipLength()
    {
        AnimationClip breakClip = null;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == BREAK_CLIP_NAME)
            {
                breakClip = clip;
            }
        }

        return breakClip == null ? 0 : breakClip.length;
    }

    public bool IsBreaking => isBreaking;
}
