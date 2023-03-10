using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    [SerializeField]
    protected float speed = 5f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Move();
    }

    private void Move()
    {
        Vector2 direction = playerTrans.position - transform.position;

        rb.MovePosition(
            (Vector2)transform.position + (direction.normalized * Time.deltaTime * speed)
        );
    }
}
