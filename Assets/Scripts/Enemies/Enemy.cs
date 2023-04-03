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
    [Tooltip("The enemy sprite renderer")]
    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [Tooltip("The enemy to enemy collider")]
    [SerializeField]
    protected Collider2D enemyBodyCollider;
    protected CollectableSpawner collectableSpawner;

    protected Stat _stat;
    protected bool isDead;

    protected SoundManager soundManager;

    protected virtual void Awake()
    {
        _stat = new Stat();
    }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collectableSpawner = GetComponent<CollectableSpawner>();

        gameSession = GameManager.GameSession();
        playerTrans = GameManager.PlayerMovement().transform;
        soundManager = GameManager.SoundManager();

        if (TryGetComponent<Collider2D>(out Collider2D col))
        {
            this.col = col;
        }

        gameSession.OnEnemySpawn(this);
        gameSession.onKillAllEnemies += ProcessDeath;
        gameSession.onRemoveModifier += stat.RemoveModifier;
    }

    protected virtual void FixedUpdate()
    {
        Flip();
    }

    protected virtual void OnDestroy()
    {
        gameSession.OnEnemyDestroy(this);
        gameSession.onKillAllEnemies -= ProcessDeath;
        gameSession.onRemoveModifier -= stat.RemoveModifier;
    }

    protected void Flip()
    {
        Vector3 newScale = new Vector3(
            playerTrans.position.x < transform.position.x ? -1f : 1f,
            transform.localScale.y,
            transform.localScale.z
        );

        spriteRenderer.transform.localScale = newScale;
    }

    public virtual void Init(Modifier[] modifiers)
    {
        Stat.ApplyModifiers(_stat, modifiers);
    }

    public float GetStat(int statType)
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

        DamagePopup.ShowDamagePopup(amount, transform, Quaternion.identity);

        soundManager.PlayClip(soundManager.audioRefs.sfxEnemyHurt);

        // Get the new health
        float newHealth = _stat.ModifyHealth(-amount);

        if (newHealth <= 0)
        {
            OnKilledByPlayer();
            ProcessDeath();
        }
    }

    protected virtual void OnKilledByPlayer()
    {
        GameManager.PlayerStatus().stat.IncrementKillCount();

        collectableSpawner.SpawnRandomCollectable(transform.position, Quaternion.identity);
    }

    protected virtual void ProcessDeath()
    {
        // Setting isDead --> play the death animation
        isDead = true;
        col.enabled = false;

        //TODO: Decide whether to keep enemy death sound
        // soundManager.PlayClip(soundManager.audioRefs.sfxEnemyDeath);

        // Try to get the length of the death animation
        if (spriteRenderer.TryGetComponent<AnimatorController>(out var animatorController))
        {
            OnDeath(animatorController.deathAnimationLength);
        }
        else
        {
            OnDeath();
        }
    }

    // Deestroy the gameobject after the death animation finishes
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
