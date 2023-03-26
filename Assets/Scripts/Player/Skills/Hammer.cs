using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private float damage;
    private Transform swingPoint;

    [SerializeField]
    private Rigidbody2D rb;

    private float startRotate;
    private float endRotate;

    private float totalRotateTime = 1.0f;
    private float elapsedTime = 0.0f;

    private void Start()
    {
        startRotate = 0f;
    }

    private void Update()
    {
        if (elapsedTime < totalRotateTime && swingPoint != null)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / totalRotateTime;
            float step = (endRotate - startRotate) * Time.deltaTime / totalRotateTime;
            transform.RotateAround(swingPoint.position, Vector3.forward, step);
        }
        else if (elapsedTime >= totalRotateTime)
        {
            Destroy(gameObject);
        }
    }

    public void Init(float damage, float endRotate, bool strike = false, Transform swingPoint = null)
    {
        this.damage = damage;
        this.endRotate = endRotate;
        this.swingPoint = swingPoint;
        elapsedTime = 0f;
    }
}