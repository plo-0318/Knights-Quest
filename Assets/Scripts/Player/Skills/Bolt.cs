using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float damage;
    private float speed;
    private float aliveTime,
        aliveTimer;
    private Color maxLevelColor;

    private GameObject enemyOnHitVFX;
    private AudioClip enemyOnHitSFX;
    private BoltBurst boltBurstPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        aliveTime = 1.5f;
        aliveTimer = 0;

        maxLevelColor = new Color(255 / 255f, 143 / 255f, 149 / 255f);

        boltBurstPrefab = null;
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

    public void Init(
        float damage,
        float speed,
        Vector2 direction,
        GameObject enemyOnHitVFX,
        AudioClip enemyOnHitSFX,
        int level,
        BoltBurst boltBurstPrefab
    )
    {
        this.damage = damage;
        rb.velocity = direction * speed;

        this.enemyOnHitVFX = enemyOnHitVFX;
        this.enemyOnHitSFX = enemyOnHitSFX;

        if (level >= 5)
        {
            spriteRenderer.color = maxLevelColor;
            this.boltBurstPrefab = boltBurstPrefab;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Hurt(damage, enemyOnHitVFX, transform.position, enemyOnHitSFX);

            if (boltBurstPrefab != null)
            {
                BoltBurst spawnBoltBurst = Instantiate(
                    boltBurstPrefab,
                    transform.position,
                    Quaternion.identity
                );

                spawnBoltBurst.Init(damage / 3f);
            }

            Destroy(gameObject);
        }
    }
}
