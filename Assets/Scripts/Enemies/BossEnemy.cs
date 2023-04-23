using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossEnemy : Enemy
{
    [Header("Base stats")]
    [SerializeField]
    protected float baseSpeed;

    [SerializeField]
    protected float baseHealth;

    [SerializeField]
    protected float baseDamage;

    protected override void Awake()
    {
        _stat = new Stat(baseHealth, baseDamage, baseSpeed);
    }

    protected override void OnKilledByPlayer()
    {
        base.OnKilledByPlayer();
    }

    protected override void ProcessDeath()
    {
        gameSession.HandleBossFightEnd();

        base.ProcessDeath();
    }
}
