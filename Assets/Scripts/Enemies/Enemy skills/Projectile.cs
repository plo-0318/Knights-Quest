using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float damage;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float aliveTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
        }

        aliveTime -= Time.deltaTime;
    }

    public void Init(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerCollider>() != null)
        {
            GameManager.PlayerStatus().Hurt(damage);

            Destroy(gameObject);
        }
    }
}
