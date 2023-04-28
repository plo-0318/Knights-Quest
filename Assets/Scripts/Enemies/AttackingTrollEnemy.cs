using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackingTrollEnemy : TrollEnemy
{
    protected bool isAttacking;

    [Header("Projectile Attack")]
    [SerializeField]
    private Transform projectileSpawnPos;

    protected override void Awake()
    {
        base.Awake();

        isAttacking = false;
    }

    protected override void PerformSpecialMove()
    {
        if (isCharging || isAttacking)
        {
            return;
        }

        if (specialMoveCoolDownTimer <= 0)
        {
            int rand = UnityEngine.Random.Range(0, 2);

            if (rand == 0)
            {
                Charge();
            }
            else
            {
                Attack();
            }
        }
    }

    protected virtual void Attack()
    {
        if (spriteRenderer.TryGetComponent<TrollAnimatorController>(out var animatorController))
        {
            if (isAttacking)
            {
                return;
            }

            isAttacking = true;

            float attackAnimationLength = animatorController.attackAnimationLength;

            int rand = UnityEngine.Random.Range(0, 2);

            if (rand == 0)
            {
                StartCoroutine(HandleAttack(attackAnimationLength, SpawnBolt));
            }
            else
            {
                StartCoroutine(HandleAttack(attackAnimationLength, SpawnWaterSpike));
            }
        }
    }

    protected IEnumerator HandleAttack(float duration, Action spawnProjectile)
    {
        canMove = false;

        float halfDuration = duration / 2;

        yield return new WaitForSeconds(halfDuration);

        spawnProjectile();

        yield return new WaitForSeconds(halfDuration);

        isAttacking = false;
        canMove = true;

        specialMoveCoolDownTimer = specialMoveCoolDownTime;
    }

    protected void SpawnBolt()
    {
        float angle = Util.GetNormalizedAngle(transform.position, playerTrans.position);

        Projectile p1 = EnemyBossAttackUtil.SpawnBolt(projectileSpawnPos.position, angle);
        Projectile p2 = EnemyBossAttackUtil.SpawnBolt(projectileSpawnPos.position, angle + 20f);
        Projectile p3 = EnemyBossAttackUtil.SpawnBolt(projectileSpawnPos.position, angle - 20f);

        p1.Init(Util.GetDirectionFromAngle(angle));
        p2.Init(Util.GetDirectionFromAngle(angle + 20f));
        p3.Init(Util.GetDirectionFromAngle(angle - 20f));

        soundManager.PlayClip(soundManager.audioRefs.sfxBoltBurst);
    }

    protected void SpawnWaterSpike()
    {
        int numSpikes = 4;
        float blinkDuration = 1f;
        float minDistance = 0.5f;
        float maxDistance = 4f;

        Vector3[] randomPos = Util.GenerateRandomPositions(
            playerTrans.position,
            numSpikes,
            minDistance,
            maxDistance
        );

        foreach (Vector3 pos in randomPos)
        {
            EnemyBossAttackUtil.SpawnWaterSpike(pos, blinkDuration);
        }

        StartCoroutine(PlayWaterSpikeSFX(blinkDuration));
    }

    protected IEnumerator PlayWaterSpikeSFX(float delay)
    {
        yield return new WaitForSeconds(delay);

        soundManager.PlayClip(soundManager.audioRefs.sfxWaterSpike);
    }

    public override bool IsAttacking() => !isDead && isAttacking;
}
