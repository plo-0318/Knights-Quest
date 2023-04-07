using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    [Header("Base stats")]
    [SerializeField]
    protected float baseSpeed = 5f;

    [SerializeField]
    protected float baseHealth = 1000f;

    [SerializeField]
    protected float baseDamage = 100f;

    protected float knockBackDuration = 0.3f;
    protected float knockBackForce = 5f;

    protected bool canMove = false;

    protected Coroutine knockBackCoroutine;

    protected override void Awake()
    {
        _stat = new Stat(baseHealth, baseDamage, baseSpeed);

        knockBackCoroutine = null;
    }

    protected override void Start()
    {
        base.Start();

        canMove = true;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector2 direction = playerTrans.position - transform.position;

        rb.MovePosition(
            (Vector2)transform.position
                + (direction.normalized * Time.deltaTime * _stat.GetStat(Stat.SPEED))
        );
    }

    public virtual void KnockBack()
    {
        KnockBack(playerTrans.position);
    }

    public virtual void KnockBack(Vector3 fromPos)
    {
        Vector3 pointOnCollider = col.bounds.ClosestPoint(fromPos);
        Vector3 direction = transform.position - pointOnCollider;

        canMove = false;
        rb.velocity = direction.normalized * knockBackForce;

        knockBackCoroutine = StartCoroutine(RecoverFromKnockBack());
    }

    protected virtual IEnumerator RecoverFromKnockBack()
    {
        float elapsedTime = 0;

        while (elapsedTime < knockBackDuration)
        {
            if (isDead)
            {
                if (knockBackCoroutine != null)
                {
                    StopCoroutine(knockBackCoroutine);
                }
                rb.velocity = Vector2.zero;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector2.zero;
        canMove = true;
    }

    protected override void OnKilledByPlayer()
    {
        base.OnKilledByPlayer();
    }

    protected override void ProcessDeath()
    {
        canMove = false;
        rb.velocity = Vector2.zero;

        base.ProcessDeath();
    }

    public override bool IsDead() => isDead;

    public override bool IsIdle() => false;

    public override bool IsMoving() => !isDead;

    //TODO: Delete this
    public void TEST_DisableMovement()
    {
        canMove = false;
    }
}
