using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : Enemy
{
    [SerializeField]
    protected float baseSpeed = 5f,
        baseHealth = 1000f,
        baseDamage = 10f;

    protected override void Awake()
    {
        _stat = new Stat(baseHealth, baseDamage, baseSpeed);
    }

    protected override void Start()
    {
        base.Start();
    }

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
                + (direction.normalized * Time.deltaTime * _stat.GetStat(Stat.Type.speed))
        );
    }
}
