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

    public float swingSpeed = 1.0f;
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

    public void Init(float damage, float endRotate, float speed, bool strike = false, Transform swingPoint = null)
    {
        this.damage = damage;
        this.endRotate = endRotate;
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
}