using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EnemyBossAttackUtil
{
    private static Projectile boltPrefab;
    private static DelayedHazard waterSpikePrefab;
    private static SlowField slowFieldPrefab;
    private static DelayedHazard lightningPrefab;
    private static DelayedHazard lightningPrefab2;

    static EnemyBossAttackUtil()
    {
        boltPrefab = Resources.Load<Projectile>("Enemy Skills/boss bolt");
        waterSpikePrefab = Resources.Load<DelayedHazard>("Enemy Skills/water spike");
        slowFieldPrefab = Resources.Load<SlowField>("Enemy Skills/slow field");
        lightningPrefab = Resources.Load<DelayedHazard>("Enemy Skills/lightning");
        lightningPrefab2 = Resources.Load<DelayedHazard>("Enemy Skills/lightning2");
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

    private static void SpawnEnemySkillWithIndicator(
        UnityEngine.Object skillPrefab,
        Vector3 spawnPos,
        float blinkDuration,
        Action additionalCallback = null
    )
    {
        AnimationUtil.SpawnSkillIndicator(
            spawnPos,
            blinkDuration,
            () =>
            {
                GameObject.Instantiate(
                    skillPrefab,
                    spawnPos,
                    Quaternion.identity,
                    GameManager.GameSession().enemyParent
                );

                if (additionalCallback != null)
                {
                    additionalCallback();
                }
            }
        );
    }

    public static void SpawnWaterSpike(Vector3 spawnPos, float blinkDuration)
    {
        Vector3 spawnOffset = new Vector2(0.1f, 0.67f);

        SpawnEnemySkillWithIndicator(waterSpikePrefab, spawnPos + spawnOffset, blinkDuration);
    }

    public static void SpawnSlowField(Vector3 spawnPos, float blinkDuration)
    {
        SpawnEnemySkillWithIndicator(slowFieldPrefab, spawnPos, blinkDuration);
    }

    public static void SpawnLightning(Vector3 spawnPos, float blinkDuration, int lightningIndex = 0)
    {
        DelayedHazard lightningToSpawn;

        if (lightningIndex == 0)
        {
            lightningToSpawn = lightningPrefab;
        }
        else
        {
            lightningToSpawn = lightningPrefab2;
        }

        SpawnEnemySkillWithIndicator(lightningToSpawn, spawnPos, blinkDuration);
    }
}
