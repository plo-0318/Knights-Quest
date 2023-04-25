using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        gameSession.onKillAllEnemies += HandleForceKill;
        gameSession.onRemoveModifier += _stat.RemoveModifier;
    }

    protected virtual void FixedUpdate()
    {
        Flip();
    }

    protected virtual void OnDestroy()
    {
        gameSession.OnEnemyDestroy(this);
        gameSession.onKillAllEnemies -= HandleForceKill;
        gameSession.onRemoveModifier -= _stat.RemoveModifier;
    }

    protected void Flip()
    {
        if (isDead)
        {
            return;
        }

        Vector3 newScale = new Vector3(
            playerTrans.position.x < transform.position.x ? -1f : 1f,
            spriteRenderer.transform.localScale.y,
            spriteRenderer.transform.localScale.z
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

    public virtual void Hurt(float amount, AudioClip onHitSfx = null)
    {
        Hurt(amount, null, onHitSfx);
    }

    public virtual void Hurt(float amount, GameObject onHitFx, AudioClip onHitSfx = null)
    {
        Hurt(amount, onHitFx, (Vector2)transform.position, onHitSfx);
    }

    public virtual void Hurt(
        float amount,
        GameObject onHitFx,
        Vector2 fromPos,
        AudioClip onHitSfx = null
    )
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

        AudioClip _onHitSfx = onHitSfx == null ? soundManager.audioRefs.sfxEnemyHurt : onHitSfx;

        soundManager.PlayClip(_onHitSfx, SoundManager.TimedSFX.ENEMY_HURT);

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
        GameManager.PlayerStatus().IncrementKillCount();

        collectableSpawner.SpawnRandomCollectables(transform.position, Quaternion.identity);
    }

    protected virtual void HandleForceKill(bool deathByPlayer)
    {
        if (!deathByPlayer)
        {
            ProcessDeath();
        }
        else
        {
            OnKilledByPlayer();
            ProcessDeath();
        }
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

            StartCoroutine(
                AnimationUtil.DecreaseScaleOverTime(
                    animatorController.deathAnimationLength,
                    spriteRenderer.transform
                )
            );
            StartCoroutine(
                AnimationUtil.FallOver(
                    animatorController.deathAnimationLength * 0.4f,
                    spriteRenderer.transform
                )
            );
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

        StopAllCoroutines();
        Destroy(gameObject, destroyTime);
    }

    public void AddModifier(Modifier modifier, bool replaceIfExits = true)
    {
        _stat.AddModifier(modifier, replaceIfExits);
    }

    public void RemoveModifier(Modifier modifier)
    {
        _stat.RemoveModifier(modifier);
    }

    public abstract bool IsDead();

    public abstract bool IsIdle();

    public abstract bool IsMoving();
}
