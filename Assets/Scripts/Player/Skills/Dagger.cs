using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    private float damage,
        speed;
    private bool piercing;

    [SerializeField]
    private GameObject onHitFx;

    private Rigidbody2D rb;

    private float destroyTimer;
    private float destroyTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        destroyTimer = 0f;
        destroyTime = 1.5f;
    }

    private void Update()
    {
        if (destroyTimer >= destroyTime)
        {
            Destroy(gameObject);
        }

        destroyTimer += Time.deltaTime;
    }

    public void Init(float damage, Vector2 velocity, bool piercing = false)
    {
        this.damage = damage;
        rb.velocity = velocity;
        this.piercing = piercing;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Hurt(damage, onHitFx, transform.position);

            if (!piercing)
            {
                Destroy(gameObject);
            }
        }
    }
}
