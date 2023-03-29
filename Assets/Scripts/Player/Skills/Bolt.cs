using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    private float damage,
        speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, Vector2 velocity)
    {
        this.damage = damage;
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Hurt(damage);

            Destroy(gameObject);
        }
    }
}
