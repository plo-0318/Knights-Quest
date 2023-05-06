using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollEnemy : BossEnemy, IAnimatableTroll
{
    protected bool canMove;

    [Header("Special Move")]
    [SerializeField]
    protected float specialMoveCoolDownTime;
    protected float specialMoveCoolDownTimer;

    // CHARGING
    protected bool isCharging;
    protected Coroutine handleChargeCoroutine;

    [Header("Charging")]
    [SerializeField]
    protected float chargeDuration;

    [SerializeField]
    protected float chargeSpeedMultiplier;
    protected float chargeIndicatorEndScale = 15f;
    protected float chargeIndicatorExtendDuration = 0.5f;

    [SerializeField]
    protected float chargeIndicatorBlinkDuration;

    protected override void Awake()
    {
        base.Awake();

        canMove = true;

        specialMoveCoolDownTimer = specialMoveCoolDownTime / 2;

        isCharging = false;

        GameManager.PlayerStatus().onPlayerHurt += HandleChargeHitPlayer;
    }

    protected virtual void Update()
    {
        if (isDead || isCharging)
        {
            return;
        }

        specialMoveCoolDownTimer -= Time.deltaTime;
    }

    protected override void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        base.FixedUpdate();

        PerformSpecialMove();
        Move();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameManager.PlayerStatus().onPlayerHurt -= HandleChargeHitPlayer;
    }

    private void Move()
    {
        if (!canMove || GameManager.PlayerMovement().IsDead())
        {
            return;
        }

        Vector2 direction = playerTrans.position - transform.position;

        float multiplier = isCharging ? chargeSpeedMultiplier : 1f;

        rb.MovePosition(
            (Vector2)transform.position
                + (direction.normalized * Time.deltaTime * _stat.GetStat(Stat.SPEED) * multiplier)
        );
    }

    protected virtual void PerformSpecialMove()
    {
        if (isCharging)
        {
            return;
        }

        if (specialMoveCoolDownTimer <= 0)
        {
            Charge();
        }
    }

    protected virtual void Charge()
    {
        canMove = false;
        isCharging = true;

        ChargeIndicator indicator = AnimationUtil.SpawnChargeIndicator(
            transform,
            Util.GetNormalizedAngle(transform.position, playerTrans.position)
        );

        indicator.SetDurations(chargeIndicatorExtendDuration, chargeIndicatorBlinkDuration);
        indicator.SetEndScale(chargeIndicatorEndScale);
        indicator.Follow(transform, playerTrans);
        indicator.Play();

        handleChargeCoroutine = StartCoroutine(HandleCharge(indicator.TotalDuration));
    }

    protected void HandleChargeEnd()
    {
        isCharging = false;
        canMove = true;

        specialMoveCoolDownTimer = specialMoveCoolDownTime;
    }

    protected virtual IEnumerator HandleCharge(float delay)
    {
        yield return new WaitForSeconds(delay);

        canMove = true;

        yield return new WaitForSeconds(chargeDuration);

        HandleChargeEnd();
    }

    protected virtual void HandleChargeHitPlayer(Enemy enemy)
    {
        // If the troll hit the player during a charge, stop charging
        if (isCharging && enemy == this && handleChargeCoroutine != null)
        {
            StopCoroutine(handleChargeCoroutine);
            HandleChargeEnd();
        }
    }

    public override bool IsDead() => isDead;

    public override bool IsIdle() => false;

    public override bool IsMoving() => !isDead && !isCharging && canMove;

    public bool IsCharging() => !isDead && isCharging;

    public virtual bool IsAttacking() => false;
}
