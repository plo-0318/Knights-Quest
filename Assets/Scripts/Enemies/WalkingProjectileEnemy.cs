using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingProjectileEnemy : WalkingEnemy, IAttackingAnimatable
{
    protected bool isAttacking;

    [Header("Projectile setting")]
    [SerializeField]
    protected GameObject projectilePrefab;

    [SerializeField]
    protected Transform projectileSpawnPos;

    [SerializeField]
    protected float attackCooldownTime;
    protected float attackCooldownTimer;

    protected Coroutine handleAttackCoroutine;

    protected WalkingProjectileEnemyAnimatiorController animatorController;

    protected override void Awake()
    {
        base.Awake();
        attackCooldownTimer = 1f;

        animatorController = GetComponentInChildren<WalkingProjectileEnemyAnimatiorController>();
    }

    protected override void FixedUpdate()
    {
        if (isDead || isAttacking)
        {
            return;
        }

        Flip();

        if (CanAttack())
        {
            Attack();
        }

        if (canMove)
        {
            Move();
        }
    }

    protected virtual bool CanAttack()
    {
        if (attackCooldownTimer <= 0)
        {
            return true;
        }

        attackCooldownTimer -= Time.deltaTime;
        return false;
    }

    protected virtual void Attack()
    {
        isAttacking = true;

        handleAttackCoroutine = StartCoroutine(HandleSpawnProjectile(0.2f));
    }

    protected virtual void SpawnProjectile()
    {
        Instantiate(projectilePrefab, projectileSpawnPos.position, Quaternion.identity);
    }

    protected virtual IEnumerator HandleSpawnProjectile(float projectileSpawnDelay)
    {
        float animationDuration = animatorController.AttackAnimationLength;

        // If the animation duration is less than projectile spawn delay duration
        // Do not delay projectile spawn
        if (animationDuration <= projectileSpawnDelay)
        {
            SpawnProjectile();

            yield return new WaitForSeconds(animationDuration);
        }
        // Else, spawn the projectile after the delay duration
        // and wait until the end of the animation duration
        else
        {
            yield return new WaitForSeconds(projectileSpawnDelay);

            SpawnProjectile();

            yield return new WaitForSeconds(animationDuration - projectileSpawnDelay);
        }

        isAttacking = false;
        attackCooldownTimer = attackCooldownTime;
    }

    protected override void ProcessDeath()
    {
        isAttacking = false;

        if (handleAttackCoroutine != null)
        {
            StopCoroutine(handleAttackCoroutine);
        }

        base.ProcessDeath();
    }

    public bool IsAttacking() => isAttacking;
}
