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
    private bool follow;
    private Transform from,
        to;

    private void Awake()
    {
        follow = false;
        from = null;
        to = null;
    }

    private void FixedUpdate()
    {
        if (!follow)
        {
            return;
        }

        if (from == null || to == null)
        {
            return;
        }

        float angle = Util.GetNormalizedAngle(from.position, to.position);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Follow(Transform from, Transform to)
    {
        follow = true;

        this.from = from;
        this.to = to;
    }

    public void Play()
    {
        StartCoroutine(Extend());
    }

    public void SetDurations(float extendDuration, float blinkDuration)
    {
        this.extendDuration = extendDuration;
        this.blinkDuration = blinkDuration;
    }

    public void SetEndScale(float endScale)
    {
        this.endScale = endScale;
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
