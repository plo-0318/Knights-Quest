using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosion : MonoBehaviour
{
    private Collider2D col;
    private float damage;

    private void Awake()
    {
        col = GetComponent<Collider2D>();

        DisableCollider();
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }

    private void DisableCollider()
    {
        col.enabled = false;
    }

    private void Init(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Hurt(damage);
        }
    }
}
