using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private Vector3 BASE_SCALE;
    private float damage;

    private void Start()
    {
        BASE_SCALE = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );
    }

    public void Init(float damage, float size)
    {
        this.damage = damage;
    }

    public void SetScale(float multiplier)
    {
        transform.localScale = BASE_SCALE * multiplier;
    }
}
