using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    private Transform hammerHolder;
    private float damage;
    private float swingSpeed;
    private Transform swingPoint;

    [SerializeField]
    GameObject onHitFx;

    private float startRotate;
    private float endRotate;
    private float currentRotation;
    private float degreeToRotate;
    private bool rotateClockwise;
    private float elapsedTime;

    private void Awake()
    {
        hammerHolder = transform.parent;
        elapsedTime = 0.0f;
    }

    private void Update()
    {

        Rotate();
    }

    private void Rotate()
    {
        if (elapsedTime > degreeToRotate)
        {
            Destroy(hammerHolder.gameObject);
        }

        float step = swingSpeed * Time.deltaTime;
        elapsedTime += step;

        float zOffset = rotateClockwise ? -step : step;

        currentRotation += zOffset;

        hammerHolder.transform.rotation = Quaternion.Euler(
            hammerHolder.transform.rotation.x,
            hammerHolder.transform.rotation.y,
            currentRotation
        );
    }

    public void Init(
        float damage,
        float endRotate,
        float swingSpeed,
        bool strike = false,
        Transform swingPoint = null
    )
    {
        this.damage = damage;
        this.endRotate = endRotate;
        this.swingSpeed = swingSpeed;
        this.swingPoint = swingPoint;
        elapsedTime = 0f;
    }

    public void Init(
        float damage,
        float degreeToRotate,
        float currentRotation,
        float rotateSpeed,
        bool clockwise
    )
    {
        this.damage = damage;
        this.degreeToRotate = degreeToRotate;
        this.currentRotation = currentRotation;
        swingSpeed = rotateSpeed;
        rotateClockwise = clockwise;
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
            if (enemy is WalkingEnemy)
            {
                WalkingEnemy we = (WalkingEnemy)enemy;
                we.KnockBack();
            }

            enemy.Hurt(damage, onHitFx, transform.position);
        }
    }
}
