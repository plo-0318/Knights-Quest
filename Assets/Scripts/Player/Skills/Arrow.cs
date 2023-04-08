using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float damage,
        speed;

    private Rigidbody2D rb;
    private Modifier speedModifier;

    private float destroyTimer;
    private float destroyTime;

    private SoundManager soundManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        soundManager = GameManager.SoundManager();
    }

    private void Start()
    {
        destroyTimer = 0f;
        destroyTime = 3f;
    }

    private void Update()
    {
        if (destroyTimer >= destroyTime)
        {
            Destroy(gameObject);
        }

        destroyTimer += Time.deltaTime;
    }

    public void Init(float damage, Vector2 velocity, Modifier modifier)
    {
        this.damage = damage;
        rb.velocity = velocity;
        speedModifier = modifier;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (speedModifier != null)
            {
                enemy.AddModifier(speedModifier);
            }

            enemy.Hurt(damage, soundManager.audioRefs.sfxEnemyHurtArrow);
        }
    }
}
