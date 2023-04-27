using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform holder;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private float endScale = 6f;

    [SerializeField]
    private float extendDuration = 1f;

    [SerializeField]
    private float blinkDuration = 1f;

    public void Play()
    {
        StartCoroutine(Extend());
    }

    public void SetDurations(float extendDuration, float blinkDuration)
    {
        this.extendDuration = extendDuration;
        this.blinkDuration = blinkDuration;
    }

    private IEnumerator Extend()
    {
        float elapsedTime = 0;

        Vector3 _endScale = new Vector3(endScale, transform.localScale.y, transform.localScale.z);

        while (elapsedTime < extendDuration)
        {
            float newScale = Mathf.Lerp(0, endScale, elapsedTime / extendDuration);

            holder.localScale = new Vector3(newScale, holder.localScale.y, holder.localScale.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        holder.localScale = _endScale;

        yield return StartCoroutine(AnimationUtil.BlinkSprite(sprite, blinkDuration, 3f));

        Destroy(gameObject);
    }

    public float TotalDuration => extendDuration + blinkDuration;
}
