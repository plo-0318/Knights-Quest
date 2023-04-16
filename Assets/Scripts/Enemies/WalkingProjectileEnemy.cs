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
    protected float projectileDamage;

    [SerializeField]
    protected float attackCooldownTime;
    protected float attackCooldownTimer;

    protected Coroutine handleAttackCoroutine;

    protected WalkingProjectileEnemyAnimatiorController animatorController;

    protected override void Awake()
    {
        base.Awake();
        attackCooldownTimer = attackCooldownTime;

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

        SpawnProjectile();

        handleAttackCoroutine = StartCoroutine(HandleAttackAnimation());
    }

    protected virtual void SpawnProjectile()
    {
        //TODO: add code for spawning the projectile here
    }

    protected virtual IEnumerator HandleAttackAnimation()
    {
        float duration = animatorController.AttackAnimationLength;

        yield return new WaitForSeconds(duration);

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

    public bool IsAttacking()
    {
        return isAttacking;
    }
}
