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
    protected float baseDamage = 10f;

    protected bool canMove = false;

    protected override void Awake()
    {
        _stat = new Stat(baseHealth, baseDamage, baseSpeed);
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
                + (direction.normalized * Time.deltaTime * _stat.GetStat(Stat.Type.SPEED))
        );
    }

    protected override void ProcessDeath()
    {
        canMove = false;

        base.ProcessDeath();
    }

    public override bool IsDead() => isDead;

    public override bool IsIdle() => false;

    public override bool IsMoving() => !isDead;
}
