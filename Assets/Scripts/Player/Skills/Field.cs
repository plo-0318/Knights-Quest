using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private CircleCollider2D col;
    private Vector3 BASE_SCALE;
    private float damage;
    private float cooldownTime;
    private float cooldownTimer;

    private Modifier speedMod;
    private float speedMultiplier;
    private bool slowEnemy;

    private float fadeOutSpeed;

    private void Awake()
    {
        speedMultiplier = -0.5f;

        speedMod = new Modifier(Stat.SPEED, "SkillField", speedMultiplier);

        BASE_SCALE = new Vector3(
            transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        col = GetComponent<CircleCollider2D>();

        cooldownTimer = 0;

        slowEnemy = false;

        fadeOutSpeed = 0.3f;
    }

    private void Start()
    {
        GameManager.GameSession().onGameLost += () =>
        {
            StartCoroutine(FadeOut());
        };
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public void Init(float damage, float cooldownTime)
    {
        this.damage = damage;
        this.cooldownTime = cooldownTime;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetScale(float multiplier)
    {
        transform.localScale = BASE_SCALE * multiplier;
    }

    public void SetSlowEnemy(bool slowEnemy)
    {
        this.slowEnemy = slowEnemy;
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

            if (slowEnemy)
            {
                enemy.AddModifier(speedMod, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            if (slowEnemy)
            {
                enemy.RemoveModifier(speedMod);
            }
        }
    }

    private IEnumerator FadeOut()
    {
        col.enabled = false;

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        while (sprite.color.a > 0)
        {
            float alpha = sprite.color.a - Time.deltaTime * fadeOutSpeed;

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);

            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameManager.GameSession().RemoveModifierFromAllEnemies(speedMod);
    }

    //TODO: Delete this
    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(
    //         transform.position + (Vector3)col.offset,
    //         col.radius * transform.localScale.x
    //     );
    // }
}
