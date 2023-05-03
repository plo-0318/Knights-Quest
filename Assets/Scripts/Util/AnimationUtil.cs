using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AnimationUtil
{
    private static ChargeIndicator chargeIndicatorPrefab;
    private static Indicator skillIndicatorPrefab;

    static AnimationUtil()
    {
        chargeIndicatorPrefab = Resources.Load<ChargeIndicator>("Misc/charge indicator");
        skillIndicatorPrefab = Resources.Load<Indicator>("Misc/skill indicator");
    }

    public static ChargeIndicator SpawnChargeIndicator(Transform parent, float angle)
    {
        ChargeIndicator indicator = GameObject.Instantiate(
            chargeIndicatorPrefab,
            parent.position,
            Quaternion.identity,
            parent
        );

        indicator.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        return indicator;
    }

    public static void SpawnSkillIndicator(Vector3 spawnPos, float duration, Action callback)
    {
        Indicator indicator = GameObject.Instantiate(
            skillIndicatorPrefab,
            spawnPos,
            Quaternion.identity
        );

        indicator.StartBlink(duration, callback);
    }

    public static IEnumerator DecreaseScaleOverTime(float duration, Transform trans)
    {
        float elapsedTime = 0f;
        float scale = 1f;
        float sign = Mathf.Sign(trans.localScale.x);

        // Decrease local scale from 1 to 0.8 for the first 2/5 of the duration
        while (elapsedTime < duration * 2 / 5)
        {
            // Calculate the new scale based on the elapsed time
            scale = Mathf.Lerp(1f, 0.8f, elapsedTime / (duration * 2 / 5));

            // Set the local scale of the transform
            trans.localScale = new Vector3(scale * sign, scale, scale);

            // Increment the elapsed time by delta time
            elapsedTime += Time.deltaTime;

            // Wait for the end of the frame before continuing the loop
            yield return null;
        }

        // Decrease local scale from 0.8 to 0 for the rest of the duration
        while (elapsedTime < duration)
        {
            // Calculate the new scale based on the elapsed time
            scale = Mathf.Lerp(0.8f, 0f, (elapsedTime - duration * 2 / 5) / (duration * 3 / 5));

            // Set the local scale of the transform
            trans.localScale = new Vector3(scale * sign, scale, scale);

            // Increment the elapsed time by delta time
            elapsedTime += Time.deltaTime;

            // Wait for the end of the frame before continuing the loop
            yield return null;
        }

        // Ensure that the final scale is exactly 0, since Lerp might not reach it
        trans.localScale = Vector3.zero;
    }

    public static IEnumerator FallOver(float duration, Transform trans)
    {
        float elapsed = 0f;

        float sign = Mathf.Sign(trans.localScale.x) * -1;
        Vector3 startPos = trans.position;

        Vector3 offset = new Vector3(0.15f * sign, 0.28f, 0f);
        Vector3 endPos = offset + trans.position;

        Quaternion startRotation = trans.localRotation;
        Quaternion endRotation = Quaternion.Euler(0f, 0f, 15f * sign * -1);

        while (elapsed < duration)
        {
            // calculate the current percentage of time elapsed
            float t = elapsed / duration;

            // move towards the position
            trans.position = Vector3.Lerp(startPos, endPos, t);

            // rotate towards the end rotation
            trans.localRotation = Quaternion.Lerp(startRotation, endRotation, t);

            // increment the elapsed time by the time since last frame
            elapsed += Time.deltaTime;

            // wait for the next frame
            yield return null;
        }

        // Set the position to the final position after the duration has elapsed
        trans.position = endPos;
        trans.localRotation = endRotation;
    }

    public static IEnumerator BlinkSprite(
        SpriteRenderer sprite,
        float duration,
        float oscillationSpeed = 6f
    )
    {
        // Blink
        float currentTime = 0;

        Color originalColor = new Color(
            sprite.color.r,
            sprite.color.g,
            sprite.color.b,
            sprite.color.a
        );

        while (currentTime < duration)
        {
            var alpha = Mathf.PingPong(Time.time * oscillationSpeed, originalColor.a);

            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            currentTime += Time.deltaTime;
            yield return null;
        }

        sprite.color = originalColor;
    }

    public static void Fade(
        Image image,
        float duration,
        float startAlpha,
        float endAlpha,
        bool scaledTime = true,
        Action callback = null
    )
    {
        image.StartCoroutine(
            HandleFadingImage(image, duration, startAlpha, endAlpha, scaledTime, callback)
        );
    }

    public static void Fade(
        SpriteRenderer spriteRenderer,
        float duration,
        float startAlpha,
        float endAlpha,
        MonoBehaviour caller,
        bool scaledTime = true,
        Action callback = null
    )
    {
        caller.StartCoroutine(
            HandleFadingSpriteRenderer(
                spriteRenderer,
                duration,
                startAlpha,
                endAlpha,
                scaledTime,
                callback
            )
        );
    }

    private static IEnumerator HandleFadingImage(
        Image image,
        float duration,
        float startAlpha,
        float endAlpha,
        bool scaled,
        Action callback
    )
    {
        float elapsedTime = 0;

        Color endColor = new Color(image.color.r, image.color.g, image.color.b, endAlpha);

        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);

            if (scaled)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                elapsedTime += Time.unscaledDeltaTime;
            }

            yield return null;
        }

        image.color = endColor;

        if (callback != null)
        {
            callback();
        }
    }

    private static IEnumerator HandleFadingSpriteRenderer(
        SpriteRenderer image,
        float duration,
        float startAlpha,
        float endAlpha,
        bool scaled,
        Action callback
    )
    {
        float elapsedTime = 0;

        Color endColor = new Color(image.color.r, image.color.g, image.color.b, endAlpha);

        while (elapsedTime < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            image.color = new Color(image.color.r, image.color.g, image.color.b, newAlpha);

            if (scaled)
            {
                elapsedTime += Time.deltaTime;
            }
            else
            {
                elapsedTime += Time.unscaledDeltaTime;
            }

            yield return null;
        }

        image.color = endColor;

        if (callback != null)
        {
            callback();
        }
    }
}
