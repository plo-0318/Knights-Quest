using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private float damage;
    private float swingSpeed;
    private Transform swingPoint;

    [SerializeField]
    GameObject onHitFx;

    private float startRotate;
    private float endRotate;
    private float elapsedTime = 0.0f;

    private void Update()
    {
        UpdateParentPosition();

        //Create the swinging
        if (elapsedTime < endRotate && swingPoint != null)
        {
            elapsedTime += swingSpeed * Time.deltaTime;
            float step = swingSpeed * Time.deltaTime;
            transform.RotateAround(swingPoint.position, Vector3.forward, step);
        }
        else if (elapsedTime >= endRotate)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void Init(float damage, float endRotate, float swingSpeed, bool strike = false, Transform swingPoint = null)
    {
        this.damage = damage;
        this.endRotate = endRotate;
        this.swingSpeed = swingSpeed;
        this.swingPoint = swingPoint;
        elapsedTime = 0f;
    }

    //Check the parent position
    private void UpdateParentPosition()
    {
        if (swingPoint != null)
        {
            transform.parent.position = swingPoint.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Hurt(damage, onHitFx, transform.position);
        }
    }
}