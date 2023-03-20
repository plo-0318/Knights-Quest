using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IAnimatable
{
    protected GameSession gameSession;
    protected Transform playerTrans;
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Collider2D col;

    [Header("References")]
    [Tooltip("The enemy to enemy collider")]
    [SerializeField]
    protected Collider2D enemyBodyCollider;
    protected Stat _stat;
    protected bool isDead;

    protected virtual void Awake()
    {
        _stat = new Stat();
    }

    protected virtual void Start()
    {
        gameSession = GameManager.GameSession();
        playerTrans = GameManager.PlayerMovement().transform;

        rb = GetComponent<Rigidbody2D>();

        if (TryGetComponent<Collider2D>(out Collider2D col))
        {
            this.col = col;
        }

        gameSession.OnEnemySpawn(this);
        gameSession.OnKillAllEnemies += ProcessDeath;
        gameSession.OnRemoveModifier += stat.RemoveModifier;
    }

    protected virtual void FixedUpdate()
    {
        Flip();
    }

    protected virtual void OnDestroy()
    {
        gameSession.OnEnemyDestroy(this);
        gameSession.OnKillAllEnemies -= ProcessDeath;
        gameSession.OnRemoveModifier -= stat.RemoveModifier;
    }

    protected void Flip()
    {
        Vector3 newScale = new Vector3(
            playerTrans.position.x < transform.position.x ? -1f : 1f,
            transform.localScale.y,
            transform.localScale.z
        );

        transform.localScale = newScale;
    }

    public virtual void Init(Modifier[] modifiers)
    {
        Stat.ApplyModifiers(_stat, modifiers);
    }

    public float GetStat(Stat.Type statType)
    {
        return _stat.GetStat(statType);
    }

    public virtual void Hurt(float amount)
    {
        Hurt(amount, null);
    }

    public virtual void Hurt(float amount, GameObject onHitFx)
    {
        Hurt(amount, onHitFx, (Vector2)transform.position);
    }

    public virtual void Hurt(float amount, GameObject onHitFx, Vector2 fromPos)
    {
        // Spawn the on-hit special effects
        if (onHitFx != null)
        {
            Vector3 spawnPos = (Vector3)fromPos;

            if (col != null)
            {
                spawnPos = col.bounds.ClosestPoint(fromPos);
            }

            GameObject fx = Instantiate(onHitFx, spawnPos, Quaternion.identity);
            fx.transform.parent = gameObject.transform;
        }

        // Get the new health
        float newHealth = _stat.ModifyHealth(-amount);

        if (newHealth <= 0)
        {
            ProcessDeath();
        }
    }

    protected virtual void ProcessDeath()
    {
        isDead = true;
        col.enabled = false;
        GameManager.PlayerStat().IncrementKillCount();

        if (TryGetComponent<AnimatorController>(out var animatorController))
        {
            OnDeath(animatorController.deathAnimationLength);
        }
        else
        {
            OnDeath();
        }
    }

    protected void OnDeath(float destroyTime = 0)
    {
        if (enemyBodyCollider != null)
        {
            enemyBodyCollider.enabled = false;
        }

        Destroy(gameObject, destroyTime);
    }

    public abstract bool IsDead();

    public abstract bool IsIdle();

    public abstract bool IsMoving();

    public Stat stat => _stat;
}
