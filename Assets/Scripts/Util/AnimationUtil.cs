using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtil
{
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
}
