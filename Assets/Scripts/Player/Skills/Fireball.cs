using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float damage,
        speed;
    private Vector2 targetPos;
    private Rigidbody2D rb;

    private float destroyTimer;
    private GameObject explosionPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        destroyTimer = 1.5f;
    }

    private void Update()
    {
        if (ReachedTarget())
        {
            SpawnExplosion();
            Destroy(gameObject);
        }

        if (destroyTimer <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            destroyTimer -= Time.deltaTime;
        }
    }

    public void Init(float damage, Vector2 velocity, Vector2 targetPos, GameObject explosionPrefab)
    {
        this.damage = damage;
        rb.velocity = velocity;
        this.targetPos = targetPos;
        this.explosionPrefab = explosionPrefab;
    }

    private bool ReachedTarget()
    {
        return Vector2.Distance(transform.position, targetPos) < 0.1f;
    }

    private void SpawnExplosion()
    {
        FireballExplosion spawnedExplosion = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity,
                GameManager.GameSession().skillParent
            )
            .GetComponent<FireballExplosion>();
    }
}
