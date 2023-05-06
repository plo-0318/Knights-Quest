using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class EnemyModifier : Modifier
{
    public enum ApplyTo
    {
        All,
        NormalEnemyOnly,
        BossEnemyOnly
    }

    public ApplyTo applyTo = ApplyTo.All;

    public EnemyModifier(Stat.StatType type, string name, float multiplier, float duration = 0)
        : base(type, name, multiplier, duration) { }

    public EnemyModifier(int type, string name, float multiplier, float duration = 0)
        : base(type, name, multiplier, duration) { }
}
