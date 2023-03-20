using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestroyOnAnimationComplete : MonoBehaviour
{
    public float delay;

    private void Start()
    {
        float animationDuration = GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;

        Destroy(gameObject, animationDuration + delay);
    }
}
