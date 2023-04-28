using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyBossAttackUtil
{
    private static Projectile boltPrefab;
    private static DelayedHazard waterSpikePrefab;

    static EnemyBossAttackUtil()
    {
        boltPrefab = Resources.Load<Projectile>("Enemy Projectiles/boss bolt");
        waterSpikePrefab = Resources.Load<DelayedHazard>("Enemy Projectiles/water spike");
    }

    public static Projectile SpawnBolt(Vector3 spawnPos, float angle)
    {
        Projectile bolt = GameObject.Instantiate(
            boltPrefab,
            spawnPos,
            Quaternion.identity,
            GameManager.GameSession().enemyParent
        );

        bolt.transform.rotation = Quaternion.Euler(0, 0, angle);

        return bolt;
    }

    public static void SpawnWaterSpike(Vector3 spawnPos, float blinkDuration)
    {
        Vector3 spawnOffset = new Vector2(0.1f, 0.67f);

        AnimationUtil.SpawnSkillIndicator(
            spawnPos,
            blinkDuration,
            () =>
            {
                GameObject.Instantiate(
                    waterSpikePrefab,
                    spawnPos + spawnOffset,
                    Quaternion.identity,
                    GameManager.GameSession().enemyParent
                );
            }
        );
    }
}
