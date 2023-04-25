using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyTest : BossEnemy
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Move();
    }

    private void Move()
    {
        Vector2 direction = playerTrans.position - transform.position;

        rb.MovePosition(
            (Vector2)transform.position
                + (direction.normalized * Time.deltaTime * _stat.GetStat(Stat.SPEED))
        );
    }

    public override bool IsDead() => isDead;

    public override bool IsIdle() => false;

    public override bool IsMoving() => !isDead;
}
