using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballField : MonoBehaviour
{
    private CircleCollider2D col;

    private float damage;
    private float cooldownTime;
    private float cooldownTimer;
    private float aliveTime;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();

        cooldownTimer = 0;
        cooldownTime = 1f;

        aliveTime = 2.5f;
    }

    private void Update()
    {
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
        }

        cooldownTimer -= Time.deltaTime;
        aliveTime -= Time.deltaTime;
    }

    public void Init(float damage)
    {
        this.damage = damage;
    }

    private void DamageEnemies()
    {
        if (cooldownTimer > 0)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            (Vector2)transform.position + col.offset,
            col.radius * transform.localScale.x
        );

        foreach (Collider2D otherCol in colliders)
        {
            if (otherCol.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.Hurt(damage);
            }
        }

        cooldownTimer = cooldownTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            DamageEnemies();
        }
    }
}
