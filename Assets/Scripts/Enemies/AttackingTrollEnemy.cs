using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackingTrollEnemy : TrollEnemy
{
    protected bool isAttacking;

    [Header("Projectile Attack")]
    [SerializeField]
    protected Transform projectileSpawnPos;

    [Header("Spawn Hazards")]
    [SerializeField]
    protected bool _spawnHazard;

    [SerializeField]
    protected float hazardCooldownTime;
    protected float hazardCooldownTimer;
    protected bool isSpawningHazard;

    protected override void Awake()
    {
        base.Awake();

        isAttacking = false;

        hazardCooldownTimer = hazardCooldownTime;
        isSpawningHazard = false;
    }

    protected override void Update()
    {
        if (isDead || isCharging || isAttacking)
        {
            return;
        }

        specialMoveCoolDownTimer -= Time.deltaTime;

        // If cannot spawn hazard or hazard is spawning, return
        if (!_spawnHazard || isSpawningHazard)
        {
            return;
        }

        hazardCooldownTimer -= Time.deltaTime;
    }

    protected override void PerformSpecialMove()
    {
        // If currently not performing special moves (charge or use skills),
        // check if special move is off cooldown
        if (!isCharging && !isAttacking)
        {
            // If special move is off cooldown, use a random special move
            if (specialMoveCoolDownTimer <= 0)
            {
                UseSpecialMove();
            }
        }

        // If can spawn hazard AND not spawning any hazards currently, check if spawn hazard is off cooldown
        if (_spawnHazard && !isSpawningHazard)
        {
            // If spawn hazard is off cooldown, spawn hazard
            if (hazardCooldownTimer <= 0)
            {
                SpawnHazard();
            }
        }
    }

    ////////////////////////// ////// //////////////////////////

    ////////////////////////// SKILLS //////////////////////////
    protected virtual void UseSpecialMove()
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

        if (_spawnHazard)
        {
            Projectile p4 = EnemyBossAttackUtil.SpawnBolt(projectileSpawnPos.position, angle + 40f);
            Projectile p5 = EnemyBossAttackUtil.SpawnBolt(projectileSpawnPos.position, angle - 40f);

            p4.Init(Util.GetDirectionFromAngle(angle + 40f));
            p5.Init(Util.GetDirectionFromAngle(angle - 40f));
        }

        p1.Init(Util.GetDirectionFromAngle(angle));
        p2.Init(Util.GetDirectionFromAngle(angle + 20f));
        p3.Init(Util.GetDirectionFromAngle(angle - 20f));

        soundManager.PlayClip(soundManager.audioRefs.sfxBoltBurst);
    }

    protected void SpawnWaterSpike()
    {
        int numSpikes = 6;
        float blinkDuration = 1f;
        float radius = 3f;

        Vector3[] positions = Util.GeneratePositionsAround(playerTrans.position, numSpikes, radius);

        EnemyBossAttackUtil.SpawnWaterSpike(playerTrans.position, blinkDuration);

        foreach (Vector3 pos in positions)
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

    ////////////////////////// ////// //////////////////////////

    ////////////////////////// HAZARD //////////////////////////
    protected void SpawnHazard()
    {
        if (BossBorder.bossBorder == null)
        {
            return;
        }

        int rand = UnityEngine.Random.Range(0, 2);

        isSpawningHazard = true;

        if (rand == 0)
        {
            SpawnFieldHazard();
        }
        else
        {
            SpawnLightning();
        }
    }

    protected void SpawnFieldHazard()
    {
        int numSlowFields = 10;
        float radius = 7.5f;

        Vector3[] positions = Util.GeneratePositionsAround(
            BossBorder.GetBossBorderPos(),
            numSlowFields,
            radius
        );

        foreach (Vector3 pos in positions)
        {
            EnemyBossAttackUtil.SpawnSlowField(pos, 1f);
        }

        isSpawningHazard = false;
        hazardCooldownTimer = hazardCooldownTime;
    }

    protected void SpawnLightning()
    {
        StartCoroutine(HandleSpawnLightning());
    }

    protected void SpawnLightningFromPos(Vector3[] positions)
    {
        foreach (var pos in positions)
        {
            EnemyBossAttackUtil.SpawnLightning(pos, 0.5f);
        }

        soundManager.PlayClip(soundManager.audioRefs.sfxLightning);
    }

    protected IEnumerator HandleSpawnLightning()
    {
        int numLightnings1 = 6;
        int numLightnings2 = 10;
        int numLightnings3 = 14;
        int numLightnings4 = 18;

        float radius1 = 3f;
        float radius2 = 4.5f;
        float radius3 = 6f;
        float radius4 = 7.5f;

        Vector3 bossBorderPos = BossBorder.GetBossBorderPos();

        Vector3[] pos1 = Util.GeneratePositionsAround(bossBorderPos, numLightnings1, radius1);
        Vector3[] pos2 = Util.GeneratePositionsAround(bossBorderPos, numLightnings2, radius2);
        Vector3[] pos3 = Util.GeneratePositionsAround(bossBorderPos, numLightnings3, radius3);
        Vector3[] pos4 = Util.GeneratePositionsAround(bossBorderPos, numLightnings4, radius4);

        SpawnLightningFromPos(pos1);

        yield return new WaitForSeconds(1f);

        SpawnLightningFromPos(pos2);

        yield return new WaitForSeconds(1f);

        SpawnLightningFromPos(pos3);

        yield return new WaitForSeconds(1f);

        SpawnLightningFromPos(pos4);

        yield return new WaitForSeconds(0.5f);

        isSpawningHazard = false;
        hazardCooldownTimer = hazardCooldownTime;
    }

    public override bool IsAttacking() => !isDead && isAttacking;
}
