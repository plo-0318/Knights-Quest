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

        rb.velocity = Vector2.zero;

        transform.position = Vector2.MoveTowards(
            transform.position,
            playerTrans.position,
            Time.deltaTime * speed
        );
    }
}
