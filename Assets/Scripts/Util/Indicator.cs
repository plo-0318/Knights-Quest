using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Indicator : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer circle,
        icon,
        background;

    private float blinkDuration;
    private float baseOsciallationSpeed = 1f;

    private IEnumerator BlinkAndExec(Action callback)
    {
        StartCoroutine(
            AnimationUtil.BlinkSprite(
                circle,
                blinkDuration,
                baseOsciallationSpeed * (circle.color.a / background.color.a)
            )
        );
        StartCoroutine(
            AnimationUtil.BlinkSprite(
                icon,
                blinkDuration,
                baseOsciallationSpeed * (icon.color.a / background.color.a)
            )
        );
        yield return StartCoroutine(
            AnimationUtil.BlinkSprite(background, blinkDuration, baseOsciallationSpeed)
        );

        callback();

        Destroy(gameObject);
    }

    public void StartBlink(float duration, Action callback)
    {
        blinkDuration = duration;
        StartCoroutine(BlinkAndExec(callback));
    }
}
