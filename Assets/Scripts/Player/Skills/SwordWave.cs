using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWave : MonoBehaviour
{
    private Rigidbody2D rb;
    private float aliveTime;
    private float aliveTimer;
    private float damage;

    [SerializeField]
    private GameObject onHitFx;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        aliveTime = 1.2f;

        aliveTimer = 0;
    }

    private void Update()
    {
        if (aliveTimer >= aliveTime)
        {
            Destroy(gameObject);
            return;
        }

        aliveTimer += Time.deltaTime;
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
            enemy.Hurt(damage, onHitFx, transform.position);
        }
    }
}
